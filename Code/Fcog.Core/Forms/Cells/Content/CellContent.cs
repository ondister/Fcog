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
        private Bitmap imageView;
        private TextView textView;

        public Bitmap ImageView
        {
            get => imageView;
            set
            {
                imageView = value;
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