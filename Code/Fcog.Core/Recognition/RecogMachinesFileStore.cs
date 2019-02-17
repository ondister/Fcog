using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Serialization;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Recognition
{
   public class RecogMachinesFileStore:IRecogMachinesStore
    {
        private readonly string folderName;

        private const string machineDirPreffix = "machine";

        private const string netFilePreffix = "net";
        private const string netFileExtension = ".json";

        private const string machineFilePreffix = "recog_machine";
        private const string machineFileExtension = ".json";


        private const string trainFilePreffix = "train";
        private const string testFilePreffix = "test";

        private const string imagesFileExtension = ".idx3-ubyte";
        private const string charactersFileExtension = ".idx1-ubyte";


        private const string imagesFilePreffix = "images";
        private const string charactersFilePreffix = "labels";

        public RecogMachinesFileStore(string folderName)
        {
            this.folderName = folderName;
        }


        private DirectoryInfo GetDirectory(string directoryName)
        {
          var directoryInfo= new DirectoryInfo(directoryName);
            if (!directoryInfo.Exists)
            {
                directoryInfo = Directory.CreateDirectory(directoryName);
            }
            return directoryInfo;
        }


        #region GetMachine

        public async Task<RecogMachine> GetRecogMachineAsync(Guid recogMachineGuid)
        {
            RecogMachine result = null;
            var machineDirectory = $"{folderName}{Path.DirectorySeparatorChar}{machineDirPreffix}_{recogMachineGuid}";

            var machineDirectoryInfo= new DirectoryInfo(machineDirectory);

            if (machineDirectoryInfo.Exists)
            {
                var net = await GetNetAsync(machineDirectoryInfo);

                var machine = GetMachine(machineDirectoryInfo);

                if (machine !=null && net != null)
                {
                    var trainDataSet = await Task.Run(()=> GetDataset(machineDirectoryInfo, trainFilePreffix,machine));
                    var testDataSet = await Task.Run(() => GetDataset(machineDirectoryInfo, testFilePreffix, machine));

                    if (trainDataSet != null && testDataSet != null)
                    {
                        machine.Initialize(net, new DataSets(trainDataSet,testDataSet));
                        result = machine;
                    }
                }
            }

            return result;
        }

        public Task RemoveMachineAsync(Guid recogMachineGuid)
        {
            throw new NotImplementedException();
        }

      

        private RecogMachine GetMachine(DirectoryInfo machineDirectoryInfo)
        {
            RecogMachine machine = null;

            var formatter = new DataContractJsonSerializer(typeof(RecogMachineWrapper));

            var fileName= $"{machineDirectoryInfo.FullName}{Path.DirectorySeparatorChar}{machineFilePreffix}{machineFileExtension}";

         
                using (var filestream = new FileStream(fileName, FileMode.Open))
                {
                  var recogMachineWrapper= (RecogMachineWrapper)formatter.ReadObject(filestream);

                    machine = recogMachineWrapper.UnWrap();
                }
            return machine;

        }


        private DataSet GetDataset(FileSystemInfo directoryInfo, string preffix, RecogMachine recogMachine)
        {
            DataSet dataSet = null;
            var imageFiles= Directory.GetFiles(directoryInfo.FullName, $"{preffix}*{imagesFileExtension}");
            var labelFiles = Directory.GetFiles(directoryInfo.FullName, $"{preffix}*{charactersFileExtension}");

            var images = new List<byte[]>();
            var labels = new List<byte>();

            if (imageFiles.Length == 1)
            {
                var imageFile = imageFiles.Single();

                using (var fileStream = new FileStream(imageFile,FileMode.Open,FileAccess.Read,FileShare.Delete))
                {
                    using (var reader = new BinaryReader(fileStream))
                    {
                        var magicNumber = ReverseBytes(reader.ReadInt32());
                        var numberOfImage = ReverseBytes(reader.ReadInt32());
                        var rowCount = ReverseBytes(reader.ReadInt32());
                        var columnCount = ReverseBytes(reader.ReadInt32());
                      
                        for (var index = 0; index < numberOfImage; index++)
                        {
                            var image = reader.ReadBytes(rowCount * columnCount);

                           images.Add(image);
                        }
                    }
                }
                
            }

            if (labelFiles.Length == 1)
            {
                var labelFile = labelFiles.Single();

                using (var fileStream = new FileStream(labelFile, FileMode.Open,FileAccess.Read,FileShare.Delete))
                {
                    using (var reader = new BinaryReader(fileStream))
                    {
                        var magicNumber = ReverseBytes(reader.ReadInt32());
                        var numberOfImage = ReverseBytes(reader.ReadInt32());
                     
                        for (var index = 0; index < numberOfImage; index++)
                        {
                            var label = reader.ReadByte();
                            labels.Add(label);
                        }
                    }
                }

            }

            var dataSetPairs = new List<DataSetPair>();

            if (images.Count == labels.Count)
            {
            
                for (var index = 0; index < images.Count;index++)
                {
                    var dataSetPair= new DataSetPair(images[index],recogMachine.FindCharacter(labels[index]));
#warning проверить наличие символа в машине!!!

                    dataSetPairs.Add(dataSetPair);
                }
            }

          
                dataSet= new DataSet(dataSetPairs,recogMachine.Characters);
         
            return dataSet;
        }

        private async Task<Net<double>> GetNetAsync(FileSystemInfo directoryInfo)
        {
            Net<double> net = null;
            var netFiles = Directory.GetFiles(directoryInfo.FullName, $"{netFilePreffix}{netFileExtension}");
            if (netFiles.Length == 1)
            {
                var netFile = netFiles.Single();

                using (var reader = new StreamReader(netFile))
                {
                    var json= await reader.ReadToEndAsync();

                    net = SerializationExtensions.FromJson<double>(json);
                }
            }
           

            return net;
        }

        #endregion

        #region SaveMachine
        public async Task SaveDataSetsAsync(RecogMachine recogMachine)
        {
            if (recogMachine.Initialized)
            {
             var directoryInfo = GetDirectory($"{folderName}{Path.DirectorySeparatorChar}{machineDirPreffix}_{recogMachine.Id}");

              await Task.Run(() => SaveDataSet(new DirectoryInfo($"{directoryInfo.FullName}{Path.DirectorySeparatorChar}{testFilePreffix}"), recogMachine.DataSets.TestDataSet));
              await Task.Run(() => SaveDataSet(new DirectoryInfo($"{directoryInfo.FullName}{Path.DirectorySeparatorChar}{trainFilePreffix}"), recogMachine.DataSets.TrainDataSet));
            }
        }

        public async Task SaveRecogMachineAsync(RecogMachine recogMachine)
        {
            if (recogMachine.Initialized)
            {
                var directoryInfo = GetDirectory($"{folderName}{Path.DirectorySeparatorChar}{machineDirPreffix}_{recogMachine.Id}");

                SaveMachine(directoryInfo, recogMachine);

                await SaveNetAsync(directoryInfo, recogMachine.ConvolutionNet);

                await SaveDataSetsAsync(recogMachine);
            }
        }

        private void SaveMachine(FileSystemInfo directoryInfo, RecogMachine recogMachine)
        {
            var settings =
                new DataContractJsonSerializerSettings
                    { UseSimpleDictionaryFormat = true };

            var fileName = $"{directoryInfo.FullName}{Path.DirectorySeparatorChar}{machineFilePreffix}{machineFileExtension}";

            var recogMachineWrapper = recogMachine.Wrap();
               
            using (var filestream = new FileStream(fileName, FileMode.Create))
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
                    filestream, Encoding.UTF8, true, true, "  "))
                {
                    var serializer = new DataContractJsonSerializer(recogMachineWrapper.GetType(), settings);
                    serializer.WriteObject(writer, recogMachineWrapper);
              
                    writer.Flush();
                }
                       
            }
               
            
        }

        private async Task SaveNetAsync(FileSystemInfo directoryInfo, Net<double> convolutionNet)
        {
            var serializedNet = convolutionNet.ToJson();
            var fileName = $"{directoryInfo.FullName}{Path.DirectorySeparatorChar}{netFilePreffix}{netFileExtension}";

            using (var filestream = new FileStream(fileName, FileMode.Create))
            {
                var data = new UTF8Encoding(true).GetBytes(serializedNet);

                await filestream.WriteAsync(data, 0, data.Length);
            }

        }
        
        private void SaveDataSet(FileSystemInfo directoryInfo, DataSet dataSet)
        {
#warning add error check
            var imageFileName = $"{directoryInfo}{imagesFilePreffix}{imagesFileExtension}";
            var charactersFileName = $"{directoryInfo}{charactersFilePreffix}{charactersFileExtension}";
            //write bytes data
            File.Delete(imageFileName);
            using (var filestream = new FileStream(imageFileName, FileMode.CreateNew))
            {
                using (var writer = new BinaryWriter(filestream))
                {
                    writer.Write(ReverseBytes(0x00000801));//magic number 
                    writer.Write(ReverseBytes(dataSet.Count));//number of images 
                    writer.Write(ReverseBytes(DataSet.ImageWidth));//number of rows 
                    writer.Write(ReverseBytes(DataSet.ImageHeight));//number of columns 
                    //data
                    foreach (var pair in dataSet.DataSetPairs)
                    {
                        foreach (var byteItem in pair.ImageBytes)
                        {
                            writer.Write(byteItem);
                        }
                       
                    }
                }
            }
            //write labels data
            File.Delete(charactersFileName);
            using (var filestream = new FileStream(charactersFileName, FileMode.CreateNew))
            {
                using (var writer = new BinaryWriter(filestream))
                {
                    writer.Write(ReverseBytes(0x00000801));//magic number 
                    writer.Write(ReverseBytes(dataSet.Count));//number of items
                
                    //data
                    foreach (var pair in dataSet.DataSetPairs)
                    {
                        writer.Write(pair.Character.Index);
                    }
                }
            }


        }

        #endregion

        public async Task<ReadOnlyCollection<RecogMachine>> GetRecogMachinesAsync()
        {
            var machines = new List<RecogMachine>();

            var directory= new DirectoryInfo(folderName);
            if (directory.Exists)
            {
                
                    foreach (var machineDirectory in directory.GetDirectories())
                    {
                        var guidString = machineDirectory.Name.Split('_');
                        if (guidString.Length == 2)
                        {
                            if (Guid.TryParse(guidString[1],out var guid))
                            {
                                var machine = await GetRecogMachineAsync(guid);
                                if (machine != null)
                                {
                                    machines.Add(machine);
                                }
                            }
                        }
                    }
               
               
            }

            return new ReadOnlyCollection<RecogMachine>(machines);
        }


        private static int ReverseBytes(int value)
        {
            var intAsBytes = BitConverter.GetBytes(value);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }

    }
}
