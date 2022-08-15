using Bunsen.API;
using Bunsen.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bunsen.Models
{
    public abstract class ModelBase : ObservableObject, IModel
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public IDictionary<string, object> ToDictionary()
        {
            var j = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(j) ??
                new Dictionary<string, object>();
        }

    }
}
