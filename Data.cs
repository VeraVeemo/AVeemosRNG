using System.Windows.Media;

namespace SolsRNG
{
    public class Data
    {
        
        public static KeyValuePair<string, int> CurrentAura = new("None", 1);
        public static string[] AuraHistory = [];

        public static readonly Dictionary<string, int> Auras = new()
        {
            {"Test1", 2},
            {"Test2", 4},
            {"Test3", 8},
            {"Test4", 16},
            {"Test5", 32},
            {"Test6", 64},
            {"Test7", 128},
            {"Test8", 256},
            {"Test9", 512},
            {"Test10", 1024}
        };

        public static readonly Dictionary<string, SolidColorBrush> Colors = new()
        {
            {"Test1", Brushes.Red},
            {"Test2", Brushes.Orange},
            {"Test3", Brushes.Yellow},
            {"Test4", Brushes.LawnGreen},
            {"Test5", Brushes.CornflowerBlue},
            {"Test6", Brushes.Indigo},
            {"Test7", Brushes.BlueViolet},
            {"Test8", Brushes.Black},
            {"Test9", Brushes.Gold},
            {"Test10", Brushes.Silver}
        };
        
        public static KeyValuePair<string, int> GetRandomAura(bool notRare)
        {
            KeyValuePair<string, int> chosenAura = new("None", 1);
            while (chosenAura.Key == "None")
            {
                foreach (var aura in Auras)
                {
                    if (new Random().Next(0, aura.Value) != 0) continue;
                    if (aura.Value is 1024 or 512 or 256 or 128 && notRare) break;
                    chosenAura = aura;
                    break;
                }
            }
            AuraHistory = [chosenAura.Key, .. AuraHistory];
            return chosenAura;
        }
    }
}
