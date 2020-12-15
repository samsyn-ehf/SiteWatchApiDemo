using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Data;

namespace SiteWatchApiDemo
{
    public class DecompressGeo : IValueConverter
    {
        class PolymorphicWriteOnlyJsonConverter<T> : JsonConverter<T>
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException($"Deserializing not supported. Type={typeToConvert}.");
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string valueStr)
            {
                return string.Empty;
            }
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PolymorphicWriteOnlyJsonConverter<SiteWatchWebApiModel.Search.Geo>());
            var data = SiteWatchWebApiModel.Search.GeoDecompress.Decompress(valueStr);            
            return JsonSerializer.Serialize(data, options);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
