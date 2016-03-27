using System;

namespace ToStringReflecttion
{
    public abstract class Customer : ICustomer
    {
        protected Customer()
        : base()
        {
            this.Id = Guid.NewGuid();
        }
        public int Age { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
