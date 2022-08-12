using Learning.Testing.API;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Learning.Testing.Models
{
    public abstract class ModelBase : IModel
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public IDictionary<string, object> ToDictionary()
        {
            var j = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(j) ??
                new Dictionary<string, object>();
        }

    }
}
