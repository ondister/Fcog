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
using Fcog.Core.Recognition;

namespace Fcog.Controls.Wpf.Recognition
{
 
    /// <summary>
    /// Логика взаимодействия для DataSetControl.xaml
    /// </summary>
    public partial class DataSetControl 
    {

        
        public DataSet DataSet
        {
            get => (DataSet)GetValue(DataSetProperty);
            set => SetValue(DataSetProperty, value);
        }

        // Using a DependencyProperty as the backing store for DataSet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSetProperty =
            DependencyProperty.Register("DataSet", typeof(DataSet), typeof(DataSetControl), new PropertyMetadata(null));




        public DataSetControl()
        {
            InitializeComponent();
        }

       
    }
}
