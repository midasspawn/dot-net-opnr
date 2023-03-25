using AppOpener.Core.BusinessEntities.Client;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AppOpener.Core;
using AppOpener.Core.BusinessEntities.Configuration;
using AppOpener.Data.Interfaces;
using AppOpener.Data.Models;
using AppOpener.Services.Configuration;
using System.Linq;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace AppOpener.Services.Client
{
	public interface IClientService
	{
		ClientViewModel GetClientById(string client_Id);
		ClientTokenViewModel GetClientTokenById(string client_Id);
		void InsertToken(ClientTokenViewModel token);
		DataResult<ClientViewModel> ValidateClient(string client_Id, string client_Secret);
		
		bool ValidateWebJobToken(string token);
	}
    
	public class ClientService : IClientService
	{
        private readonly IMongoCollection<ClientViewModel> _clientModelCollection;
        private readonly IMongoCollection<ClientTokenViewModel> _clientTokenViewModelCollection;
		public ClientService(IOptions<MongoDbDatabaseSettings> mongoDbDatabaseSettings)
		{
           
            var mongoClient = new MongoClient(
              mongoDbDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoDbDatabaseSettings.Value.DatabaseName);

            _clientModelCollection = mongoDatabase.GetCollection<ClientViewModel>(
               mongoDbDatabaseSettings.Value.ClientCollectionName);

            _clientTokenViewModelCollection = mongoDatabase.GetCollection<ClientTokenViewModel>(
             mongoDbDatabaseSettings.Value.ClientTokenCollectionName);

        }
		public ClientTokenViewModel GetClientTokenById(string client_Id)
		{
			var clientToken = _clientTokenViewModelCollection.Find(x =>x.ClientId.Equals(client_Id)).SortByDescending(g=>g.IssuedOn).FirstOrDefault();
			if (clientToken != null)
			{
				var result = clientToken;
				return result;
			}
			else
				return null;
		}

		public void InsertToken(ClientTokenViewModel token)
		{
			var client = _clientTokenViewModelCollection.InsertOneAsync(token);
            client.Wait();
		}

		public DataResult<ClientViewModel> ValidateClient(string client_Id, string client_Secret)
		{
			var result = new DataResult<ClientViewModel>();

			var client = _clientModelCollection.Find(x => x.ClientKey.Equals(client_Id)).FirstOrDefault();

			// need to set appropriate messages
			if (client == null)
			{
				result.AddError("Client does not exist.");
				return result;
			}

			// validate the password
			var isValid = client.ClientSecret.ToLower() == client_Secret.ToLower() ? true : false;

			if (!isValid)
			{
				result.AddError("Invalid client.");
				return result;
			}

			var client_vm = client;
			if (client_vm != null)
			{
				result.Data = client_vm;
			}
			else
			{
				result.AddError("Invalid client.");
			}

			return result;
		}


		public ClientViewModel GetClientById(string client_Id)
		{
			var client = _clientModelCollection.Find(x => x.ClientKey.Equals(client_Id)).FirstOrDefault();

			return client;
		}
		public bool ValidateWebJobToken(string token)
		{
			return true;
			
		}

	}
   
}
