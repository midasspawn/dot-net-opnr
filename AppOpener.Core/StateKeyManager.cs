using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOpener.Core
{
    /// <summary>
    /// Represents all the objects keys
    /// </summary>
    public static class StateKeyManager
    {
        public const string SiteUrl = "Site:SiteUrl";
        public const string AbsoluteURl = "Site:AbsoluteUrl";

        public const string RelativeWebRoot = "Site:RelativeWebRoot";

		#region RegEx Expressions
		public const string AlphaNumericValidationRegEx = @"^[-_ .a-zA-Z0-9]+$"; //^[-_ a-zA-Z0-9]+$ //^[a-zA-Z0-9]*$
		public const string PasswordValidationRegEx = @"^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$";//(?=.*[A-Z])
        public const string EmailValidationRegEx = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string MobileValidationRegEx = @"^((\+)?(\d{2}(\-)?))?(\d{10}){1}?$";
		//public const string URlValidationRegEx = @"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
		public const string URlValidationRegEx = @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";
		public const string URlWithoutProtocolValidationRegEx = @"((http|https)://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        public const string GuidValidationRegEx = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
        public const string GuidEmptyValidationRegEx = @"^(\{{0,1}([0]){8}-([0]){4}-([0]){4}-([0]){4}-([0]){12}\}{0,1})$";
		public const string PinCodeValidationRegEx = @"^[1-9][0-9]{5}$";

		public const string DimensionValidationRegEx = @"^(\d+(\.\d+|)\s?x\s?\d+(\.\d+|)(\s?x\s?\d*(\.?\d+|))?)$";
		#endregion

		#region Area Names

		public static class AreaNames
        {
            public static string WebAdministrator { get { return "WebAdministrator"; } }
        }

        #endregion

        #region Security

        public const string EncryptionKey = "zJzpIuF54D"; // IMPORTANT: DO NOT CHANGE THIS VALUE. IF CHANGED THEN ALL AES DECRYPTIONS WOULD FAIL
        public const string TemporaryPassword = "123456";

        #endregion
    }
}
