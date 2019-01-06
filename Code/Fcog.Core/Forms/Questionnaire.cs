using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Fcog.Core.Annotations;

namespace Fcog.Core.Forms
{
    /// <summary>
    ///     Questionnaire containing several recognizable forms.
    /// </summary>
    public abstract class Questionnaire : INotifyPropertyChanged
    {
        protected QuestionnareProperties properties;
        protected ObservableCollection<RecogForm> recogForms;


        protected Questionnaire()
        {
            recogForms = new ObservableCollection<RecogForm>();
            RecogForms = new ReadOnlyObservableCollection<RecogForm>(recogForms);
            properties = new QuestionnareProperties();
        }

        public ReadOnlyObservableCollection<RecogForm> RecogForms { get; }


        public QuestionnareProperties Properties => properties;


        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}