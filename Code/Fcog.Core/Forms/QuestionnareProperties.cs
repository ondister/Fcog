using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Fcog.Core.Annotations;

namespace Fcog.Core.Forms
{
    [DataContract]
    public class QuestionnareProperties : INotifyPropertyChanged
    {
        private string author;
        private DateTime? creationDateTime;
        private string description;
        private Guid guid;
        private string name;
        private QVersion version;

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public QVersion Version
        {
            get { return version; }
            set
            {
                version = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public Guid Guid
        {
            get { return guid; }
            set
            {
                guid = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public DateTime? CreationDateTime
        {
            get { return creationDateTime; }
            set
            {
                creationDateTime = value;
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