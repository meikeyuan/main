using GroundWellDesign.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    partial class Document : Window
    {
        // 提醒用户是否需要保存的判断
        private bool layer_equal(LayerBaseParams layer1, LayerBaseParams layer2)
        {
            if(layer1 == null || layer2 == null)
            {
                return false;
            }

            for(int i = 0; i < 17; ++i)
            {
                if(layer1[i] != layer2[i] && !layer1[i].Equals(layer2[i]))
                {
                    return false;
                }
            }

            return true;
        }


        //保存
        private void saveEdit()
        {
            int layerNum = int.Parse(currLayerCombo.Text);
            if (layerNum > layers.Count)
            {
                MessageBox.Show("已经录入" + layers.Count + "层,请修改层号再保存。", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (layerNum == layers.Count)
            {
                LayerBaseParamsViewModel layer = new LayerBaseParamsViewModel(this, editLayer.LayerParams);
                layers.Add(layer);
                layer.CengHou = layer.CengHou;
            }
            else
            {
                LayerBaseParamsViewModel layer = new LayerBaseParamsViewModel(this, editLayer.LayerParams);
                layers[layerNum] = layer;
                layer.CengHou = layer.CengHou;
            }
                

        }

        //保存按钮
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            saveEdit();
        }

        //重置按钮
        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            editLayer.reset();
        }

        //上一层按钮
        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerCombo.Text);
            if (layerNum < layers.Count)
            {
                if (!layer_equal(layers[layerNum].LayerParams, editLayer.LayerParams))
                    if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        saveEdit();
                    }

                if (layerNum > 0)
                {
                    currLayerCombo.SelectedIndex -= 1;
                }
            }
            else
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                    currLayerCombo.SelectedIndex -= 1;
                }
                else
                {
                    LayerNbrs.RemoveAt(layerNum);
                    currLayerCombo.SelectedIndex -= 1;
                }
            }
        }

        //下一层按钮
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerCombo.Text);
            if (layerNum < layers.Count)
            {
                if(!layer_equal(layers[layerNum].LayerParams, editLayer.LayerParams))
                    if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        saveEdit();
                    }

                if (layerNum == layers.Count - 1)
                {
                    LayerNbrs.Add(layerNum + 1);
                }
                currLayerCombo.SelectedIndex += 1;
            }
            else
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                    LayerNbrs.Add(layerNum + 1);
                    currLayerCombo.SelectedIndex += 1;
                }
            }
        }

        //完成按钮
        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = gridinputTabItem;
        }

        private void currLayerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!currLayerCombo.HasItems)
                return;

            int index = currLayerCombo.SelectedIndex;
            if (index == -1)
            {
                currLayerCombo.SelectedIndex = 0;
                return;
            }

            if (index == layers.Count)
                editLayer.reset();
            else
                editLayer.LayerParams = layers[index].LayerParams;

            guideBind(editLayer);
        }
    }
}
