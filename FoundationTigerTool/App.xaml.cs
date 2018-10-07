using System;
using System.Windows;
using FoundationTigerTool.Views;

namespace FoundationTigerTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                OneTimeRunWindow oneTimeRunWindow = new OneTimeRunWindow();
                MainWindow = oneTimeRunWindow;
                MainWindow.Show();
            }
            else
            {
                Shutdown(1);
            }
        }
    }
}