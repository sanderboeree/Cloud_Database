using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public class ErrorDetails
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual string PropertyName { get; set; }
        public virtual ICollection<Error> Errors { get; set; }
    }
}
