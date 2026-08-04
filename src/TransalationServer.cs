namespace trosecnik.src
{
    static class TransalationServer
    {
        private static readonly Dictionary<string, string> CzLanguage = new()
        {
            { "lang", "Čestina" },
            { "gameName", "Trosečník" }
            ,
            { "deathMsg1", "Umřel jsi" },
            { "deathMsg2", "Mezerník pro ukončení hry" },

            { "hudBarThirst", "Žízeň" },
            { "hudBarHunger", "Hlad" },
            { "hudBarHealth", "Zdraví" },

            { "item-redBerries", "Červené bobule"},
                { "itemIntDesc-redBerries", "Sníst kliknutím" },

            { "item-woodenBridge", "Dřevěný most"},
                { "itemIntDesc-woodenBridge", "Postavit klinkutím na vodu" },

            { "item-stoneAxe", "Kamená sekera" },
                { "itemIntDesc-stoneAxe", "Těžit stromy kliknutím" },

            { "item-woodenLog", "Dřevo" },

            { "item-treeSapling", "Sazenice stromu" },
                { "itemIntDesc-treeSapling", "Zasadit kliknutím" },

            { "item-woodenWall", "Dřevěná zeď" },
                { "itemIntDesc-woodenWall", "Postavit klinkutím" },
        };
        private static readonly Dictionary<string, string> EngLanguage = new()
        {
            { "lang", "English" },
            { "gameName", "Survivor" },

            { "deathMsg1", "You Died" },
            { "deathMsg2", "Space to exit the game" },

            { "hudBarThirst", "Thirst" },
            { "hudBarHunger", "Hunger" },
            { "hudBarHealth", "Health" },

            { "item-redBerries", "Red Berries"},
                { "itemIntDesc-redBerries", "Click to eat" },

            { "item-woodenBridge", "Wooden Bridge"},
                { "itemIntDesc-woodenBridge", "Click on water to build" },

            { "item-stoneAxe", "Stone Axe" },
                { "itemIntDesc-stoneAxe", "Click to chop down trees" },

            { "item-woodenLog", "Wooden Log" },

            { "item-treeSapling", "Tree Sapling" },
                { "itemIntDesc-treeSapling", "Click to plant" },

            { "item-woodenWall", "Wooden Wall" },
                { "itemIntDesc-woodenWall", "Click to build" },
        };

        private static string Language = "cz";
        public static void SetLanguage(string langAlias)
        {
            Language = langAlias;
        }

        public static string GetLanguage()
        {
            return Language;
        }

        public static string GetTransalated(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;
            switch (Language?.ToLowerInvariant())
            {
                case "cz":
                    if (CzLanguage.TryGetValue(key, out var czVal)) return czVal;
                    break;
                case "eng":
                    if (EngLanguage.TryGetValue(key, out var engVal)) return engVal;
                    break;
            }
            return key;
        }
    }
}