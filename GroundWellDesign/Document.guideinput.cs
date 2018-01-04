using System.Windows;
using System.Windows.Controls;

namespace GroundWellDesign
{
    partial class Document : Window
    {
        // 提醒用户是否需要保存的判断
        private bool layer_equal(BaseLayerBaseParams layer1, BaseLayerBaseParams layer2)
        {
            if(layer1 == null || layer2 == null)
            {
                return false;
            }

            for(int i = 0; i < 17; ++i)
            {
                if(layer1[i] != layer2[i] || !layer1[i].Equals(layer2[i]))
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
            if (layerNum > layers.Count + 1)
            {
                MessageBox.Show("已经录入" + layers.Count + "层,请修改层号再保存。", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (layerNum == layers.Count + 1)
            {
                LayerBaseParams layer = new LayerBaseParams(this, editLayer);
                layers.Add(layer);
                layer.CengHou = layer.cengHou;
            }
            else
            {
                LayerBaseParams layer = new LayerBaseParams(this, editLayer);
                layers[layerNum - 1] = layer;
                layer.CengHou = layer.cengHou;
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

            if (layerNum == 1)
                return;

            if (layerNum <= layers.Count && !layer_equal(layers[layerNum - 1], editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                }


            }
            if (layerNum > 1 && layerNum <= layers.Count + 1)
            {
                currLayerCombo.SelectedIndex = layerNum - 2;
            }

        }

        //下一层按钮
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerCombo.Text);
            if (layerNum <= layers.Count && !layer_equal(layers[layerNum - 1], editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                }
            }


            if (layerNum < layers.Count)
            {
                currLayerCombo.SelectedIndex = layerNum;
            }
            else if (layerNum == layers.Count)
            {
                if(LayerNbrs.Count < layerNum + 1)
                    LayerNbrs.Add(layerNum + 1);
                currLayerCombo.SelectedIndex = layerNum;
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
                editLayer.copy(layers[index]);

            guideBind(editLayer);
        }



    }
}
