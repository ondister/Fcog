using System;
using System.Drawing;
using System.Windows;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Questions;
using Fcog.Core.IO.Templates.Readers;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Recognition;
using Fcog.Demo.Wpf.BarCode;
using Fcog.Demo.Wpf.CreateQuestionnaire;
using Fcog.Demo.Wpf.RecogQuestionnaire;
using Fcog.Demo.Wpf.TrainRecogMachine;

namespace Fcog.Demo.Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
      

        public MainWindow()
        {
            InitializeComponent();
        }

      
        private void ButtonCreateBarCode_OnClick(object sender, RoutedEventArgs e)
        {
            var barCodeWindow = new BarcodeCreatorWindow {Owner = this};
            barCodeWindow.ShowDialog();
        }

        private void ButtonCreateQuestionnaire_OnClick(object sender, RoutedEventArgs e)
        {
            var createQuestionnaireWindow = new QPropWindow{Owner = this};
            createQuestionnaireWindow.ShowDialog();

        }

        private void ButtonRecognizequestionnaire_OnClick(object sender, RoutedEventArgs e)
        {
          var qSelectWindow= new QSelectWindow { Owner=this};
            qSelectWindow.ShowDialog();
        }

        private void ButtonTrainRecogs_OnClick(object sender, RoutedEventArgs e)
        {
            var machinesManager = new RecogMachinesManager {Owner = this};
            machinesManager.ShowDialog();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            StartProgress();

            var store= new RecogMachinesFileStore(DemoSettings.RecogMachinesPath);
            await RecogMachinesPool.Instance.InitializeStoreAsync(store);

            StopProgress();
        }

        private void StartProgress()
        {
            ProgressBar.Visibility = Visibility.Visible;
        }

        private void StopProgress()
        {
            ProgressBar.Visibility = Visibility.Collapsed;
        }
    }
}
