using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.ViewModels;

namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for SmsMassivi_Rni_UserController.xaml
    /// </summary>
    public partial class SmsMassivi_Rni_UserController : UserControl
    {
        public SmsMassivi_Rni_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;
        }
    }
}
