using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Fcog.Core.Serialization;

namespace Fcog.Core.IO.Templates.Writers
{
    public class TemplateFileWriter : ITemplateWriter
    {
        private readonly string folderName;
        internal static string FilePreffix = "questionnaire_";
        internal static string FileExtension = ".json";
        public TemplateFileWriter(string folderName)
        {
            this.folderName = folderName;
        }

        public WriteResult Write(QModelWrapper questionnaire)
        {
            var result = new WriteResult();

           var settings =
                new DataContractJsonSerializerSettings
                    { UseSimpleDictionaryFormat = true };

            try
            {
                var formatter = new DataContractJsonSerializer(questionnaire.GetType());
                var fileName = CreateFileName(questionnaire);
                
                if (!string.IsNullOrEmpty(fileName))
                {
                    using (var filestream = new FileStream(fileName, FileMode.OpenOrCreate))
                    {
                        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
                            filestream, Encoding.UTF8, true, true, "  "))
                        {
                            var serializer = new DataContractJsonSerializer(questionnaire.GetType(), settings);
                            serializer.WriteObject(writer, questionnaire);
                            writer.Flush();
                        }
                        // formatter.WriteObject(filestream, questionnaire);
                    }
                }
                else
                {
                    result.Messages.Add("Cant create fileName");
                }
            }
            catch (Exception ex)
            {
                result.Exceptions.Add(ex);
            }

            return result;
        }

        private string CreateFileName(QModelWrapper questionnaire)
        {
            var directoryInfo = new DirectoryInfo(folderName);
            
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            var result = $"{directoryInfo.FullName}{Path.DirectorySeparatorChar}{FilePreffix}{questionnaire.Properties.Guid.ToString()}{FileExtension}";
            return result;
        }
    }
}