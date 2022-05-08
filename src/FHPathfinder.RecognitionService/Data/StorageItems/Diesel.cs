namespace FHPathfinder.RecognitionService.Data.StorageItems;

public record Diesel : IStorageItem
{
    public StorageItemType Type => StorageItemType.Diesel;
    public string Name => "Diesel";
    public uint Count { get; }

    public Diesel(uint count)
    {
        Count = count;
    }
}