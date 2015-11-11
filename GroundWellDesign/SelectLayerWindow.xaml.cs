using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace GroundWellDesign
{
    /// <summary>
    /// SelectLayerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLayerWindow : Window
    {



        private ObservableCollection<LayerParams> existedLayers = new ObservableCollection<LayerParams>();


        public LayerParams WantedLayer
        {
            set;
            get;
        }

        public SelectLayerWindow(string yanXing, LayerParams wantedLayer)
        {
            InitializeComponent();

            //判断选择的岩性
            string findPath = MainWindow.FILE_PATH + yanXing;
            string[] files = Directory.GetFiles(findPath);
            foreach (string filename in files)
            {
                BaseParams baseParam = (BaseParams)DataSaveAndRestore.restoreObj(filename);
                existedLayers.Add(new LayerParams(baseParam));
            }
            
            existedLayerGrid.DataContext = existedLayers;
            WantedLayer = wantedLayer;
        }

        private void selectChooseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (existedLayerGrid.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一项");
                return;
            }


            WantedLayer.copyAndEventEcpYanXing(existedLayers[existedLayerGrid.SelectedIndex]);

            this.Close();
        }

        private void selectAvgBtn_Click(object sender, RoutedEventArgs e)
        {
            WantedLayer.miaoShu = "you get Avg";

            this.Close();
        }





    }
}
