namespace FHPathfinder.RecognitionService.Data;

public interface IStorageItem
{
    StorageItemType Type { get; }

    string Name { get; }

    uint Count { get; }
}