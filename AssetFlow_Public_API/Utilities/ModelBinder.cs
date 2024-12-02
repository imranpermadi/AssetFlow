using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAMS_Public_API.Utilities
{

    public class MyDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if(reader.Value != null)
            {
                return reader.Value;
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value);

        }
    }

    public class CustomDateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (CustomDateTimeModelBinder.SUPPORTED_DATETIME_TYPES.Contains(context.Metadata.ModelType))
            {
                return new BinderTypeModelBinder(typeof(CustomDateTimeModelBinder));
            }
            return null;
        }
    }

    public class CustomDateTimeModelBinder : IModelBinder
    {
        public static List<Type> SUPPORTED_DATETIME_TYPES = new List<Type>() { typeof(DateTime), typeof(DateTime?) };

        public Task BindModelAsync (ModelBindingContext modelBindingContext)
        {
           
            if (modelBindingContext == null)
            {
                throw new ArgumentNullException (nameof(modelBindingContext));
            }
            if (!SUPPORTED_DATETIME_TYPES.Contains(modelBindingContext.ModelType))
            {
                return Task.CompletedTask;
            }
            var modelName = modelBindingContext.ModelName;
            var valueProviderResult = modelBindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            modelBindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var dateTimeToParse = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(dateTimeToParse))
            {
                return Task.CompletedTask;
            }

            var formattedDateTime = ParseDateTime(dateTimeToParse);
            modelBindingContext.Result = ModelBindingResult.Success(formattedDateTime);
            return Task.CompletedTask;
        }
        static DateTime? ParseDateTime(string date)
        {
            var CUSTOM_DATETIME_FORMATS = new string[]
            {
            "ddMMyyyy",
            "dd-MM-yyyy-THH-mm-ss",
            "dd-MM-yyyy-HH-mm-ss",
            "dd-MM-yyyy-HH-mm",
            };
            foreach (var format in CUSTOM_DATETIME_FORMATS)
            {
                if (DateTime.TryParseExact(
                    date, format, null,
                    DateTimeStyles.None,
                    out DateTime validDate)
                   )
                {
                    return validDate;
                }
            }
            return null;
        }
    }
}
