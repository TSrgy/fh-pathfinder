namespace FHPathfinder.RecognitionService.Data.StorageItems;

public record CrudeOil : IStorageItem
{
    public StorageItemType Type => StorageItemType.CrudeOil;
    public string Name => "Crude Oil";
    public uint Count { get; }

    public CrudeOil(uint count)
    {
        Count = count;
    }
}