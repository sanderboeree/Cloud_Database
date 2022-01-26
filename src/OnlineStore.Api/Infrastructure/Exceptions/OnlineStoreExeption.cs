using System;

namespace OnlineStore.Api.Infrastructure.Exceptions
{
    public class OnlineStoreExeption : Exception
    {
        public string ErrorCode { get; set; }

        public OnlineStoreExeption(string errorCode, string errorMessage) : base(errorMessage)
        {
            ErrorCode = errorCode;
        }
    }
}