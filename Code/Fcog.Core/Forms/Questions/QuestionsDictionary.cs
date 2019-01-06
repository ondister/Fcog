using System;
using System.Collections.Generic;

namespace Fcog.Core.Forms.Questions
{
    public static class QuestionsDictionary
    {
        #region IQuestion 

        private static readonly Dictionary<Type, Func<string, RecogTools, Question>> questionsDictionary =
            new Dictionary<Type, Func<string,RecogTools, Question>>
            {
                {typeof(MarkQuestion), GetMarkQuestion},
                {typeof(MultiQuestion), GetMultiQuestion},
                {typeof(SingleQuestion), GetSimpleTextQuestion},
                {typeof(TextQuestion), GetTextQuestion},
                {typeof(RecogTextQuestion), GetRecogTextQuestion}
            };

        private static Question GetRecogTextQuestion(string label,RecogTools recogTools)
        {
            return new RecogTextQuestion(label, recogTools);
        }

        private static Question GetTextQuestion(string label,RecogTools recogTools)
        {
            return new TextQuestion(label, recogTools);
        }

        private static Question GetSimpleTextQuestion(string label, RecogTools recogTools)
        {
            return new SingleQuestion(label, recogTools);
        }


        private static Question GetMultiQuestion(string label, RecogTools recogTools)
        {
            return new MultiQuestion(label, recogTools);
        }

        private static Question GetMarkQuestion(string label, RecogTools recogTools)
        {
            return new MarkQuestion(label, recogTools);
        }


        public static Question GetQuestion( Type type, string label, RecogTools recogTools) 
        {
            Func<string, RecogTools, Question> getQuestionFunction;
            questionsDictionary.TryGetValue(type, out getQuestionFunction);

            return getQuestionFunction?.Invoke(label, recogTools);
        }

      

        #endregion

        #region IQuestion Guid

        private static readonly Dictionary<Type, Func<string, Guid, RecogTools,Question>> questionsGuidDictionary =
            new Dictionary<Type, Func<string, Guid, RecogTools,Question>>
            {
                {typeof(MarkQuestion), GetMarkQuestion},
                {typeof(MultiQuestion), GetMultiQuestion},
                {typeof(SingleQuestion), GetSimpleTextQuestion},
                {typeof(TextQuestion), GetTextQuestion},
                {typeof(RecogTextQuestion), GetRecogTextQuestion}
            };

        private static Question GetRecogTextQuestion(string label, Guid guid, RecogTools recogTools)
        {
            return new RecogTextQuestion(label, guid, recogTools);
        }

        private static Question GetTextQuestion(string label, Guid guid, RecogTools recogTools)
        {
            return new TextQuestion(label, guid, recogTools);
        }

        private static Question GetSimpleTextQuestion(string label, Guid guid, RecogTools recogTools)
        {
            return new SingleQuestion(label, guid, recogTools);
        }


        private static Question GetMultiQuestion(string label, Guid guid, RecogTools recogTools)
        {
            return new MultiQuestion(label, guid, recogTools);
        }

        private static Question GetMarkQuestion(string label, Guid guid, RecogTools recogTools)
        {
            return new MarkQuestion(label, guid, recogTools);
        }


        public static Question GetQuestion(Type type, string label, Guid guid, RecogTools recogTools) 
        {
            Func<string, Guid, RecogTools, Question> getQuestionFunction;
            questionsGuidDictionary.TryGetValue(type, out getQuestionFunction);

            return getQuestionFunction?.Invoke(label, guid, recogTools);
        }

        #endregion
    }
}