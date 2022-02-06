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

namespace OrderTrackingSystem.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for MindMapControl.xaml
    /// </summary>
    public partial class MindMapControl : UserControl
    {
        public MindMapControl()
        {
            InitializeComponent();
        }

        private void expandBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if(btn != null)
            {
                btn.Content = btn.Content.Equals("+") ? "-" : "+";
            }
        }
    }
}
