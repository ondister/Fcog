using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Fcog.Core.Annotations;

namespace Fcog.Core.Recognition
{
    [DataContract]
   public class TrainResult:INotifyPropertyChanged
    {
        private double trainAccuracy;
        private double loss;
        private double testAccuracy;
        private int epochsCount;

        [DataMember]
        public double Loss
        {
            get => loss;
            set
            {
                if (value.Equals(loss)) return;
                loss = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public double TrainAccuracy
        {
            get => trainAccuracy;
            set
            {
                if (value.Equals(trainAccuracy)) return;
                trainAccuracy = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public double TestAccuracy
        {
            get => testAccuracy;
            set
            {
                if (value.Equals(testAccuracy)) return;
                testAccuracy = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public int EpochsCount
        {
            get => epochsCount;
            set
            {
                if (value == epochsCount) return;
                epochsCount = value;
                OnPropertyChanged();
            }
        }





        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
