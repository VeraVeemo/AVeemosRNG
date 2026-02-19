using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SolsRNG
{

    public partial class MainWindow : Window
    {
        private Storyboard ShuffleAnim;
        private bool HistoryEnabled  = false;
        private bool AuraViewEnabled = false;
        private bool busy            = false;

        private void TogglePanel(object sender, RoutedEventArgs e)
        {
            if (sender.ToString().Contains("History"))
            {
                if (AuraViewEnabled) return;
                HistoryEnabled            = !HistoryEnabled;
                PanelView.Visibility      = !HistoryEnabled ? Visibility.Hidden : Visibility.Visible; // yes i added the not operator here just to make it look good
                RollButton.IsEnabled      = !HistoryEnabled;
                AuraViewButton.IsEnabled  = !HistoryEnabled;
                AuraViewButton.Visibility = !HistoryEnabled ? Visibility.Visible : Visibility.Hidden;
                PanelView.Items.Clear();
                if (!HistoryEnabled) return;
                foreach (var entry in Data.AuraHistory)
                {
                    Label newLabel = new()
                    {
                        Content = $"{entry} - 1 in {Data.Auras[entry]}",
                        Foreground = Data.Colors[entry]
                    };
                    PanelView.Items.Add(newLabel);
                }
            }
            else
            {
                if (HistoryEnabled) return;
                AuraViewEnabled          = !AuraViewEnabled;
                PanelView.Visibility     = !AuraViewEnabled ? Visibility.Hidden : Visibility.Visible;
                RollButton.IsEnabled     = !AuraViewEnabled;
                HistoryButton.IsEnabled  = !AuraViewEnabled;
                HistoryButton.Visibility = !AuraViewEnabled ? Visibility.Visible : Visibility.Hidden;
                PanelView.Items.Clear();
                if (!AuraViewEnabled) return;
                foreach (var entry in Data.Auras)
                {
                    Label newLabel = new()
                    {
                        Content = $"{entry.Key} - 1 in {entry.Value}",
                        Foreground = Data.Colors[entry.Key]
                    };
                    PanelView.Items.Add(newLabel);
                }
            }
        }

        private void RollAura(object sender, RoutedEventArgs e)
        {
            if (HistoryEnabled || AuraViewEnabled || busy) return;
            RollButton.IsEnabled = false;
            var aura             = Data.GetRandomAura(false);
            bool trolled         = aura.Value is 1024 or 512 or 256 or 128;
            AuraText.Content     = aura.Key;
            AuraText.Foreground  = Data.Colors[aura.Key];
            Chance.Content       = $"1 in {aura.Value}";
            Chance.Foreground    = Data.Colors[aura.Key];
            AuraText.BeginStoryboard(ShuffleAnim);
            if (trolled)
            {
                var fakeOut = Data.GetRandomAura(true);
                busy        = true;
                Task.Delay(2250).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        AuraText.Content     = fakeOut.Key;
                        AuraText.Foreground  = Data.Colors[fakeOut.Key];
                        Chance.Content       = $"1 in {fakeOut.Value}";
                        Chance.Foreground    = Data.Colors[fakeOut.Key];
                        RollButton.IsEnabled = true;
                        busy                 = false;
                    });
                });
            }
            else
            {
                RollButton.IsEnabled = true;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            ShuffleAnim = (Storyboard)rollGrid.Resources["ShuffleAnimation"];
            KeyDown += ((sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.H:
                        TogglePanel("HistoryView", args);
                        break;
                    case Key.A:
                        TogglePanel("AuraView", args);
                        break;
                    case Key.R:
                        RollAura(sender, args);
                        break;
                }
            });
        }
    }
}