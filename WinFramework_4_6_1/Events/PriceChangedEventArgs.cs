namespace Events
{
    public class PriceChangedEventArgs : System.EventArgs
    {
        public readonly decimal LastPrice;
        public readonly decimal NewPrice;

        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
        {
            this.LastPrice = lastPrice;
            this.NewPrice = newPrice;
        }
    }
}
