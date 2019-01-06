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
using System.Windows.Shapes;

namespace Fcog.Demo.Wpf.CreateQuestionnaire
{
    /// <summary>
    /// Логика взаимодействия для QPropWindow.xaml
    /// </summary>
    public partial class QPropWindow 
    {
        public QPropWindow()
        {
            InitializeComponent();
        }

        private void QProperties_OnClickNext(object sender, EventArgs e)
        {
            var properties = QProperties.Properties;

            var formCreatorWindow = new FormCreatorWindow(properties) {Owner = this.Owner};

            Close();

            formCreatorWindow.ShowDialog();
        }
    }
}
