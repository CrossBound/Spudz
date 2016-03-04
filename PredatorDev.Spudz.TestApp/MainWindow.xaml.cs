using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PredatorDev.Spudz;

namespace PredatorDev.Spudz.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private StringLog _logger;
        private FogBugzManager fogbugz;

        public MainWindow()
        {
            InitializeComponent();
            // REFERENCE CREDIT: mijacobs @ https://msdn.microsoft.com/en-us/windows/uwp/controls-and-patterns/resourcedictionary-and-xaml-resource-references
            _logger = (StringLog)this.Resources["LogData"];
            _logger.AppendLine("initialized");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string token = TokenTextBox.Text;
                this.fogbugz = new FogBugzManager("https://predatordev.fogbugz.com/api.asp", token);
                _logger.AppendLine("Signed on");
            }
            catch (Exception error)
            {
                _logger.AppendLine(error.Message);
            }
        }

        private void TicketsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tickets = this.fogbugz.Tickets;
                _logger.AppendLine(tickets);
            }
            catch (Exception error)
            {
                _logger.AppendLine(error.Message);
            }
        }
    }
}
