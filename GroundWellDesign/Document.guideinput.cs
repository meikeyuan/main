using System.Windows;

namespace GroundWellDesign
{
    partial class Document : Window
    {
        //保存
        private void saveEdit()
        {
            int layerNum = int.Parse(currLayerTb.Text);
            if (layerNum > layers.Count + 1)
            {
                MessageBox.Show("已经录入" + layers.Count + "层,请修改层号再保存。", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (layerNum == layers.Count + 1)
            {
                LayerBaseParams layer = new LayerBaseParams(editLayer);
                layers.Add(layer);
            }
            else
            {
                LayerBaseParams layer = new LayerBaseParams(editLayer);
                layers[layerNum - 1] = layer;
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
            int layerNum = int.Parse(currLayerTb.Text);

            if (layerNum == 1)
                return;

            if (layerNum <= layers.Count && !layers[layerNum - 1].Equals(editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                }


            }
            if (layerNum > 1 && layerNum <= layers.Count + 1)
            {
                currLayerTb.Text = layerNum - 1 + "";
                editLayer.copyNoEvent(layers[layerNum - 2]);
                guideBind(editLayer);
            }

        }

        //下一层按钮
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerTb.Text);
            if (layerNum <= layers.Count && !layers[layerNum - 1].Equals(editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    saveEdit();
                }
            }


            if (layerNum < layers.Count)
            {
                currLayerTb.Text = layerNum + 1 + "";
                editLayer.copyNoEvent(layers[layerNum]);
                guideBind(editLayer);
            }
            else if (layerNum == layers.Count)
            {
                currLayerTb.Text = layerNum + 1 + "";
                editLayer.reset();
                guideBind(editLayer);
            }

        }

        //完成按钮
        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = gridinputTabItem;
        }




    }
}
