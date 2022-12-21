namespace Voodoo.Pattern
{
    public interface IPoolableCycle<T>
    {
        T Create();
        void Use(T item);
        void Free(T item);
        void Dispose(T item);
    }
}