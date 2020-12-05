using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeSplit.View.UserControlView
{
    /// <summary>
    /// Interaction logic for AddTripControl.xaml
    /// </summary>
    public partial class AddTripControl : UserControl
    {
        public AddTripControl()
        {
            InitializeComponent();
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentObject = sender as DatePicker;
            MessageBox.Show(currentObject.SelectedDate.ToString());
        }
    }
}
