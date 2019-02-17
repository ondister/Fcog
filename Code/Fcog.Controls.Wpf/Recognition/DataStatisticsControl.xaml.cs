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
    /// Логика взаимодействия для DataStatisticsControl.xaml
    /// </summary>
    public partial class DataStatisticsControl 
    {


        public DataSets DataSets
        {
            get => (DataSets)GetValue(DataSetsProperty);
            set => SetValue(DataSetsProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSetsProperty =
            DependencyProperty.Register("DataSets", typeof(int), typeof(DataStatisticsControl), new PropertyMetadata(null));


        public DataStatisticsControl()
        {
            InitializeComponent();
        }
    }
}
