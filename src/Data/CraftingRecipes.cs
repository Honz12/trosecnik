namespace trosecnik.src.Data
{

    public static class CraftingRecipes
    {
        public readonly struct ItemEntry(string itemId, int count)
        {
            public readonly string ItemId = itemId;
            public readonly int Count = count;
        }

        public class Recipe(ItemEntry[] ingredients, ItemEntry result)
        {
            public ItemEntry[] Ingredients = ingredients;
            public ItemEntry Result = result;
        }

        public static Recipe[] GetRecipes()
        {
            return recipes;
        }

        private static readonly Recipe[] recipes = [
            new(
                [new("woodenLog", 2)],
                new("woodenBridge", 2)
            ),
            new(
                [new("woodenLog", 4)],
                new("woodenWall", 2)
            ),
            new(
                [new("woodenLog", 8)],
                new("hammer", 1)
            ),
        ];
    }
}
