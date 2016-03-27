using System;

namespace ToStringReflecttion
{
    public interface ICustomer
    {
        int Age { get; set; }
        Guid Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}