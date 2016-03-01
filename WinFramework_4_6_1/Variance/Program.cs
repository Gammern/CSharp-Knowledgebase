using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variance
{
    class Animal { }
    class Bear : Animal { }
    class Camel : Animal { }
    class ZooCleaner
    {
        public static void Wash(IPoppable<Animal> animals) {  }
    }

    interface IFoo<T> { }
    class FooString : IFoo<string> { }
    class FooObject : IFoo<object> { }

    /*
    Covariance:
    Assuming Bear is convertible to Animal, Stack<T> has a covariant type parameter T if Stack<Bear> is
    convertible to Stack<Animal>.
    Contravariance:
    Int the reverse direction
    */

    class Program
    {
        static void Main(string[] args)
        {
            // Stack covariance
            Animal animal = new Bear(); // Bear is convertible to Animal (asumption above)
            var bearStack = new Stack<Bear>();
            bearStack.Push(animal as Bear);
            bearStack.Push(new Bear());
            IPoppable<Animal> animalPop = bearStack;
            animal = animalPop.Pop();
            //Stack<Animal> animalst = bears;  // not a covariant parameter, classes do not permit covariant paramters since data flow in both directions
            ZooCleaner.Wash(bearStack); // OK, covariant method

            // contravariance
            var animalStack = new Stack<Animal>();
            IPushable<Bear> bearPush = animalStack;     // legal
            IPushable<Camel> camelPush = animalStack;   // legal, contravariant
            animalStack.Push(new Bear());
            animalStack.Push(new Camel());
            camelPush.Push(new Camel());
            bearPush.Push(new Bear());

            // Interface covariance
            string s = "Hello";
            object o = s;
            IFoo<string> fooString = new FooString();
            //IFoo<object> fooObject; fooObject = fooString; // HEY! interfaces should be covariant!!!! Whats up?

            // Array covariance
            Bear[] bearArr = new Bear[] { new Bear(), new Bear() };
            Animal[] animalArr = bearArr; // OK, covariant
            animalArr[1] = new Bear();
            //animalArr[1] = new Camel();   ArrayTypeMismatchException: Attempted to access an element as a type incompatible with the array.
            //animalArr[1] = new Animal();  ditto

        }
    }
}
