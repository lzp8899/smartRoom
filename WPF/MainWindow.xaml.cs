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

namespace Web
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

      
        private void btnOpenZNKG_Click(object sender, RoutedEventArgs e)
        {
            MainApp.Instance.deviceServer.ForceOpenZNKG(true);
        }

        private void btnCloseZNKG_Click(object sender, RoutedEventArgs e)
        {
            MainApp.Instance.deviceServer.ForceOpenZNKG(false);
        }

        private void btnOpenPXJ_Click(object sender, RoutedEventArgs e)
        {
            MainApp.Instance.deviceServer.ForceOpenPXJByManual();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NLog.LogManager.GetLogger("default").Info("开始停止服务");
            MainApp.Instance.Stop();
            MainApp.Instance.SaveJson();
        }

        private void btnLoadJson_Click(object sender, RoutedEventArgs e)
        {
            MainApp.Instance.LoadJson();
        }

        private void btnSaveJson_Click(object sender, RoutedEventArgs e)
        {
            MainApp.Instance.SaveJson();
        }
    }
}
