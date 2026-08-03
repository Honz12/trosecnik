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
        };
        private static Dictionary<string, string> EngLanguage = new Dictionary<string, string>
        {
            { "lang", "English" },
            { "gameName", "Survivor" },
            { "deathMsg1", "You Died" },
            { "deathMsg2", "Space to exit the game" },
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