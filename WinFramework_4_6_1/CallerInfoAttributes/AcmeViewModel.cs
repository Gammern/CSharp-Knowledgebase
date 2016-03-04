using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CallerInfoAttributes
{
    public class AcmeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };    // dummy delegate, no need for null check

        void RaisePropertyChanged([CallerMemberName] string propertyName = null) 
            => PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        private string customerName;

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                if (value == customerName) return;
                customerName = value;
                RaisePropertyChanged();
                // compiler converts the avove line to
                // RaisePropertyChanged("CustomerName");
            }
        }
    }
}
