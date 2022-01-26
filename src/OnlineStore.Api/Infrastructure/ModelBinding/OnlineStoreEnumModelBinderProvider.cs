using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace OnlineStore.Api.Infrastructure.ModelBinding
{
    public class OnlineStoreEnumModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsEnum)
            {
                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

                return new OnlineStoreEnumModelBinder(context.Metadata.UnderlyingOrModelType, loggerFactory);
            }

            return null;
        }
    }
}
