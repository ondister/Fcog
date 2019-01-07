using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Fcog.Core.Annotations;
using Fcog.Core.Serialization;

namespace Fcog.Core.Forms.Cells.Content
{
    [DataContract]
    public class CellContent : INotifyPropertyChanged
    {
        private byte[] imageBytes;
        private TextView textView;

        public byte[] ImageBytes
        {
            get => imageBytes;
            set
            {
                imageBytes = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public TextView TextView
        {
            get => textView;
            set
            {
                textView = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}