using System.Collections.Generic;
using Joive.BurglinGnomes.SwimmingRing.Items;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Joive.BurglinGnomes.SwimmingRing.Game
{
    internal static class GameText
    {
        private static readonly Dictionary<string, TextPair> TextByLanguage = new Dictionary<string, TextPair>
        {
            { "en", new TextPair("Inflatable Swimming Ring", "Keeps you steady in water.") },
            { "ru", new TextPair("Надувной круг", "Помогает держаться на воде.") },
            { "uk", new TextPair("Надувний круг", "Допомагає триматися на воді.") },
            { "de", new TextPair("Aufblasbarer Schwimmring", "Hilft dir, im Wasser stabil zu bleiben.") },
            { "fr", new TextPair("Bouée gonflable", "Vous aide à rester stable dans l'eau.") },
            { "es", new TextPair("Flotador inflable", "Te ayuda a mantenerte estable en el agua.") },
            { "pt", new TextPair("Boia inflável", "Ajuda você a se manter estável na água.") },
            { "it", new TextPair("Ciambella gonfiabile", "Ti aiuta a restare stabile in acqua.") },
            { "pl", new TextPair("Nadmuchiwane koło", "Pomaga utrzymać się stabilnie w wodzie.") },
            { "tr", new TextPair("Şişme yüzme simidi", "Suda dengede kalmana yardımcı olur.") },
            { "zh", new TextPair("充气游泳圈", "帮助你在水中保持稳定。") },
            { "ja", new TextPair("空気入り浮き輪", "水中で体勢を保ちやすくする。") },
            { "ko", new TextPair("공기 주입식 튜브", "물속에서 안정적으로 움직일 수 있게 해 줍니다.") }
        };

        internal static void RegisterSwimmingRingText()
        {
            IReadOnlyList<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
            if (locales == null || locales.Count == 0)
            {
                AddText("Items", SwimmingRingDefinition.ItemName, SwimmingRingDefinition.DisplayName);
                AddText("Items Descriptions", SwimmingRingDefinition.ItemName, SwimmingRingDefinition.Description);
                return;
            }

            foreach (Locale locale in locales)
            {
                TextPair text = GetText(locale);
                AddText("Items", SwimmingRingDefinition.ItemName, text.Name, locale);
                AddText("Items Descriptions", SwimmingRingDefinition.ItemName, text.Description, locale);
            }
        }

        private static void AddText(string tableName, string key, string value)
        {
            AddText(tableName, key, value, null);
        }

        private static void AddText(string tableName, string key, string value, Locale locale)
        {
            StringTable table = LocalizationSettings.StringDatabase.GetTable(tableName);
            if (locale != null)
            {
                table = LocalizationSettings.StringDatabase.GetTable(tableName, locale);
            }

            if (table == null || table.GetEntry(key) != null)
            {
                return;
            }

            table.AddEntry(key, value);
        }

        private static TextPair GetText(Locale locale)
        {
            string code = locale.Identifier.Code.ToLowerInvariant();
            if (TextByLanguage.TryGetValue(code, out TextPair exact))
            {
                return exact;
            }

            int separatorIndex = code.IndexOf('-');
            string language = separatorIndex >= 0 ? code.Substring(0, separatorIndex) : code;
            if (TextByLanguage.TryGetValue(language, out TextPair languageText))
            {
                return languageText;
            }

            return TextByLanguage["en"];
        }

        private readonly struct TextPair
        {
            internal readonly string Name;
            internal readonly string Description;

            internal TextPair(string name, string description)
            {
                Name = name;
                Description = description;
            }
        }
    }
}
