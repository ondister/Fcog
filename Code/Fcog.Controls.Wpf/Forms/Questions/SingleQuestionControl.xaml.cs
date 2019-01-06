using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Controls.Wpf.Forms.Cells;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Forms.Questions;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    /// <summary>
    ///     Логика взаимодействия для MarkQuestionControl.xaml
    /// </summary>
    public partial class SingleQuestionControl : IQuestionControl
    {
        private ICellControl cellControl;
        private Question question;


        public SingleQuestionControl()
        {
            InitializeComponent();
        }

        public Canvas ImageCanvas { get; set; }
        public DeleteQuestionHandler DeleteQuestionAction { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public Question Question
        {
            get { return question; }
            set
            {
                question = value;
                if (question?.Cells != null && cellControl == null) //convert cell to cellControl
                {
                    question.CellAdded += Question_CellAdded;
                    question.CellRemoved += Question_CellRemoved;
                    UpdateCellControls();
                }
                OnPropertyChanged();
            }
        }

        private void Question_CellRemoved(object sender, CellEventArgs e)
        {
            if (cellControl != null)
            {
                if (cellControl.CellRectangle != null)
                {
                    ImageCanvas.Children.Remove(cellControl.CellRectangle.RectangleShape);
                }
                StackPanelCellControls.Children.Clear();
                cellControl = null;
            }
        }

        private void Question_CellAdded(object sender, CellEventArgs e)
        {
            UpdateCellControls();
        }

        private void UpdateCellControls()
        {
            if (question.Cells.Count == 1)
            {
                cellControl = CellControlsDictionary.GetCellControl(question.Cells.Single(), ImageCanvas, DeleteCell);
                StackPanelCellControls.Children.Add(cellControl as UIElement);
            }
            else
            {
                StackPanelCellControls.Children.Clear();
            }
        }


        private void DeleteCell(Cell cell)
        {
           Question.RemoveCell(cell);
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

   
        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteQuestionAction(question);
        }
    }
}