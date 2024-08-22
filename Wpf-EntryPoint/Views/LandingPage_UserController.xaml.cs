using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.ViewModels;

namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for LandingPage_UserController.xaml
    /// </summary>
    public partial class LandingPage_UserController : UserControl
    {
        public LandingPage_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;
        }
    }
}
