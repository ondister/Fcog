using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Forms.Cells;
using Fcog.Core.Forms.Questions;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    /// <summary>
    ///     Логика взаимодействия для UnknownQuestionControl.xaml
    /// </summary>
    public partial class UnknownQuestionControl : IQuestionControl
    {
        private Question question;


        public UnknownQuestionControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<ICellControl> CellControls { get; set; }


        public Canvas ImageCanvas { get; set; }
        public DeleteQuestionHandler DeleteQuestionAction { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Question Question
        {
            get { return question; }
            set
            {
                question = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}