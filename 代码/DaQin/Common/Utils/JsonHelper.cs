using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Utils
{
    public class JsonHelper
    {
        /// <summary>
        /// json字符串转换成集合
        /// </summary>
        public static List<T> ConvertToList<T>(string jsonStr) where T : new()
        {
            List<T> list = new List<T>();
            JObject jObject = JObject.Parse("{\"data\":" + jsonStr + "}");
            foreach (JToken jToken in jObject["data"])
            {
                T item = new T();
                PropertyInfo[] propertyInfoList = typeof(T).GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfoList)
                {
                    JToken val = jToken[propertyInfo.Name];
                    if (val != null && !string.IsNullOrWhiteSpace(val.Value<string>()))
                    {
                        propertyInfo.SetValue(item, val.Value<string>(), null);
                    }
                }
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// json字符串转换成集合
        /// </summary>
        public static T ConvertToModel<T>(string jsonStr) where T : new()
        {
            List<T> list = ConvertToList<T>(jsonStr);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return default(T);
        }
    }
}
