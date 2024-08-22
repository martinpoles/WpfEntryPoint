using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace Wpf_EntryPoint
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Abilita il tracing degli errori di binding
            PresentationTraceSources.SetTraceLevel(this.MainWindow, (PresentationTraceLevel)TraceLevel.Info);
        }
    }

}
