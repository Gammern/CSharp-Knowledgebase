namespace ToStringReflecttion
{
    public sealed class CustomerReflection : Customer
    {
        public override string ToString()
        {
            return this.ToStringReflection();
        }
    }
}
