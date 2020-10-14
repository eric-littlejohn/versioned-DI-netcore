namespace VersionedDI
{
    public interface IServiceCache
    {
        object this[object key] { get; }

        object Get(object key);

        object Add(object key, object implmentationInstance);
    }
}
