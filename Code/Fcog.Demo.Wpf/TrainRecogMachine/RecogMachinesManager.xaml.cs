using Fcog.Core.Recognition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fcog.Controls.Wpf.Annotations;
using OxyPlot;


namespace Fcog.Demo.Wpf.TrainRecogMachine
{
#warning Refactor All
    /// <summary>
    /// Логика взаимодействия для RecogMachinesManager.xaml
    /// </summary>
    public partial class RecogMachinesManager:INotifyPropertyChanged
    {
        private ObservableCollection<RecogMachine> machines;
        private RecogMachine selectedMachine;
        private ObservableCollection<DataPoint> trainPoints;
        private ObservableCollection<DataPoint> testPoints;
        private string currentTrainAccuracyText;
        private string currentTestAccuracyText;
        private DataPoint currentTrainDataPoint;
        private DataPoint currentTestDataPoint;
        private DataPoint currentLossDataPoint;
        private ObservableCollection<DataPoint> lossPoints;
        private string currentLossText;
       // private RecogMachinesFileStore store;

        public string CurrentLossText
        {
            get => currentLossText;
            set
            {
                if (value == currentLossText) return;
                currentLossText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DataPoint> LossPoints
        {
            get => lossPoints;
            set
            {
                if (Equals(value, lossPoints)) return;
                lossPoints = value;
                OnPropertyChanged();
            }
        }

        public DataPoint CurrentLossDataPoint
        {
            get => currentLossDataPoint;
            set
            {
                if (value.Equals(currentLossDataPoint)) return;
                currentLossDataPoint = value;
                OnPropertyChanged();
            }
        }

        public DataPoint CurrentTrainDataPoint
        {
            get => currentTrainDataPoint;
            set
            {
                if (value.Equals(currentTrainDataPoint)) return;
                currentTrainDataPoint = value;
                OnPropertyChanged();
            }
        }


        public DataPoint CurrentTestDataPoint
        {
            get => currentTestDataPoint;
            set
            {
                if (value.Equals(currentTestDataPoint)) return;
                currentTestDataPoint = value;
                OnPropertyChanged();
            }
        }

        public string CurrentTrainAccuracyText
        {
            get => currentTrainAccuracyText;
            set
            {
                if (value == currentTrainAccuracyText) return;
                currentTrainAccuracyText = value;
                OnPropertyChanged();
            }
        }

        public string CurrentTestAccuracyText
        {
            get => currentTestAccuracyText;
            set
            {
                if (value == currentTestAccuracyText) return;
                currentTestAccuracyText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RecogMachine> Machines
        {
            get => machines;
            set
            {
                machines = value;
                OnPropertyChanged();
            }
        }

     

        public RecogMachine SelectedMachine
        {
            get => selectedMachine;
            set
            {
                if (Equals(value, selectedMachine)) return;
                selectedMachine = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DataPoint> TrainPoints
        {
            get => trainPoints;
            set
            {
                if (Equals(value, trainPoints)) return;
                trainPoints = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DataPoint> TestPoints
        {
            get => testPoints;
            set
            {
                if (Equals(value, testPoints)) return;
                testPoints = value;
                OnPropertyChanged();
            }
        }

        public RecogMachinesManager()
        {
            InitializeComponent();
          
        }

        private  void RecogMachinesManager_OnLoaded(object sender, RoutedEventArgs e)
        {
             //store=new RecogMachinesFileStore(@"../../../RecogMachines");

            //await   RecogMachinesPool.Instance.InitializeStoreAsync(store);
            Machines=new ObservableCollection<RecogMachine>(RecogMachinesPool.Instance.RecogMachines);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListBoxmachines_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is RecogMachine machine)
            {
                SelectedMachine = machine;
               
            }

        }


        private async void ButtonStartTrain_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedMachine != null && SelectedMachine.Initialized)
            {
                TestPoints = new ObservableCollection<DataPoint>();
                TrainPoints = new ObservableCollection<DataPoint>();
                LossPoints= new ObservableCollection<DataPoint>();

                SelectedMachine.EpochDone += SelectedMachine_EpochDone;
                await SelectedMachine.StartTrainAsync(TrainerType.Adam, (int) NumericUpDownBatchSize.Value,(int)NumericUpDownMaxIterations.Value,NumericUpDownMinTestAccuracy.Value.Value);

            }
        }

        private void SelectedMachine_EpochDone(object sender, EventArgs e)
        {
            var epoch = SelectedMachine.TrainResult.EpochsCount;
            var trainAccuracy = SelectedMachine.TrainResult.TrainAccuracy;
            var testAccuracy = SelectedMachine.TrainResult.TestAccuracy;
            var loss = Math.Round(SelectedMachine.TrainResult.Loss,4);

            CurrentTestAccuracyText = testAccuracy.ToString(CultureInfo.CurrentCulture);
            CurrentTrainAccuracyText =trainAccuracy.ToString(CultureInfo.CurrentCulture);
            CurrentLossText = loss.ToString(CultureInfo.CurrentCulture);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentTestDataPoint = new DataPoint(epoch, testAccuracy);
                CurrentTrainDataPoint = new DataPoint(epoch, trainAccuracy);
                CurrentLossDataPoint = new DataPoint(epoch, loss);

                TrainPoints.Add(CurrentTrainDataPoint);
                TestPoints.Add(CurrentTestDataPoint);
                LossPoints.Add(CurrentLossDataPoint);
            }));
            

        }

        private void ButtonStopTrain_OnClick(object sender, RoutedEventArgs e)
        {
            StopTrain();
        }


        private void StopTrain()
        {
            if (SelectedMachine != null && SelectedMachine.Initialized)
            {
                SelectedMachine.StopTrain();
            }
        }


        private async void ButtonSaveMachine_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedMachine != null && SelectedMachine.Initialized && !SelectedMachine.Training)
            {

                await RecogMachinesPool.Instance.SaveRecogMachineAsync(SelectedMachine);
                MessageBox.Show($"{SelectedMachine.Name} was saved");
            }
        }

        private void ButtonViewDataSets_OnClick(object sender, RoutedEventArgs e)
        {
            var dataSetViewer = new DataSetsViewer {Owner = this};
            dataSetViewer.LoadDataSets(SelectedMachine?.DataSets);
            dataSetViewer.ShowDialog();
        }
    }
}
