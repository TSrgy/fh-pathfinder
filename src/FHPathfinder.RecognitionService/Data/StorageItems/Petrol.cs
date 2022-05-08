namespace FHPathfinder.RecognitionService.Data.StorageItems;

public record Petrol : IStorageItem
{
    public StorageItemType Type => StorageItemType.Petrol;
    public string Name => "Petrol";
    public uint Count { get; }

    public Petrol(uint count)
    {
        Count = count;
    }
}