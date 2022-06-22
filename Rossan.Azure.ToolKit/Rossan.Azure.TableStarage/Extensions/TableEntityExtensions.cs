using Azure.Data.Tables;
using Newtonsoft.Json;

namespace Rossan.Azure.TableStarage.Extensions
{
    public static class TableEntityExtensions
    {
        public static Dictionary<string, TValue> ToDictionary<T, TValue>(this T obj)
             where T : ITableEntity
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }

        public static bool IsNull<T>(this T obj)
             where T : ITableEntity
        {
            return ReferenceEquals((object)obj, default(T));
        }
    }
}
