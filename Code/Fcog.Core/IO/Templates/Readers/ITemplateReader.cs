using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fcog.Core.Forms;
using Fcog.Core.Serialization;

namespace Fcog.Core.IO.Templates.Readers
{
  public interface ITemplateReader
  {
      ReadResult<QModelWrapper> Read(Guid questionnaireGuid);

      ReadResult<List<QuestionnareProperties>> ReadAllProperties();
   
      Task<ReadResult<List<QuestionnareProperties>>> ReadAllPropertiesAsync();

    }

  
}
