using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utilities.Utils
{
    public static class ErrorCode
    {
        //Biến Success(hoàn thành)
        public static readonly int SUCCESS_EXECEPTION = 8;

        public static readonly int SYSTEM_EXECEPTION = -99;
        public static readonly int USER_NOT_EXISTS = -100;
        public static readonly int USER_OTP_EXPIRED = -101;
        public static readonly int USER_OTP_INVALID = -102;
        public static readonly int NOT_LOGIN = -103;
        public static readonly int USERNAME_OR_PASSWORD_INVALID = -104;
        public static readonly int USERNAME_OR_EMAIL_OR_MOBILE_EXISTED = -106;
        public static readonly int COINT_NOT_ENOUGH = -200;
        public static readonly int COIN_IS_NEGATIVE_OR_ZERO = -300;
        public static readonly int REPASSWORD_NOT_MATCH = -107;
        public static readonly int LIST_ID_EMPTY = -500;
        
        //SAP
        public static readonly int ADD_PO_PO_PROCESS_SUCCESS = 200;
        public static readonly int ADD_PO_PO_PROCESS_ERROR = 500;
        public static readonly int ADD_PO_PO_PROCESS_WRONG_ACC = 401;
        public static readonly int CANT_FIND_PO = 404;
        public static readonly int ADD_PO_MISSING_FIELD = 400;


    }
    public static class UserType
    {
        public static readonly int Admin = 1;
        public static readonly int Teacher = 2;
        public static readonly int Parent = 3;
        public static readonly int Student = 4;
        public static readonly int Manager = 5;
        public static readonly int Operation = 7;
        public static readonly int PCN = 8;//Phó chủ nhiệm
        public static readonly int BGD = 9;//Ban giám đốc
    }
    public static class GlobalVariable
    {
        public static readonly string APP_NAME = "MY_ADG";
        public static readonly string PO_APPROVE_LINK = "https://sap-approve-link.com";
        public static readonly string PO_REJECT_LINK = "https://sap-approve-link.com";
    }

    public class SapApiException : Exception
    {
        public int ErrorCode { get; }

        public SapApiException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }


}
