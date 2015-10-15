using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    public partial class MainWindow : Window

    {
        //所有已经录入岩层参数
        ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();

        void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dataGrid_LoadingRow(sender, e);
            if (dataGrid.Items != null)
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    try
                    {
                        DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            row.Header = (i + 1).ToString();
                        }
                    }
                    catch { }
                }
            }

        }




        //删除
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex == -1)
                return;
            layers.RemoveAt(selectedIndex);

        }

        //下方插入
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            layers.Insert(dataGrid.SelectedIndex + 1, new LayerParams());
        }

        //上移
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex == -1 || selectedIndex == 0)
                return;

            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex - 1];
            layers[selectedIndex - 1] = layer;
        }

        //下移
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid.SelectedIndex;
            if (selectedIndex == -1 || selectedIndex == dataGrid.Items.Count - 1)
                return;

            LayerParams layer = layers[selectedIndex];
            layers[selectedIndex] = layers[selectedIndex + 1];
            layers[selectedIndex + 1] = layer;
        }






    }
}
