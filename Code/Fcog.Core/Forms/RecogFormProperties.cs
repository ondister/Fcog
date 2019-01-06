using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Fcog.Core.Annotations;

namespace Fcog.Core.Forms
{
    /// <summary>
    ///     Class RecogFormProperties.
    ///     Each form must have an identifier and an index in properties.
    /// </summary>
    [DataContract]
    public class RecogFormProperties : INotifyPropertyChanged
    {
        private int formId;
        private int formIndex;
        private double markerHeight;
        private double markerHeightTolerance;
        private double markerWidth;
        private double markerWidthTolerance;

        /// <summary>Gets or sets the height of the marker.</summary>
        /// <value>The height of the marker.</value>
        [DataMember]
        public double MarkerHeight
        {
            get { return markerHeight; }
            set
            {
                markerHeight = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the width of the marker.
        /// </summary>
        /// <value>The width of the marker.</value>
        [DataMember]
        public double MarkerWidth
        {
            get { return markerWidth; }
            set
            {
                markerWidth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the marker height tolerance.
        /// </summary>
        /// <value>The marker height tolerance.</value>
        [DataMember]
        public double MarkerHeightTolerance
        {
            get { return markerHeightTolerance; }
            set
            {
                markerHeightTolerance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the marker width tolerance.
        /// </summary>
        [DataMember]
        public double MarkerWidthTolerance
        {
            get { return markerWidthTolerance; }
            set
            {
                markerWidthTolerance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the form identifier.
        /// </summary>
        /// <value>The form identifier.</value>
        [DataMember]
        public int FormId
        {
            get { return formId; }
            set
            {
                formId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the index of the form.
        /// </summary>
        /// <value>The index of the form.</value>
        [DataMember]
        public int FormIndex
        {
            get { return formIndex; }
            set
            {
                formIndex = value;
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