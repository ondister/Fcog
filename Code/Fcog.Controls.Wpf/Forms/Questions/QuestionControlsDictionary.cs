using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Fcog.Core.Forms.Questions;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    public static class QuestionControlsDictionary
    {
        private static readonly
            Dictionary<Type, Func<Question, Canvas, DeleteQuestionHandler,
                IQuestionControl>> questionControlsDictionary =
                new Dictionary<Type, Func<Question, Canvas, DeleteQuestionHandler,
                    IQuestionControl>>
                {
                    {typeof(MultiQuestion), GetMultiQuestionControl},
                    {typeof(SingleQuestion), GetMultiQuestionControl},
                    {typeof(MarkQuestion), GetSingleQuestionControl},
                    {typeof(RecogTextQuestion), GetMultiQuestionControl},
                    {typeof(TextQuestion), GetSingleQuestionControl}
                };


        private static IQuestionControl GetSingleQuestionControl(Question question, Canvas canvas,
            DeleteQuestionHandler deleteQuestionAction)
        {
            return new SingleQuestionControl
            {
                ImageCanvas = canvas,
                DeleteQuestionAction = deleteQuestionAction,
                Question = question
            };
        }

        private static IQuestionControl GetMultiQuestionControl(Question question, Canvas canvas,
            DeleteQuestionHandler deleteQuestionAction)
        {
            return new MultiQuestionControl
            {
                ImageCanvas = canvas,
                DeleteQuestionAction = deleteQuestionAction,
                Question = question
            };
        }

        private static IQuestionControl GetUnknownQuestionControl(Question question, Canvas canvas,
            DeleteQuestionHandler deleteQuestionAction)
        {
            return new UnknownQuestionControl
            {
                ImageCanvas = canvas,
                DeleteQuestionAction = deleteQuestionAction,
                Question = question               
            };
        }


        public static IQuestionControl GetQuestionControl(Question question, Canvas canvas,
            DeleteQuestionHandler deleteQuestionAction)
        {
            Func<Question, Canvas, DeleteQuestionHandler, IQuestionControl>
                getQuestionFunction;
            questionControlsDictionary.TryGetValue(question.GetType(), out getQuestionFunction);
            return getQuestionFunction != null
                ? getQuestionFunction(question, canvas, deleteQuestionAction)
                : GetUnknownQuestionControl(question, canvas, deleteQuestionAction);
        }
    }
}