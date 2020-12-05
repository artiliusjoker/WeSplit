using System.Windows;
using System.Windows.Input;

namespace WeSplit.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand CloseAppCommand { get; set; }

        public MainViewModel()
        {          
            CloseAppCommand = new RelayCommand<Window>((p) => { return true; }, (p) => 
            {
                if (p != null)
                {
                    p.Close();
                }
            });
        }
    }
}
