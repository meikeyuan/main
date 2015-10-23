using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using AxMxDrawXLib;


namespace GroundWellDesign
{

    public partial class MainWindow : Window
    {

        AxMxDrawX cadViewer;
        

        public MainWindow()
        {
            InitializeComponent();

            //岩层参数录入初始化
            layers.Add(new LayerParams());
            paramGrid.DataContext = layers;


            //自动更新层号 层号不保存在集合中
            paramGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            paramGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);


            //向导式binding
            miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu") { Source = editLayer });
            yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            caiDongCB.SetBinding(ComboBox.TextProperty, new Binding("CaiDong") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer });
            cengHouTb.SetBinding(TextBox.TextProperty, new Binding("CengHou") { Source = editLayer });
            ziRanMiDuTb.SetBinding(TextBox.TextProperty, new Binding("ZiRanMiDu") { Source = editLayer });
            bianXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("BianXingMoLiang") { Source = editLayer });
            kangLaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangLaQiangDu") { Source = editLayer });
            kangYaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangYaQiangDu") { Source = editLayer });
            tanXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("TanXingMoLiang") { Source = editLayer });
            boSonBiTb.SetBinding(TextBox.TextProperty, new Binding("BoSonBi") { Source = editLayer });
            neiMoCaJiaoTb.SetBinding(TextBox.TextProperty, new Binding("NeiMoCaJiao") { Source = editLayer });
            nianJuLiTb.SetBinding(TextBox.TextProperty, new Binding("NianJuLi") { Source = editLayer });
            q0Tb.SetBinding(TextBox.TextProperty, new Binding("Q0") { Source = editLayer });
            q1Tb.SetBinding(TextBox.TextProperty, new Binding("Q1") { Source = editLayer });
            q2Tb.SetBinding(TextBox.TextProperty, new Binding("Q2") { Source = editLayer });


            otherDataGrid.DataContext = keyLayers;


            cadViewer = new AxMxDrawX();
            cadViewer.BeginInit();
            wfHost.Child = cadViewer;
            cadViewer.EndInit();
            cadViewer.OpenDwgFile("示意钻井结构.dwg");

        }

        

        
    }



}
