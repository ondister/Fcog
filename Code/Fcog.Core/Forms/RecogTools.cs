using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Fcog.Core.Annotations;
using Fcog.Core.Barcodes;
using Fcog.Core.Serialization;

namespace Fcog.Core.Forms
{

    public class RecogTools : INotifyPropertyChanged, IWrapped<RecogToolsWrapper>
    {
        private Bitmap imageForRecognize;
        private Bitmap invertedImage;
        private BarCodeMarker marker;
        private WorkMode workMode;

        public Bitmap ImageForRecognize
            {
            get => imageForRecognize;
            internal set
            {
                imageForRecognize = value;
                OnPropertyChanged();
            }
        }

      

        public Bitmap InvertedImage
        { 
            get => invertedImage;
            internal set { invertedImage = value; OnPropertyChanged(); }
        }


        public BarCodeMarker Marker
        {
            get => marker;
            internal set { marker = value; OnPropertyChanged();}
        }

        public WorkMode WorkMode
        {
            get => workMode;
            set { workMode = value; OnPropertyChanged();}
        }

     


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public RecogToolsWrapper Wrap()
        {
            var adapter = new RecogToolsWrapper {Marker = Marker.Wrap()};
            return adapter;
        }
    }
}