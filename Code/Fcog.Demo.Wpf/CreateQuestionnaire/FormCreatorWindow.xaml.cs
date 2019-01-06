using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Forms;
using Fcog.Core.Forms;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Recognition;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Fcog.Demo.Wpf.CreateQuestionnaire
{
    /// <summary>
    ///     Логика взаимодействия для FormCreatorWindow.xaml
    /// </summary>
    public partial class FormCreatorWindow : INotifyPropertyChanged
    {
        private const string fileStorePath = "Templates";

        private readonly QModel questionnaire;
        private RecogForm activeForm;
        private ObservableCollection<WpfRecogForm> formControls;

        private RecogFormProperties formProperties;

        public FormCreatorWindow(QuestionnareProperties qProperties)
        {
            InitializeComponent();
            FormControls = new ObservableCollection<WpfRecogForm>();
            questionnaire = new QModel(qProperties);
            AddForm();
        }

        public ObservableCollection<WpfRecogForm> FormControls
        {
            get { return formControls; }
            set
            {
                formControls = value;
                OnPropertyChanged();
            }
        }

        public RecogForm ActiveForm
        {
            get { return activeForm; }
            private set
            {
                activeForm = value;
                OnPropertyChanged();
            }
        }

    
        private void ButtonAddForm_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActiveForm.Questions.Any() && ActiveForm.IsAllCellsFounded())
            {
                AddForm();
            }
            else
            {
                MessageBox.Show("You must select all cell of each question", "Warning");
            }
        }

        private void AddForm()
        {
            formProperties = new RecogFormProperties
            {
                MarkerHeight = 8d,
                MarkerHeightTolerance = 3d,
                MarkerWidth = 80d,
                MarkerWidthTolerance = 10d
            };

            ActiveForm = questionnaire.AddRecogForm(formProperties);
            var formControl = new WpfRecogForm {RecognitionForm = ActiveForm};
            FormControls.Add(formControl);

        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var templateFileWriter = new TemplateFileWriter(fileStorePath);
            try
            {
                questionnaire.SaveTemplate(templateFileWriter);
                Close();
            }
            catch (FormRecognizeException ex)
            {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        #region Progress

        private void StartProgress()
        {
            ButtonAddForm.IsEnabled = false;
            ButtonAddImage.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
        }

        private void StopProgress()
        {
            ButtonAddForm.IsEnabled = true;
            ButtonAddImage.IsEnabled = true;
            ProgressBar.Visibility = Visibility.Hidden;
        }

        #endregion

        private void ButtonAddImage_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActiveForm.RecogTools.ImageForRecognize == null)
            {
                var fileDialog = new OpenFileDialog
                {
                    Filter = @"*.jpg; *.jpeg|*.jpg; *.jpeg",
                    Title = @"Select Form Image",
                    Multiselect = false
                };
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var fileName = fileDialog.FileName;
                    var bitmap = new Bitmap(fileName);
                    ActiveForm.AddImage(bitmap);
                }
            }
        }

        private void ButtonRotateLeft_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActiveForm.RecogTools.ImageForRecognize != null && ActiveForm.RecogTools.Marker == null)
            {
                ActiveForm.RotateLeft();
            }
        }

        private void ButtonRotateRight_OnClick(object sender, RoutedEventArgs e)
        {
            if (ActiveForm.RecogTools.ImageForRecognize != null && ActiveForm.RecogTools.Marker == null)
            {
                ActiveForm.RotateRight();
            }
        }

        private async void ButtonFindMarker_OnClick(object sender, RoutedEventArgs e)
        {
            StartProgress();

            try
            {
                await ActiveForm.FindMarkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                StopProgress();
            }
        }

        private void ListBoxForms_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var formControl = e.AddedItems[0] as WpfRecogForm;
            if (formControl != null)
            {
                GroupBoxForm.Content=formControl;
                ActiveForm = formControl.RecognitionForm;
            }
        }

        #region INotifyProperty

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}