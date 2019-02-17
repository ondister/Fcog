using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Forms;
using Fcog.Core.Forms;
using Fcog.Core.IO.Templates.Readers;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Recognition;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Fcog.Demo.Wpf.RecogQuestionnaire
{
   
    public partial class FormRecogWindow : INotifyPropertyChanged
    {
        private const string fileStorePath = "Templates";

        private readonly QRecog questionnaire;
        private RecogForm activeForm;
        private ObservableCollection<WpfRecogForm> formControls;

       

        public FormRecogWindow(QuestionnareProperties qProperties)
        {
            InitializeComponent();
            FormControls = new ObservableCollection<WpfRecogForm>();

            var filereader= new TemplateFileReader(fileStorePath);

            var result = filereader.Read(qProperties.Guid);
            if (result.Result != null)
            {
                 questionnaire = result.Result.UnWrap();
                foreach (var r in questionnaire.RecogForms)
                {
                    r.RecogTools.WorkMode = WorkMode.RecognizeForm;
                    var formControl = new WpfRecogForm { RecognitionForm = r};
                    FormControls.Add(formControl);
                }
            }


            ListBoxForms.SelectedIndex = 0;
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

    
       
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var templateFileWriter = new TemplateFileWriter(fileStorePath);
            try
            {
                //save datsets
               RecogMachinesPool.Instance.SaveAllDataSets();
              //  questionnaire.SaveTemplate(templateFileWriter);
              
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
            ButtonAddImage.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
        }

        private void StopProgress()
        {
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

        private async void ButtonndRecognize_OnClick(object sender, RoutedEventArgs e)
        {
            StartProgress();

            try
            {
                var isFormImageValid = await ActiveForm.FindAndCheckMarkerAsync();
                if (isFormImageValid)
                {
                       await  ActiveForm.RecognizeAsync();
                }
                else
                {
                    MessageBox.Show("Form image Id and index not equal with loaded temlape", "Error");
                }
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
    }
}