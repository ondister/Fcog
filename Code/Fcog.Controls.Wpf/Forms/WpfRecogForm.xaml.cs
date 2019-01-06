using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Forms.Questions;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Questions;
using MousePoint = System.Windows.Point;

namespace Fcog.Controls.Wpf.Forms
{
    /// <summary>
    ///     Логика взаимодействия для WpfRecogCanvas.xaml
    /// </summary>
    public partial class WpfRecogForm : INotifyPropertyChanged
    {
        private readonly ImageBrush imageBackGround;

        private ObservableCollection<IQuestionControl> questionControls;
        private RecogForm recognitionForm;

        public WpfRecogForm()
        {
            InitializeComponent();
            imageBackGround = new ImageBrush
            {
                TileMode = TileMode.None,
                Stretch = Stretch.None
            };
        }

        public RecogForm RecognitionForm
        {
            get { return recognitionForm; }
            set
            {
                recognitionForm = value;

                recognitionForm.MarkerFinded += delegate
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        AddBackGroundImage(recognitionForm.RecogTools.ImageForRecognize);
                        //set questions panel to visible after marker finding
                        GridQuestionControls.Visibility = Visibility.Visible;
                    }));

                };
                recognitionForm.ImageChanged += delegate
                {
                    AddBackGroundImage(recognitionForm.RecogTools.ImageForRecognize);
                };

                if (recognitionForm != null) AddBackGroundImage(recognitionForm.RecogTools.ImageForRecognize);

                if (recognitionForm.Questions != null )
                {
                    QuestionControls = new ObservableCollection<IQuestionControl>(recognitionForm.Questions .Select(q => QuestionControlsDictionary.GetQuestionControl(q, ImageCanvas, DeleteQuestion)) .OrderBy(q => q.Question.Index).ToList());
                    recognitionForm.QuestionAdded += RecognitionForm_QuestionAdded;
                    recognitionForm.QuestionRemoved += RecognitionForm_QuestionRemoved;
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<IQuestionControl> QuestionControls
        {
            get { return questionControls; }
            set
            {
                questionControls = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RecognitionForm_QuestionRemoved(object sender, QuestionEventArgs args)
        {
            var controlForRemove = questionControls.FirstOrDefault(q => q.Question.Equals(args.Question));
            if (controlForRemove != null)
            {
                questionControls.Remove(controlForRemove);
            }
        }

        private void RecognitionForm_QuestionAdded(object sender, QuestionEventArgs args)
        {
            var questionControl =
                QuestionControlsDictionary.GetQuestionControl(args.Question, ImageCanvas, DeleteQuestion);
            QuestionControls.Add(questionControl);
        }


        private void DeleteQuestion(Question question)
        {
            RecognitionForm.RemoveQuestion(question);
        }

      
        private void AddBackGroundImage(Bitmap image)
        {
            if (image != null)
            {
                var imageSource = BitmapToBitmapSourceConverter.Convert(image);
               ImageCanvas.Height = imageSource.Height;
               ImageCanvas.Width = imageSource.Width; 

              imageBackGround.ImageSource = imageSource;
                ImageCanvas.Background = imageBackGround;
            }
            else
            {   ImageCanvas.Children.Clear();
                imageBackGround.ImageSource = null;
                ImageCanvas.Background = null;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}