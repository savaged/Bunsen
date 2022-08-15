using Bunsen.API;
using System;

namespace Bunsen.ViewModels.Core
{
    public class AddingModelEventArgs : EventArgs
    {
        public AddingModelEventArgs(IModel model)
        {
            Model = model ??
                throw new ArgumentNullException(nameof(model));
        }

        public IModel Model { get; }
    }
}
