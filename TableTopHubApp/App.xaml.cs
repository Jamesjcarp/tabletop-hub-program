using System.Configuration;
using System.Data;
using System.Windows;

namespace TableTopHubApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MusicScreen musicTab;
        public static BattleMapScreen battleTab;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Thread musicThread = new Thread(StartMusicWindow);
            musicThread.SetApartmentState(ApartmentState.STA);
            musicThread.Start();

            Thread battleThread = new Thread(StartBattleWindow);
            battleThread.SetApartmentState(ApartmentState.STA);
            battleThread.Start();
        }

        private void StartMusicWindow()
        {
            musicTab = new MusicScreen();
            musicTab.ShowDialog();
        }

        private void StartBattleWindow()
        {
            battleTab = new BattleMapScreen();
            battleTab.ShowDialog();
        }
    }

}
