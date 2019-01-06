using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Localization;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Localization;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    /// <summary>
    /// Логика взаимодействия для AddCellControl.xaml
    /// </summary>
    public partial class AddCellControl :INotifyPropertyChanged
    {
        public int CellIndex
        {
            get { return cellIndex; }
            set { cellIndex = value; OnPropertyChanged();}
        }

        public string CellLabel
        {
            get { return cellLabel; }
            set { cellLabel = value;OnPropertyChanged(); }
        }

        public Question Question
        {
            get { return (Question)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register("Question", typeof(Question), typeof(AddCellControl), new PropertyMetadata(null));

        private int cellIndex;
        private string cellLabel=UI.NewCellSign;


        public AddCellControl()
        {
            InitializeComponent();
        }

     

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (Question != null)
            {
                var cell = Question.AddCell(CellLabel);

                if (CellIndex != 0)
                {
                    cell.Index = CellIndex;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
