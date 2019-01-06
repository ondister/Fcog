using System;
using System.Collections.Generic;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Training;

namespace Fcog.Core.Recognition
{
    internal static class TrainersFactory
    {
        private static readonly IDictionary<TrainerType, Func<int, Net<double>, TrainerBase<double>>> trainersDictionary
            = new Dictionary<TrainerType, Func<int, Net<double>, TrainerBase<double>>>
            {
                {TrainerType.Sgd, CreateSgd},
                {TrainerType.Adam, CreateAdam}
            };

        public static TrainerBase<double> CreateTrainer(TrainerType trainerType, int batchSize, Net<double> net)
        {
            trainersDictionary.TryGetValue(trainerType, out var factoryMethod);

            return factoryMethod?.Invoke(batchSize, net);
        }

        private static TrainerBase<double> CreateSgd(int batchSize, Net<double> net)
        {
            var trainer = new SgdTrainer<double>(net)
            {
                BatchSize = batchSize,
                LearningRate = 0.01,
                L2Decay = 0.001,
                Momentum = 0.9
            };
            return trainer;
        }

        private static TrainerBase<double> CreateAdam(int batchSize, Net<double> net)
        {
            var trainer = new AdamTrainer<double>(net)
            {
                BatchSize = batchSize,
                LearningRate = 0.001,
                Beta1 = 0.9,
                Beta2 = 0.999,
                Eps = 1e-08
            };
            return trainer;
        }


        //properties of the adam trainer
        //alpha.Also referred to as the learning rate or step size.The proportion that weights are updated (e.g. 0.001). Larger values(e.g. 0.3) results in faster initial learning before the rate is updated.Smaller values(e.g. 1.0E-5) slow learning right down during training
        //beta1.The exponential decay rate for the first moment estimates(e.g. 0.9).
        //beta2.The exponential decay rate for the second-moment estimates(e.g. 0.999). This value should be set close to 1.0 on problems with a sparse gradient(e.g.NLP and computer vision problems).
        //epsilon.Is a very small number to prevent any division by zero in the implementation(e.g. 10E-8).
        //TensorFlow: learning_rate=0.001, beta1=0.9, beta2=0.999, epsilon=1e-08.
        //Keras: lr=0.001, beta_1=0.9, beta_2=0.999, epsilon=1e-08, decay=0.0.
        //Blocks: learning_rate=0.002, beta1=0.9, beta2=0.999, epsilon=1e-08, decay_factor=1.
        //Lasagne: learning_rate=0.001, beta1=0.9, beta2=0.999, epsilon=1e-08
        //Caffe: learning_rate=0.001, beta1=0.9, beta2=0.999, epsilon=1e-08
        //MxNet: learning_rate=0.001, beta1=0.9, beta2=0.999, epsilon=1e-8
        //Torch: learning_rate=0.001, beta1=0.9, beta2=0.999, epsilon=1e-8
    }
}