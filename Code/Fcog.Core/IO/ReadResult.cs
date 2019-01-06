using System;
using System.Collections.Generic;
using Fcog.Core.Forms;

namespace Fcog.Core.IO
{
   public class ReadResult<T>
    {
      public  T Result { get; set; }

      public List<string> Messages { get; set; }

      public  List<Exception> Exceptions { get; set; }

    }
}
