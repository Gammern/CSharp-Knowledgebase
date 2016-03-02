using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    // Don't need this one as EventHandler<> is more appropriate
    //public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);

    public class Stock
    {
        string symbol;
        decimal price;

        public Stock(string symbol) { this.symbol = symbol; }

        public event EventHandler<PriceChangedEventArgs> PriceChanged;  // event => compiler expands to add/remove for +=/-=

        protected virtual void OnPriceChanged(PriceChangedEventArgs e) => PriceChanged?.Invoke(this, e);
        //{
        //    if (PriceChanged != null) PriceChanged(this, e);
        //}

        public decimal Price
        {
            get { return price; }
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, price));
            }
        }
    }
}
