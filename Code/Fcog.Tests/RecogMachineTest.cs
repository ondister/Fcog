using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using Fcog.Core.Recognition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fcog.Tests
{
    [TestClass]
    public class RecogMachinetest
    {
        private const string storePath = "recogMachines";

        private const string machinesPath = "../../RecogMachines";

        [TestMethod]
        public void MachineCreate()
        {
            var digitalMachine = new RecogMachine("Mnist machine");

            digitalMachine.AddCharacter(0, "0");
            digitalMachine.AddCharacter(1, "1");
            digitalMachine.AddCharacter(2, "2");
            digitalMachine.AddCharacter(3, "3");
            digitalMachine.AddCharacter(4, "4");
            digitalMachine.AddCharacter(5, "5");
            digitalMachine.AddCharacter(6, "6");
            digitalMachine.AddCharacter(7, "7");
            digitalMachine.AddCharacter(8, "8");
            digitalMachine.AddCharacter(9, "9");

#warning очень важно проиниуиализировать машину. Нужно добавить исключение
            digitalMachine.Initialize();


            digitalMachine.DataSets.TrainDataSet.AddDataSetPair(new DataSetPair(new byte[]{0,1,1},new Character(0,"0")));
            digitalMachine.DataSets.TestDataSet.AddDataSetPair(new DataSetPair(new byte[] { 0, 1, 1 }, new Character(0, "0")));

            var machineGuid = digitalMachine.Id;

            var store = new RecogMachinesFileStore(storePath);
            store.SaveRecogMachineAsync(digitalMachine).Wait();

            var restoredMachine = store.GetRecogMachineAsync(machineGuid).Result;

            Assert.AreEqual(digitalMachine.Name, restoredMachine.Name);
            Assert.AreEqual(digitalMachine.Id,restoredMachine.Id);
            Assert.IsTrue(restoredMachine.Characters.Any());
            Assert.AreEqual(digitalMachine.Characters.Count, restoredMachine.Characters.Count);
            Assert.IsNotNull(restoredMachine.ConvolutionNet);
            Assert.IsNotNull(restoredMachine.DataSets?.TrainDataSet);
            Assert.IsNotNull(restoredMachine.DataSets?.TestDataSet);
        }

        [TestMethod]
        public void MachineTrain()
        {
            var store = new RecogMachinesFileStore(machinesPath);
            var machine = store.GetRecogMachineAsync(Guid.Parse("09c2f6b6-9db7-4afe-a1d4-64ce1156e3ba")).Result;

            Assert.IsNotNull(machine);

            machine.EpochDone += delegate
            {
                Console.WriteLine(
                    $"train acc: {machine.TrainResult.TrainAccuracy} test acc: {machine.TrainResult.TestAccuracy} loss:{machine.TrainResult.Loss}");
            };

            machine.StartTrainAsync(TrainerType.Sgd, 20, 100).Wait();

            Assert.IsTrue(machine.TrainResult.TrainAccuracy!=0);
            Assert.IsTrue(machine.TrainResult.TestAccuracy != 0);

            store.SaveRecogMachineAsync(machine).Wait();

        }

        [TestMethod]
        public void MachineForecast()
        {
            var imagesCount = 600;
            var store = new RecogMachinesFileStore(machinesPath);
            var machine = store.GetRecogMachineAsync(Guid.Parse("09c2f6b6-9db7-4afe-a1d4-64ce1156e3ba")).Result;

            Assert.IsNotNull(machine);

            var input = machine.DataSets.TestDataSet.NextBatch(imagesCount);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            machine.ConvolutionNet.Forward(input.InputVolume);
            var prediction=    machine.ConvolutionNet.GetPrediction();

            stopWatch.Stop();

            var buffer= new CircularBuffer<double>(imagesCount);
            for (var i = 0; i < input.Characters.Count; i++)
            {
               buffer.Add(input.Characters[i].Index == prediction[i] ? 1.0 : 0.0);
            }


            Console.WriteLine($"elapsed: {stopWatch.ElapsedMilliseconds} ms");

            Console.WriteLine($"acc: {buffer.Items.Average()*100.0}");


        }
    }
}