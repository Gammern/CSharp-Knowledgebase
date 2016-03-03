using System;

/// <summary>
/// Events are a language pattern that formalizes broadcaster/subscriber pattern using delegates.
/// event keyword make routine more robust by only allowing +=/-= and not =
/// </summary>
/// 
namespace Events
{
    // ols style, new use event with EventHandler<>
    public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);

    public class Broadcaster
    {
#pragma warning disable 67        
        public event PriceChangedHandler PriceChanged; // old style
#pragma warning restore 67        

        // compiler translates this to something like
        private PriceChangedHandler _priceChanged;
        public event PriceChangedHandler _PriceChanged
        {
            add { _priceChanged += value; }
            remove { _priceChanged -= value; }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Stock stock = new Stock("THPW") { Price = 27.10M };
            stock.PriceChanged += stock_PriceChanged;
            stock.Price = 31.59M;
        }

        private static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if((e.NewPrice - e.LastPrice)/e.LastPrice > 0.1M)
                Console.WriteLine("Alert, 10% stock price increase! sell sell");
        }
    }
}
