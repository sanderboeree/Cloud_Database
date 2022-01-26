using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace OnlineStore.Api.Infrastructure.ModelBinding
{
    public class OnlineStoreEnumModelBinder : IModelBinder
    {
        private readonly TypeConverter _typeConverter;

        public OnlineStoreEnumModelBinder(Type type, ILoggerFactory loggerFactory)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _typeConverter = TypeDescriptor.GetConverter(type);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                var value = valueProviderResult.FirstValue?.Replace("_", "");

                var model = !string.IsNullOrWhiteSpace(value)
                    ? _typeConverter.ConvertFrom(null, valueProviderResult.Culture, value)
                    : null;

                ValidateModel(bindingContext, valueProviderResult, model);

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                var isFormatException = exception is FormatException;

                if (!isFormatException && exception.InnerException != null)
                {
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }

                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, exception, bindingContext.ModelMetadata);

                return Task.CompletedTask;
            }
        }

        private static void ValidateModel(ModelBindingContext bindingContext, ValueProviderResult valueProviderResult, object model)
        {
            if (model == null)
            {
                if (!bindingContext.ModelMetadata.IsReferenceOrNullableType)
                {
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(valueProviderResult.ToString())
                    );
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
            else if (EnumIsDefined(model, bindingContext))
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueIsInvalidAccessor(valueProviderResult.ToString())
                );
            }
        }

        private static bool EnumIsDefined(object model, ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;

            if (bindingContext.ModelMetadata.IsFlagsEnum)
            {
                var underlyingModel = Convert.ChangeType(model, Enum.GetUnderlyingType(modelType), CultureInfo.InvariantCulture).ToString();
                var convertedModel = model.ToString();

                return !string.Equals(underlyingModel, convertedModel, StringComparison.OrdinalIgnoreCase);
            }

            return Enum.IsDefined(modelType, model);
        }
    }
}
