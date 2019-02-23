using System.Windows;
using System.Windows.Controls;
using Fcog.Core.Recognition;

namespace Fcog.Controls.Wpf.Recognition
{
    public class DataSetsViewer : Control
    {
        // Using a DependencyProperty as the backing store for DataSet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSetsProperty =
            DependencyProperty.Register("DataSets", typeof(DataSets), typeof(DataSetsViewer),
                new PropertyMetadata(null));


        static DataSetsViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataSetsViewer),
                new FrameworkPropertyMetadata(typeof(DataSetsViewer)));
        }

        public DataSets DataSets
        {
            get => (DataSets) GetValue(DataSetsProperty);
            set => SetValue(DataSetsProperty, value);
        }


       
    }
}