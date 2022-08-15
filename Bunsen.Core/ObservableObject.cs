using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bunsen.Core
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// The CallerMemberName attribute that is applied to the optional
        /// propertyName parameter causes the property name of the caller
        /// to be substituted as an argument.
        /// <param name="propertyName"></param>
        /// </summary>
        protected void NotifyPropertyChanged(
            [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
