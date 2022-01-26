namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public static class ErrorCode
    {
        public static class HttpStatus500
        {
            public const string Default = "500.0";
            public const string DefaultMessage = "There is a server side problem, please contact system administrator";
        }

        public static class HttpStatus409
        {
            public const string Default = "409.0";
            public const string DefaultMessage = "Unable to process entity";

            public const string EntityExists = "409.1";
            public const string EntityExistsMessage = "Entity already exists";

            public const string EntityConcurrency = "409.2";
            public const string EntityConcurrencyMessage = "Entity out of date";
        }

        public static class HttpStatus404
        {
            public const string Default = "404.0";
            public const string DefaultMessage = "Requested resource not found";
        }

        public static class HttpStatus401
        {
            public const string Default = "401.0";
            public const string DefaultMessage = "Unauthorized request";
        }

        public static class HttpStatus403
        {
            public const string Default = "403.0";
            public const string DefaultMessage = "Request not allowed";

            public const string UserRights = "403.2";
            public const string UserRightsMessage = "User does not have sufficient rights.";

            public const string InvalidCurrentPassword = "403.13";
            public const string InvalidCurrentPasswordMessage = "Current password is invalid";
        }

        public static class HttpStatus400
        {
            public const string Default = "400.0";
            public const string DefaultMessage = "Validation error(s)";

            public const string RequiredValue = "400.2";
            public const string RequiredValueMessage = "Required value";

            public const string InvalidLength = "400.3";
            public const string InvalidLengthMessage = "Invalid length";

            public const string InvalidValue = "400.1";
            public const string InvalidValueMessage = "Invalid value";

            public const string InvalidDataType = "400.5";
            public const string InvalidDataTypeMessage = "Invalid data type";

            public const string InvalidEmailAddress = "400.13";
            public const string InvalidEmailAddressMessage = "Invalid e-mail address";
        }
    }
}
