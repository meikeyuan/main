using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;

namespace GroundWellDesign
{
 

    public partial class MainWindow : Window
    {
        
        

        


        public MainWindow()
        {
            InitializeComponent();

            //岩层评价系数录入表格初始化
            int types = LayerParams.YanXingOpt.Count;

            for (int i = 0; i < types; i++)
            {
                PJXS pjxs = new PJXS();
                pjxs.YanXing = LayerParams.YanXingOpt[i];
                pjxss.Add(pjxs);

            }
            pjxsGrid.DataContext = pjxss;



            //岩层其他参数录入初始化
            layers.Add(new LayerParams());
            dataGrid.DataContext = layers;

            //自动更新层号 层号不保存在集合中
            dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
            dataGrid.UnloadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_UnloadingRow);


            //向导式binding
            miaoshuTb.SetBinding(TextBox.TextProperty, new Binding("MiaoShu") { Source = editLayer });
            yanXingCB.SetBinding(ComboBox.TextProperty, new Binding("YanXing") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            caiDongCB.SetBinding(ComboBox.TextProperty, new Binding("CaiDong") { Source = editLayer, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            leiJiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("LeiJiShenDu") { Source = editLayer });
            juLiMeiShenDuTb.SetBinding(TextBox.TextProperty, new Binding("JuLiMeiShenDu") { Source = editLayer });
            cengHouTb.SetBinding(TextBox.TextProperty, new Binding("CengHou") { Source = editLayer });
            ziRanMiDuTb.SetBinding(TextBox.TextProperty, new Binding("ZiRanMiDu") { Source = editLayer });
            bianXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("BianXingMoLiang") { Source = editLayer });
            kangLaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangLaQiangDu") { Source = editLayer });
            kangYaQiangDuTb.SetBinding(TextBox.TextProperty, new Binding("KangYaQiangDu") { Source = editLayer });
            tanXingMoLiangTb.SetBinding(TextBox.TextProperty, new Binding("TanXingMoLiang") { Source = editLayer });
            boSonBiTb.SetBinding(TextBox.TextProperty, new Binding("BoSonBi") { Source = editLayer });
            neiMoCaJiaoTb.SetBinding(TextBox.TextProperty, new Binding("NeiMoCaJiao") { Source = editLayer });
            nianJuLiTb.SetBinding(TextBox.TextProperty, new Binding("NianJuLi") { Source = editLayer });



        }


        
        
       


    }



}
