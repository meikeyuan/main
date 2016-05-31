﻿using System;
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

        private string yanXing;

        private bool allSelcted;
        public LayerParams WantedLayer
        {
            set;
            get;
        }

        public SelectLayerWindow(string yanXing, LayerParams wantedLayer)
        {
            InitializeComponent();

            //判断选择的岩性
            string findPath = Document.DATABASE_PATH + yanXing;
            string[] files = Directory.GetFiles(findPath);
            foreach (string filename in files)
            {
                BaseParams baseParam = (BaseParams)DataSaveAndRestore.restoreObj(filename);
                existedLayers.Add(new LayerParams(baseParam));
            }
            
            existedLayerGrid.DataContext = existedLayers;
            WantedLayer = wantedLayer;
            this.yanXing = yanXing;
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




        private void selectAllBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < existedLayerGrid.Items.Count; i++)
            {
                var row = existedLayerGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (row != null)
                {
                    var box = existedLayerGrid.Columns[0].GetCellContent(row) as CheckBox;
                    if (box != null)
                    {
                        if (allSelcted)
                        {
                            box.IsChecked = false;
                        }
                        else
                        {
                            box.IsChecked = true;
                        }
                    }
                }
            }
            allSelcted = !allSelcted;
        }


        private void selectAvgBtn_Click(object sender, RoutedEventArgs e)
        {
            LayerParams tmpLayer = new LayerParams(WantedLayer.mainWindow);
            tmpLayer.yanXing = yanXing;

            //将选择的序号存入list
            List<int> selectedList = new List<int>();
            for (int i = 0; i < existedLayerGrid.Items.Count; i++)
            {
                var row = existedLayerGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (row != null)
                {
                    var box = existedLayerGrid.Columns[0].GetCellContent(row) as CheckBox;
                    if (box != null)
                    {
                        if (box.IsChecked == true)
                        {
                            selectedList.Add(i);
                        }
                    }
                }
            }


            //首先判断有没有东西
            int count = selectedList.Count;
            if (count <= 0)
            {
                WantedLayer.copyAndEventEcpYanXing(tmpLayer);
                this.Close();
                return;
            }

            //遍历编号list
            foreach(int i in selectedList)
            {
                LayerParams layer = existedLayers[i];
                tmpLayer.leiJiShenDu += layer.leiJiShenDu;
                tmpLayer.juLiMeiShenDu += layer.juLiMeiShenDu;
                tmpLayer.cengHou += layer.cengHou;
                tmpLayer.ziRanMiDu += layer.ziRanMiDu;
                tmpLayer.bianXingMoLiang += layer.bianXingMoLiang;
                tmpLayer.kangLaQiangDu += layer.kangLaQiangDu;
                tmpLayer.kangYaQiangDu += layer.kangYaQiangDu;
                tmpLayer.tanXingMoLiang += layer.tanXingMoLiang;
                tmpLayer.boSonBi += layer.boSonBi;
                tmpLayer.neiMoCaJiao += layer.neiMoCaJiao;
                tmpLayer.nianJuLi += layer.nianJuLi;
                tmpLayer.q0 += layer.q0;
                tmpLayer.q1 += layer.q1;
                tmpLayer.q2 += layer.q2;
                tmpLayer.miaoShu = layer.miaoShu;
                tmpLayer.dataBaseNum = 0;
            }
            WantedLayer.LeiJiShenDu = tmpLayer.leiJiShenDu / count;
            WantedLayer.JuLiMeiShenDu = tmpLayer.juLiMeiShenDu / count;
            WantedLayer.CengHou = tmpLayer.cengHou / count;
            WantedLayer.ZiRanMiDu = tmpLayer.ziRanMiDu / count;
            WantedLayer.BianXingMoLiang = tmpLayer.bianXingMoLiang / count;
            WantedLayer.KangLaQiangDu = tmpLayer.kangLaQiangDu / count;
            WantedLayer.KangYaQiangDu = tmpLayer.kangYaQiangDu / count;
            WantedLayer.TanXingMoLiang = tmpLayer.tanXingMoLiang / count;
            WantedLayer.BoSonBi = tmpLayer.boSonBi / count;
            WantedLayer.NeiMoCaJiao = tmpLayer.neiMoCaJiao / count;
            WantedLayer.NianJuLi = tmpLayer.nianJuLi / count;
            WantedLayer.Q0 = tmpLayer.q0 / count;
            WantedLayer.Q1 = tmpLayer.q1 / count;
            WantedLayer.Q2 = tmpLayer.q2 / count;
            WantedLayer.MiaoShu = "取数据库平均值";
            WantedLayer.dataBaseNum = 0;
            this.Close();
        }
    }
}
