using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Fcog.Core.Forms;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Serialization;

namespace Fcog.Core.IO.Templates.Readers
{
   public class TemplateFileReader:ITemplateReader
    {
        private readonly string folderName;
        public TemplateFileReader(string folderName)
        {
            this.folderName = folderName;
        }

        public ReadResult<QModelWrapper> Read(Guid questionnaireGuid)
        {
          var fileName = FindFileName(questionnaireGuid);
            return Read(fileName.FullName);
        }

        private ReadResult<QModelWrapper> Read(string fileName)
        {
            var readResult = new ReadResult<QModelWrapper>();

            var formatter = new DataContractJsonSerializer(typeof(QModelWrapper));
            
            var fileInfo= new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                using (var filestream = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    readResult.Result = (QModelWrapper)formatter.ReadObject(filestream);
                }
            }
            else
            {
                readResult.Messages.Add("Can't find file");
            }

            return readResult;
        }


        public ReadResult<List<QuestionnareProperties>> ReadAllProperties()
        {
            var result = new ReadResult<List<QuestionnareProperties>> {Result = new List<QuestionnareProperties>()};
            var directoryInfo = new DirectoryInfo(folderName);
            {
                if (directoryInfo.Exists)
                {
                    var files = directoryInfo.GetFiles($"{TemplateFileWriter.FilePreffix}*{TemplateFileWriter.FileExtension}");
                    foreach (var file in files)
                    {
                        var readResult = Read(file.FullName);
                        if (readResult.Result != null)
                        {
                            var questionnaire = readResult.Result.UnWrap();
                            result.Result.Add(questionnaire.Properties);
                        }
                    }

                }
            }

            return result;
        }

        public async Task<ReadResult<List<QuestionnareProperties>>> ReadAllPropertiesAsync()
        {
            var result = new ReadResult<List<QuestionnareProperties>> { Result = new List<QuestionnareProperties>() };

            await Task.Run(() =>
           {
               result= ReadAllProperties();
           });

            return result;
        }


        private FileInfo FindFileName(Guid guid)
        {
            FileInfo fileInfo = null;
            var directoryInfo= new DirectoryInfo(folderName);
            {
                if (directoryInfo.Exists)
                {
                    var files = directoryInfo.GetFiles(
                        $"{TemplateFileWriter.FilePreffix}{guid}{TemplateFileWriter.FileExtension}");
                    if (files.Length == 1)
                    {
                        fileInfo= files[0];
                    }
                   
                }
            }

            return fileInfo;
        }

    }
}
