using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Herbs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModel();

            this.Closed += (_, __) => (this.DataContext as ViewModel).Dispose(); 
        }

        private void OnTopChecked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel).OnTopChecked.Execute();
        }

        private void OnNormalChecked(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel).OnNormalChecked.Execute();
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ViewModel).OnGotFocus.Execute();
        }

        private void OnActivated(object sender, EventArgs e)
        {
            (this.DataContext as ViewModel).OnGotFocus.Execute();
        }
    }
}
