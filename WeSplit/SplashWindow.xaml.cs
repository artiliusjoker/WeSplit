using System.Configuration;
using System.Windows;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
                config.Save(ConfigurationSaveMode.Minimal);
            }
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            var showSplash = bool.Parse(value);

            if (showSplash == false)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                return;
            }
            Models.Location randomized = Models.DataAccess.GetRandomLocationForSplash();
            tbxName.Text = randomized.Name;
            tbxDescription.Text = randomized.Description;
            this.Show();
        }
    }

}
