using System.Windows;

namespace GroundWellDesign
{
    partial class MainWindow : Window


    {

        //向导式当前编辑的岩层参数
        LayerParams editLayer = new LayerParams();


        //保存
        private void saveEdit()
        {
            int layerNum = int.Parse(currLayerTb.Text);
            if (layerNum > layers.Count + 1)
            {
                MessageBox.Show("已经录入" + layers.Count + "层,请修改层号再保存。");
                return;
            }


            if (layerNum == layers.Count + 1)
            {
                LayerParams layer = new LayerParams(editLayer);
                layers.Add(layer);
            }
            else
                layers[layerNum - 1].copy(editLayer);

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

            if (layerNum <= layers.Count && !layers[layerNum - 1].equals(editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？") == MessageBoxResult.OK)
                {
                    saveEdit();
                }


            }
            if (layerNum > 1 && layerNum <= layers.Count + 1)
            {
                currLayerTb.Text = layerNum - 1 + "";
                editLayer.copy(layers[layerNum - 2]);
            }

        }

        //下一层按钮
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            int layerNum = int.Parse(currLayerTb.Text);
            if (layerNum <= layers.Count && !layers[layerNum - 1].equals(editLayer) || layerNum == layers.Count + 1)
            {
                if (MessageBox.Show("保存修改？") == MessageBoxResult.OK)
                {
                    saveEdit();
                }
            }


            if (layerNum < layers.Count)
            {
                currLayerTb.Text = layerNum + 1 + "";
                editLayer.copy(layers[layerNum]);
            }
            else if (layerNum == layers.Count)
            {
                currLayerTb.Text = layerNum + 1 + "";
                editLayer.reset();
            }

        }

        //完成按钮
        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {

            tabControl.SelectedItem = tabControl.Items[1];

        }




    }
}
