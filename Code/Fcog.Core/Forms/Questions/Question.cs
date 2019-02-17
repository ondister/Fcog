using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Fcog.Core.Annotations;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    public abstract class Question : IRecognizable, INotifyPropertyChanged, IEquatable<Question>,
        IWrapped<QuestionWrapper>
    {
        protected ObservableCollection<Cell> cells;

        protected readonly Guid guid;
        private int index;
        private string label;

        private RecogTools recogTools;

        private Question()
        {
            cells = new ObservableCollection<Cell>();
            Cells = new ReadOnlyObservableCollection<Cell>(cells);
        }

        private Question(RecogTools recogTools) :this()
        {
            RecogTools = recogTools;
        }


        internal Question(string label, RecogTools recogTools) : this(recogTools)
        {
            Label = label;
            guid = Guid.NewGuid();
           
        }

        internal Question(string label, Guid guid, RecogTools recogTools) : this(recogTools)
        {
            Label = label;
            this.guid = guid;
        }



        public string Label
        {
            get => label;
            set
            {
                label = value;
                OnPropertyChanged();
            }
        }

        public int Index
        {
            get => index;
            set
            {
                index = value;
                OnPropertyChanged();
            }
        }

        public ReadOnlyObservableCollection<Cell> Cells { get; }
        public Guid Guid => guid;

      
       

        public RecogTools RecogTools
        {
            get => recogTools;
            set
            {
                recogTools = value;
                UpdateCellsRecogTools();
            }
        }

        public RecogMachine RecogMachine { get; internal set; }
    

        public void Recognize()
        {
#warning сделать проверку на наличие всего
            foreach (var cell in Cells)
            {
                cell.Recognize();
            }
        }

        public async Task RecognizeAsync()
        {
            foreach (var cell in Cells)
            {
                await cell.RecognizeAsync();
            }
        }

        internal virtual void SetRecogMachine(RecogMachine recogMachine)
        {
            RecogMachine = recogMachine;
            foreach (var cell in Cells)
            {
                cell.SetRecogMachine(RecogMachine);
            }
        }

        public event EventHandler<CellEventArgs> CellAdded;
        public event EventHandler<CellEventArgs> CellRemoved;

        private void UpdateCellsRecogTools()
        {
            foreach (var cell in cells)
            {
                cell.RecogTools = RecogTools;
            }
        }

       
        public abstract Cell AddCell(string cellLabel);

        internal abstract void AddCell(Cell cell);

        public void RemoveCell(Cell cell)
        {
            cells.Remove(cell);
            OnCellRemoved(new CellEventArgs(cell));
        }

        protected virtual void OnCellAdded(CellEventArgs e)
        {
            CellAdded?.Invoke(this, e);
        }

        protected virtual void OnCellRemoved(CellEventArgs e)
        {
            CellRemoved?.Invoke(this, e);
        }

        #region IWrapped<QuestionWrapper>

        public abstract QuestionWrapper Wrap();

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IEquatable<Question>

        public bool Equals(Question other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return guid.Equals(other.guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Question) obj);
        }

        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        #endregion
    }
}