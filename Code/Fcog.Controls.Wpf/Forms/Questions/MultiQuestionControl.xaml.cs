using System;
using System.Collections.ObjectModel;
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
    ///     Логика взаимодействия для MultiQuestionControl.xaml
    /// </summary>
    public partial class MultiQuestionControl : IQuestionControl
    {
        private ObservableCollection<ICellControl> cellControls;


        private Question question;

        public MultiQuestionControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<ICellControl> CellControls
        {
            get { return cellControls; }
            set
            {
                cellControls = value;
                OnPropertyChanged();
            }
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
                if (question?.Cells != null && CellControls == null) //convert cells to cellControls
                {
                    UpdateCellControls();
                    question.CellAdded += Question_CellAdded;
                    question.CellRemoved += Question_CellRemoved;
                }

                OnPropertyChanged();
            }
        }

        private void Question_CellRemoved(object sender, CellEventArgs args)
        {
            var controlForRemove = cellControls.FirstOrDefault(c => c.Cell.Equals(args.Cell));
            if (controlForRemove != null)
            {
                if (controlForRemove.CellRectangle != null)
                    ImageCanvas.Children.Remove(controlForRemove.CellRectangle.RectangleShape);
                cellControls.Remove(controlForRemove);
            }
        }

        private void Question_CellAdded(object sender, CellEventArgs args)
        {
            CellControls.Add(CellControlsDictionary.GetCellControl(args.Cell, ImageCanvas, DeleteCell));
        }


        private void UpdateCellControls()
        {
            CellControls = new ObservableCollection<ICellControl>(question.Cells
                .Select(c => CellControlsDictionary.GetCellControl(c, ImageCanvas, DeleteCell))
                .OrderBy(c => c.Cell.Index)
                .ToList());
        }

        private void DeleteCell(Cell cell)
        {
            question.RemoveCell(cell);
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

        private void ButtonAddCell_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}