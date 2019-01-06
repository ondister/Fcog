using System;
using System.Collections.ObjectModel;
using Fcog.Core.Forms.Questions;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using Fcog.Controls.Wpf.Forms.Cells;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    public delegate void DeleteQuestionHandler(Question question);
    public interface IQuestionControl:INotifyPropertyChanged
    {
        Question Question { get; set; }

        Canvas ImageCanvas { get; set; }
        DeleteQuestionHandler DeleteQuestionAction { get; set; }
    }
}