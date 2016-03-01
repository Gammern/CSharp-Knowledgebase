namespace Variance
{
    public interface IPoppable<out T> { T Pop(); }  // "out T" covariant T
    public interface IPushable<in T> { void Push(T obj); } // "in T" contravariant T
    class Stack<T> : IPoppable<T>, IPushable<T>
    {
        int position;
        T[] data = new T[100];
        public void Push(T obj) => data[position++] = obj;
        public T Pop() => data[--position];
    }
}
