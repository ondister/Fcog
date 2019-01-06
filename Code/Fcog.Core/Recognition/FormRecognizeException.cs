using System;
using System.Runtime.Serialization;

namespace Fcog.Core.Recognition
{
 public   class FormRecognizeException : ApplicationException
    {
        #region Constructors

        public FormRecognizeException()
        {
        }

        public FormRecognizeException(string message) : base(message)
        {
        }

        public FormRecognizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FormRecognizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
    }
}
