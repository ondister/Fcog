using Fcog.Core.Recognition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Fcog.Demo.Wpf.TrainRecogMachine
{
    /// <summary>
    /// Логика взаимодействия для RecogMachinesManager.xaml
    /// </summary>
    public partial class RecogMachinesManager:INotifyPropertyChanged
    {
        private ObservableCollection<RecogMachine> machines;
        private RecogMachine selectedMachine;

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

        public RecogMachinesManager()
        {
            InitializeComponent();
        }

        private async void RecogMachinesManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            var store=new RecogMachinesFileStore(@"../../../RecogMachines");

            await   RecogMachinesPool.Instance.InitializeStoreAsync(store);
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
    }
}
