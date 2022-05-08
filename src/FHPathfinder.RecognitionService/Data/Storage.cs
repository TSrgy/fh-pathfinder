namespace FHPathfinder.RecognitionService.Data;

public class Storage : Dictionary<StorageItemType, uint>
{
    public Storage(IDictionary<StorageItemType, uint> dictionary)
        : base(dictionary)
    {
        
    }
}