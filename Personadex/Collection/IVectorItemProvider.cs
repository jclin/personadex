namespace Personadex.Collection
{
    internal interface IVectorItemProvider<out T> where T : class
    {
        long GetCount();
        T CreateItem(int vectorIndex);
    }
}
