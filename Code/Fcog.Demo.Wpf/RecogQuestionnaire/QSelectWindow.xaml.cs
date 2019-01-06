using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Core.Forms;
using Fcog.Core.IO.Templates.Readers;

namespace Fcog.Demo.Wpf.RecogQuestionnaire
{
    /// <summary>
    ///     Логика взаимодействия для QSelectWindow.xaml
    /// </summary>
    public partial class QSelectWindow : INotifyPropertyChanged
    {
        private const string fileStorePath = "Templates";

        private ObservableCollection<QuestionnareProperties> questionnaires;

        public QSelectWindow()
        {
            InitializeComponent();
            Questionnaires = new ObservableCollection<QuestionnareProperties>();
        }

        public ObservableCollection<QuestionnareProperties> Questionnaires
        {
            get { return questionnaires; }
            set
            {
                questionnaires = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartProgress()
        {
            ProgressBar.Visibility = Visibility.Visible;
        }

        private void StopProgress()
        {
            ProgressBar.Visibility = Visibility.Hidden;
        }


        private async void QSelectWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            StartProgress();
            try
            {
                var filereader = new TemplateFileReader(fileStorePath);
                var result = await filereader.ReadAllPropertiesAsync();
                if (result.Result.Any())
                {
                    Questionnaires = new ObservableCollection<QuestionnareProperties>(result.Result);
                }
            }
            finally
            {
                StopProgress();
            }
        }

        private void ListBoxForms_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.AddedItems[0] as QuestionnareProperties;

            ButtonOpen.IsEnabled = selectedItem != null;
        }

        private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var properties = ListBoxForms.SelectedValue as QuestionnareProperties;
            if (properties != null)
            {
                var formRecogWindow = new FormRecogWindow(properties) {Owner = this.Owner};

                Close();

                formRecogWindow.ShowDialog();
            }
        }
    }
}