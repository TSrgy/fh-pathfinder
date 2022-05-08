namespace FHPathfinder.RecognitionService.Data;

public class Storage : HashSet<IStorageItem>
{
    public Storage()
        : base(new StorageItemsComparer())
    {
        
    }

    private class StorageItemsComparer : IEqualityComparer<IStorageItem>
    {
        public bool Equals(IStorageItem x, IStorageItem y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Type == y.Type;
        }

        public int GetHashCode(IStorageItem obj)
        {
            return HashCode.Combine(obj.Name);
        }
    }
}