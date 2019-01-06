using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fcog.Core.Annotations;

namespace Fcog.Core.Forms
{
    public class QVersion : INotifyPropertyChanged
    {
        private int build;
        private int major;
        private int minor;

        public QVersion(int major, int minor, int build)
        {
            Major = major;
            Minor = minor;
            Build = build;
        }

        public QVersion()
        {
        }

        public int Major
        {
            get { return major; }
            set
            {
                major = value;
                OnPropertyChanged();
            }
        }

        public int Minor
        {
            get { return minor; }
            set
            {
                minor = value;
                OnPropertyChanged();
            }
        }

        public int Build
        {
            get { return build; }
            set
            {
                build = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{build}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}