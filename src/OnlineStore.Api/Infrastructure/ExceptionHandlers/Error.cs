using Newtonsoft.Json;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public class Error
    {
        /// <summary>
        /// The error code
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string ErrorCode { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string ErrorMessage { get; set; }

    }
}
