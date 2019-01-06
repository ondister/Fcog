using System;
using System.Collections.Generic;

namespace Fcog.Core.IO
{
   public class WriteResult
   {
       public List<string> Messages { get; set; }
       public List<Exception> Exceptions { get; set; }
   }
}
