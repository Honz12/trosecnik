using trosecnik.src.InventorySpace;
using trosecnik.src.InventorySpace.Items;

namespace trosecnik.src.Data
{
    public static class ItemData
    {
        public readonly struct ItemEntryData
        {
            public string TexturePath { get; }
            public Type ItemType { get; }

            public ItemEntryData(string texturePath, Type itemType)
            {
                if (!typeof(IItem).IsAssignableFrom(itemType))
                {
                    throw new ArgumentException($"Type {itemType.Name} must implement IItem.", nameof(itemType));
                }

                TexturePath = texturePath;
                ItemType = itemType;
            }

            public IItem CreateInstance()
            {
                return (IItem)Activator.CreateInstance(ItemType)!;
            }

            public IItem CreateInstance(object[] args)
            {
                return (IItem)Activator.CreateInstance(ItemType, args)!;
            }
        }

        private static readonly Dictionary<string, ItemEntryData> data = new()
        {
            { "redBerries", new ItemEntryData("items/item_0003.png", typeof(RedBerriesItem)) },
            { "stoneAxe", new ItemEntryData("items/item_0004.png", typeof(StoneAxeItem)) },
            { "woodenBridge", new ItemEntryData("items/item_0005.png", typeof(WoodenBridgeItem)) },
            { "woodenLog", new ItemEntryData("items/item_0006.png", typeof(WoodenLogItem)) },
            { "treeSapling", new ItemEntryData("items/item_0007.png", typeof(TreeSaplingItem)) },
        };
        
        public static string GetTexture(string id)
        {
            return data[id].TexturePath;
        }
        
        public static Type GetClassType(string id)
        {
            return data[id].ItemType;
        }
        
        public static ItemEntryData GetItemEntryData(string id)
        {
            return data[id];
        }
    }
}