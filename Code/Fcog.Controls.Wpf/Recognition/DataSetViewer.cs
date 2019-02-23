using System.Windows;
using System.Windows.Controls;
using Fcog.Core.Recognition;

namespace Fcog.Controls.Wpf.Recognition
{
    public class DataSetViewer : Control
    {
        // Using a DependencyProperty as the backing store for DataSet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataSetProperty =
            DependencyProperty.Register("DataSet", typeof(DataSet), typeof(DataSetViewer),
                new PropertyMetadata(null, OnDataSetsChanged));

        private Label labelTotalData;
        private ListBox listBoxData;
        private ListBox listBoxStatistics;


        static DataSetViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataSetViewer),
                new FrameworkPropertyMetadata(typeof(DataSetViewer)));
        }

        public DataSet DataSet
        {
            get => (DataSet) GetValue(DataSetProperty);
            set => SetValue(DataSetProperty, value);
        }


        private static void OnDataSetsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DataSetViewer) d;

            if (e.NewValue is DataSet)
            {
                control.InitializeControls();
            }
        }

        public override void OnApplyTemplate()
        {
            InitializeControls();
            base.OnApplyTemplate();
        }


        private void InitializeControls()
        {
            labelTotalData = GetTemplateChild("LabelTotalData") as Label;
            listBoxStatistics = GetTemplateChild("ListBoxStatistics") as ListBox;
            listBoxData = GetTemplateChild("ListBoxData") as ListBox;


            var dataStat = DataSet.GetStatistics();
            if (labelTotalData != null)
            {
                labelTotalData.Content = dataStat.DatasetPairsCount;
            }

            if (listBoxStatistics != null)
            {
                listBoxStatistics.ItemsSource = dataStat.CharactersStatistics;
            }

            if (listBoxData != null)
            {
                listBoxData.ItemsSource = DataSet.DataSetPairs;
            }
        }
    }
}