using System.Windows;

using WeSplit.Models;

namespace WeSplit.View
{
    /// <summary>
    /// Interaction logic for DetailTripWindow.xaml
    /// </summary>
    public partial class DetailTripWindow : Window
    {
        public DetailTripWindow(Trip selectedTrip)
        {
            InitializeComponent();
            DataContext = new ViewModel.DetailTripViewModel(selectedTrip);
        }
    }
}
