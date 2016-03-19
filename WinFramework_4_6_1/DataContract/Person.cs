using System.Runtime.Serialization;

namespace DataContract
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name;

        [DataMember]
        public int Age;

        public override string ToString() => $"Name: {Name},  Age: {Age}";
    }
}
