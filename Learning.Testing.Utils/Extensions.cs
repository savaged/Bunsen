using Newtonsoft.Json;
using System.Collections.Generic;

namespace Learning.Testing.Utils
{
    public static class Extensions
    {
        public static string ToJson(this IDictionary<string, object> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }
    }
}
