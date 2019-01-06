using System;
using System.Collections.Generic;
using System.Linq;
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
using Path = System.IO.Path;

namespace Fcog.Demo.Wpf.BarCode
{
    /// <summary>
    /// Логика взаимодействия для BarcodeCreatorWindow.xaml
    /// </summary>
    public partial class BarcodeCreatorWindow 
    {
         public int Id { get; set; }

        public int FormsNumber { get; set; }

        public BarcodeCreatorWindow()
        {
            InitializeComponent();
           
        }

        private void Buttoncreate_OnClick(object sender, RoutedEventArgs e)
        {
            if (FormsNumber != 0)
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                   var result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        var folderName = dialog.SelectedPath;
                        for (var formNumber = 1; formNumber <= FormsNumber; formNumber++)
                        {
                            var fileName = $"{folderName}{Path.DirectorySeparatorChar}BarCode_{Id}_{formNumber}.jpg";
                            Core.Barcodes.BarCode.Create(fileName,Id,formNumber);
                        }

                        Close();
                    }
                }
            }
        }
    }
}
