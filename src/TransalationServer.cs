namespace trosecnik.src
{
    static class TransalationServer
    {
        private static Dictionary<string, string> CzLanguage = new Dictionary<string, string>
        {
            { "lang", "Čestina" },
            { "gameName", "Trosečník" },
            { "deathMsg1", "Umřel jsi" },
            { "deathMsg2", "Mezerník pro ukončení hry" },
            { "hudBarThirst", "Žízeň" },
            { "hudBarHunger", "Hlad" },
            { "hudBarHealth", "Zdraví" },
            { "item-redBerries", "Červené bobule"},
                { "itemIntDesc-redBerries", "Sníst kliknutím" },

            { "item-woodenBridge", "Dřevěný most"},
                { "itemIntDesc-woodenBridge", "Postavit klinkutím na vodu (vzdálenost 1)" },

            { "item-stoneAxe", "Kamená sekera" },
                { "itemIntDesc-stoneAxe", "Těžit stromy kliknutím (vzdálenost 1)" },

            { "item-woodenLog", "Dřevo" },
        };
        private static Dictionary<string, string> EngLanguage = new Dictionary<string, string>
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
                { "itemIntDesc-woodenBridge", "Click on water to build (range 1)" },

            { "item-stoneAxe", "Stone Axe" },
                { "itemIntDesc-stoneAxe", "Click to chop down trees (range 1)" },

            { "item-woodenLog", "Wooden Log" },
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