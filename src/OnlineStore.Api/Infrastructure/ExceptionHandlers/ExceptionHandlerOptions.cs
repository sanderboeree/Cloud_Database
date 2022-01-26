using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OnlineStore.Api.Infrastructure.ExceptionHandlers
{
    public class ExceptionHandlerOptions
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public ExceptionHandlerOptions()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
