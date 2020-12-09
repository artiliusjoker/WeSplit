using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
        public DetailTripWindow(int tripID)
        {
            InitializeComponent();
            DataContext = new ViewModel.DetailTripViewModel(tripID);
        }

        public int CarouselItemCount { get; set; } = 0;
        private int _currentElement = 0;

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ListBox && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void MemberDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DataGrid && !e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = sender
                };
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void AnimateCarousel()
        {
            var carousel = Helpers.VisualTreeViewHelper.FindChild<StackPanel>(ImageCarousel, "Carousel");
            Storyboard storyboard = (this.Resources["CarouselStoryboard"] as Storyboard);
            DoubleAnimation animation = storyboard.Children.First() as DoubleAnimation;
            Storyboard.SetTarget(animation, carousel);
            animation.To = -550 * _currentElement;
            storyboard.Begin();
        }

        private void BtnLeftClick(object sender, RoutedEventArgs e)
        {
            if (_currentElement > 0)
            {
                _currentElement--;
                AnimateCarousel();
            }
        }

        private void BtnRightClick(object sender, RoutedEventArgs e)
        {
            if (_currentElement < CarouselItemCount - 1)
            {
                _currentElement++;
                AnimateCarousel();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CarouselItemCount = int.Parse(CarouselCount.Text);
            if (CarouselItemCount < 1)
            {
                ImagesDetail.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImagesDetail.Visibility = Visibility.Visible;
            }
        }
    }
}
