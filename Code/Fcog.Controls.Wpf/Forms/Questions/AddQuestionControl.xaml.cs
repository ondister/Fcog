using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Localization;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Recognition;


namespace Fcog.Controls.Wpf.Forms.Questions
{
    /// <summary>
    /// Логика взаимодействия для AddQuestionControl.xaml
    /// </summary>
    public partial class AddQuestionControl:INotifyPropertyChanged
    {
        private int questionIndex;
        private string questionLabel;
        private Type selectedQuestionType;
        private RecogMachine selectedMachine;

        public Type SelectedQuestionType
        {
            get { return selectedQuestionType; }
            set { selectedQuestionType = value; OnPropertyChanged(); }
        }

        public RecogMachine SelectedMachine
        {
            get { return selectedMachine; }
            set { selectedMachine = value; OnPropertyChanged();}
        }

        public event EventHandler<QuestionEventArgs> ButtonOkClick;

        public RecogForm RecogForm
        {
            get { return (RecogForm)GetValue(RecogFormProperty); }
            set { SetValue(RecogFormProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecogForm.
        public static readonly DependencyProperty RecogFormProperty = DependencyProperty.Register("RecogForm", typeof(RecogForm), typeof(AddQuestionControl), new PropertyMetadata(null));

        
        public Dictionary<Type, string> QuestionTypeDictionary { get; } = new Dictionary<Type, string>
        {
            { typeof(MarkQuestion),MarkQuestion.TypeDescription},
            { typeof(MultiQuestion),MultiQuestion.TypeDescription},
            { typeof(SingleQuestion),SingleQuestion.TypeDescription},
            { typeof(RecogTextQuestion),RecogTextQuestion.TypeDescription},
            { typeof(TextQuestion),TextQuestion.TypeDescription},
        };

        public IEnumerable<RecogMachine> MachineCollection { get; set; }


       public int QuestionIndex
        {
            get { return questionIndex; }
            set { questionIndex = value; OnPropertyChanged();}
        }

        public string QuestionLabel
        {
            get { return questionLabel; }
            set { questionLabel = value; OnPropertyChanged(); }
        }

        public AddQuestionControl()
        {
            InitializeComponent();
           

            FillMachines();

        }

        private void FillMachines()
        {
            MachineCollection = RecogMachinesPool.Instance.RecogMachines;
            OnPropertyChanged(nameof(MachineCollection));
        }




        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedQuestionType != null && SelectedMachine!=null && !string.IsNullOrEmpty(QuestionLabel))
                {
                    var question = RecogForm.AddQuestion(selectedQuestionType, questionLabel, SelectedMachine);
                    if (QuestionIndex != 0)
                    {
                        question.Index = questionIndex;
                    }

                    ClearValues();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void ClearValues()
        {
            QuestionLabel = UI.NewQuestionSign;
            QuestionIndex = 0;
           // SelectedQuestionType = null;
           // SelectedLanguage = null;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected virtual void OnButtonOkClick(QuestionEventArgs args)
        {
            ButtonOkClick?.Invoke(this, args);
        }
    }
}
