using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public class ApiError : Error
    {
        /// <summary>
        /// The reference id of the error
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string RefId { get; set; }

        /// <summary>
        /// Additional error details
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<ErrorDetails> ErrorDetails { get; set; }
    }
}
