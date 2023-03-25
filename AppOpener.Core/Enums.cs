using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Core
{
	public enum APIStatusCode
	{
		OK = 200,
		EntityDoesNotExists = 422,
		DuplicateEntity = 442,
		InvalidEmail = 443,
		InvalidPassword = 444,
		ErrorSendingMail = 445,
		EntityCannotBeDeleted = 446,
		SomethingWentWrong = 500,
		BadRequest = 400
	}

	public class APIStatusMessage
	{
		public const string EntityDoesNotExists = "Entity Does Not Exists.";
		public const string DuplicateEntity = "Record already exists.";
		public const string InvalidEmail = "Invalid Email-Id";
		public const string InvalidPassword = "Invalid Password";
		public const string ErrorSendingMail = "Error sending mail.";
		public const string EntityCannotBeDeleted = "Entity cannot be deleted.";
		public const string ServerError = "Oops ! something went wrong.";
		public const string RecordUpdated = "Record updated."; //"Record updated successfully.";
		public const string RecordInserted = "Record saved.";//"Record inserted successfully.";
		public const string BadRequest = "Bad Request";
		public const string RecordDeleted = "Record deleted.";//"Record deleted successfully.";
		public const string OtpSend = "OTP sent successfully.";

		public const string EntityCannotBeDeactivate = "Entity cannot be set as inactive.";
	}
	public class PaymentStatusMessage
	{
		public const string NoOrderGenerated = "No order generated with order id {0}";
		public const string PaymenetSignataureNotMatch = "Payment signature does not match.";
	}

	public enum SystemRole
	{
		SystemAdmin = 1,
		Customer = 2
	}

	public enum AddressType
	{
		BillingAddress = 1,
		ShippingAddress = 2
	}
	public enum OrderItemType
	{
		Essential = 1,
		NonEssential = 2
	}
	public enum PaymentGateway
	{
		RazorPay = 1
	}

	public enum PackageMode
	{
		COD = 1,
		Prepaid = 2
	}

	public enum Status
	{
		New = 1,
		ReadyToShip = 2,
		CancelOrder = 3,
		PickUpInitiated = 4,
		CancelPickup = 5,
		Manifested = 6,
		PickupPending = 7,
		PickupCompleted = 8,
		InTransit = 9,
		Exception = 10,
		OutForDelivery = 11,
		Delivered = 12,
		RTO = 13,
		RTOInTransit = 14,
		RTODelivered = 15
	}

	public enum PickupStatus
	{
		Initiated = 1,
		PartialPickup = 2,
		PickupCompleted = 3
	}

	public enum OTPStatus
	{
		Generated = 1,
		Regenerated = 2,
		Verified = 3,
		NotUsed = 4
	}
	public enum OTPType
	{
		ForgotPassword = 1
	}

	public enum BulkOrderRequestType
	{
		ExcelCreateOrder = 1
	}


	public enum NDRActionType
	{
		Raised = 1,
		ReAttempt = 2,
		ChangeAddress = 3,
		ChangeContactNumber = 4,
		RTO = 5,
		FakeAttempt = 6
	}

	public enum BulkOrderRequestStatus
	{
		Completed = 1,
		CompletedWithError = 2,
		Exception = 3
	}

	public enum ServiceProviderMode
	{
		Surface = 1,
		Air = 2,
		Water = 3
	}
	public enum ServiceProviderType
	{
		Normal = 1,
		Express = 2
	}

	public enum InvoiceStatus
	{
		Due = 1,
		Paid = 2
	}
	public enum CODStatus
	{
		Generated = 1,
		Remitted = 2
	}

	public enum WalletRecordType
	{
		WalletRecharge = 1,
		ShipmentCharge = 2,
		CODRemittance = 3,
		CODReversal = 4
	}
}
