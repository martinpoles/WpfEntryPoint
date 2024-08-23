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

namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for PuliziaNumeri_39_UserController.xaml
    /// </summary>
    public partial class PuliziaNumeri_39_UserController : UserControl
    {
        public PuliziaNumeri_39_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;
        }
    }
}
