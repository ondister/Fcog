using System;


namespace Fcog.Core.Forms.Questions
{
    public class QuestionEventArgs : EventArgs
    {
        public Question Question { get; }

        public QuestionEventArgs(Question question)
        {
            Question = question;
        }
    }
}
