using AppOpener.Data.Interfaces;
using AppOpener.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppOpener.Services
{   public interface IReportService
    {
        Report GetReportServiceById(Int32 report_id);

    }
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork unitOfWork;

        public ReportService(IUnitOfWork unitOfWork_)
        {
            this.unitOfWork = unitOfWork_;
        }

        public Report GetReportServiceById(Int32 report_id)
        {
            var clientToken = unitOfWork.Repository<Report>().GetQuery().Include(x => x.ReportId)
                            .Where(x => x.ReportId.Equals(report_id)).OrderByDescending(x => x.ReportId).FirstOrDefault();
            if (clientToken != null)
            {
                var result = clientToken;
                return result;
            }
            else
                return null;
        }

    }
}
