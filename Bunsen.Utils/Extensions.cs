using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bunsen.Utils
{
    public static class Extensions
    {
        public static string ToJson(this IDictionary<string, object> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        public static T? Clone<T>(this T t) where T : class
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(t));
        }
        
    }
}
