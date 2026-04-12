namespace Interfaces
{
    public interface ICollectable<T>
    {
        public void BeingCollected(T item);
    }
}