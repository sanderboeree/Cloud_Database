
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace OnlineStore.Api.Infrastructure.Json
{
    public class OnlineStoreEnumNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return string.Concat(name
                    .Select((character, index) => index > 0 && char.IsUpper(character)
                        ? $"_{character}"
                        : character.ToString()))
                .ToUpper();
        }
    }
}
