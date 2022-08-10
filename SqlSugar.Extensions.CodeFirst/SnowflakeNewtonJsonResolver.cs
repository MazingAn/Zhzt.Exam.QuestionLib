using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SqlSugar.Extensions.CodeFirst
{
    /// <summary>
    /// 重写json的序列化和反序列化方法  针对long类型做单独处理
    /// 【类似于自定义一个convert】 因为默认的convert在传出long的时候会精度损失
    /// </summary>
    public class SnowflakeNewtonJsonResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonConverter? ResolveContractConverter(Type objectType)
        {
            if (objectType == typeof(long))
            {
                return new JsonConverterLong();
            }
            return base.ResolveContractConverter(objectType);
        }
    }

    internal class JsonConverterLong : JsonConverter
    {
        /// <summary>
        /// 能不能转
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// 从json数据转换为long类型的数据
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if ((reader.ValueType == null || reader.ValueType == typeof(long?)) && reader.Value == null)
            {
                return null;
            }
            else
            {
                long.TryParse(reader.Value != null ? reader.Value.ToString() : "", out long value);
                return value;
            }
        }

        /// <summary>
        /// 把long类型的数据转换为json数据（按照字符串处理，不做花里胡哨的操作，免得损失精度）
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue(value);
            else
                writer.WriteValue(value + "");
        }
    }
}
