using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Core.Forms;

namespace Fcog.Controls.Wpf.Questionnaire
{
    /// <summary>
    /// Логика взаимодействия для QProperties.xaml
    /// </summary>
    public partial class WpfQProperties:INotifyPropertyChanged
    {
        private QuestionnareProperties properties;

       

        public QuestionnareProperties Properties
        {
            get { return (QuestionnareProperties)GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register("Properties", typeof(QuestionnareProperties), typeof(WpfQProperties), new PropertyMetadata(null));


     

        public WpfQProperties()
        {
            InitializeComponent();
            Properties = new QuestionnareProperties
            {
                CreationDateTime = DateTime.Today,
                Version = new QVersion()
            };
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}
