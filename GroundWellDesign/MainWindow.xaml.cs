using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using System.Drawing;
using AxMxDrawXLib;
using MxDrawXLib;

namespace GroundWellDesign
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //录入的岩性评价系数
        ObservableCollection<PJXS> pjxss = new ObservableCollection<PJXS>();
        //所有已经录入岩层参数
        ObservableCollection<LayerParams> layers = new ObservableCollection<LayerParams>();
        //向导式当前编辑的岩层参数
        LayerParams editLayer = new LayerParams();


        public MainWindow()
        {
            InitializeComponent();

            int types = LayerParams.YanXingOpt.Count;

            for (int i = 0; i < types; i++)
            {
                PJXS pjxs = new PJXS();
                pjxs.YanXing = LayerParams.YanXingOpt[i];
                pjxss.Add(pjxs);

            }

            pjxsGrid.DataContext = pjxss;




            layers.Add(new LayerParams());
            dataGrid.DataContext = layers;

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


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {

            saveEdit();
        }


        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            editLayer.reset();
        }

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
            if (layerNum > 1 && layerNum <= layers.Count + 1) { 
                currLayerTb.Text = layerNum - 1 + "";
                editLayer.copy(layers[layerNum - 2]);
            }

        }

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

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {

            tabControl.SelectedItem = tabControl.Items[1];

        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "打开一个dwg文件";
            fileDialog.Filter = "dwg文件(*.dwg)|*.dwg";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filePath = fileDialog.FileName;
                mxDraw.OpenDwgFile(filePath);
            }


        }


        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            MxDrawSelectionSet ss = new MxDrawSelectionSet();

            IMxDrawResbuf spFilter = new MxDrawResbuf();


            ss.Select2(MCAD_McSelect.mcSelectionSetUserSelect, null, null, null, null);

            for (int i = 0; i < ss.Count; i++)
            {
                MxDrawEntity ent = ss.Item(i);

                if (ent is MxDrawMText)
                {
                    MxDrawMText mText = (MxDrawMText)ent;
                    if (mText.Contents.EndsWith("m"))
                    {
                        mText.Contents = "长度标注";
                    }
                    else
                    {
                        mText.Contents = "其他标注";
                    }

                }
                else if (ent is MxDrawDimRotated)
                {
                    MxDrawDimRotated dim = (MxDrawDimRotated)ent;
                    MessageBox.Show(dim.DimensionText);
                    if (dim.DimensionText.EndsWith("m"))
                    {
                        dim.DimensionText = "长度标注";
                    }
                    else
                    {
                        dim.DimensionText = "其他标注";
                    }
                }


            }

            mxDraw.Regen();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


    }



    public class LayerParams : INotifyPropertyChanged
    {

        public static List<String> YanXingOpt { get; set; }
        public static List<String> CaiDongOpt { get; set; }
        static LayerParams()
        {
            YanXingOpt = new List<string> { "黄土", "细粒砂岩", "泥岩", "中粒砂岩", "粉砂岩", "砂质泥岩", "粗粒砂岩", "细砂岩", "中砂岩", "煤" };
            CaiDongOpt = new List<string> { "初次采动Q0", "重复采动Q1", "重复采动Q2" };
        }


        public LayerParams()
        {
            YanXing = YanXingOpt[0];
            CaiDong = CaiDongOpt[0];

            LeiJiShenDu = "";
            JuLiMeiShenDu = "";
            CengHou = "";
            ZiRanMiDu = "";
            BianXingMoLiang = "";
            KangLaQiangDu = "";
            KangYaQiangDu = "";
            TanXingMoLiang = "";
            BoSonBi = "";
            NeiMoCaJiao = "";
            NianJuLi = "";
            MiaoShu = "";

        }

        public LayerParams(LayerParams layer)
        {
            copy(layer);
        }



        public bool equals(LayerParams layer)
        {
            return YanXing.Equals(layer.YanXing) &&
            CaiDong.Equals(layer.CaiDong) &&
            LeiJiShenDu.Equals(layer.LeiJiShenDu) &&
            JuLiMeiShenDu.Equals(layer.JuLiMeiShenDu) &&
            CengHou.Equals(layer.CengHou) &&
            ZiRanMiDu.Equals(layer.ZiRanMiDu) &&
            BianXingMoLiang.Equals(layer.BianXingMoLiang) &&
            KangLaQiangDu.Equals(layer.KangLaQiangDu) &&
            KangYaQiangDu.Equals(layer.KangYaQiangDu) &&
            TanXingMoLiang.Equals(layer.TanXingMoLiang) &&
            BoSonBi.Equals(layer.BoSonBi) &&
            NeiMoCaJiao.Equals(layer.NeiMoCaJiao) &&
            NianJuLi.Equals(layer.NianJuLi) && MiaoShu.Equals(layer.MiaoShu);

        }
        public void reset()
        {
            copy(new LayerParams());
        }

        public void copy(LayerParams layer)
        {
            YanXing = layer.YanXing;
            CaiDong = layer.CaiDong;
            LeiJiShenDu = layer.LeiJiShenDu;
            JuLiMeiShenDu = layer.JuLiMeiShenDu;
            CengHou = layer.CengHou;
            ZiRanMiDu = layer.ZiRanMiDu;
            BianXingMoLiang = layer.BianXingMoLiang;
            KangLaQiangDu = layer.KangLaQiangDu;
            KangYaQiangDu = layer.KangYaQiangDu;
            TanXingMoLiang = layer.TanXingMoLiang;
            BoSonBi = layer.BoSonBi;
            NeiMoCaJiao = layer.NeiMoCaJiao;
            NianJuLi = layer.NianJuLi;
            MiaoShu = layer.MiaoShu;
        }

        String yanXing;
        public String YanXing
        {
            get { return yanXing; }
            set
            {
                yanXing = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("YanXing"));
                }
            }
        }

        String caiDong;
        public String CaiDong
        {
            get { return caiDong; }
            set
            {
                caiDong = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CaiDong"));
                }
            }
        }
        String leiJiShenDu;
        public String LeiJiShenDu
        {
            get { return leiJiShenDu; }
            set
            {
                leiJiShenDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LeiJiShenDu"));
                }
            }
        }
        String juLiMeiShenDu;
        public String JuLiMeiShenDu
        {
            get { return juLiMeiShenDu; }
            set
            {
                juLiMeiShenDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("JuLiMeiShenDu"));
                }
            }
        }

        String cengHou;
        public String CengHou
        {
            get { return cengHou; }
            set
            {
                cengHou = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CengHou"));
                }
            }
        }

        String ziRanMiDu;
        public String ZiRanMiDu
        {
            get { return ziRanMiDu; }
            set
            {
                ziRanMiDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ZiRanMiDu"));
                }
            }
        }
        String bianXingMoLiang;
        public String BianXingMoLiang
        {
            get { return bianXingMoLiang; }
            set
            {
                bianXingMoLiang = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BianXingMoLiang"));
                }
            }
        }

        String kangLaQiangDu;
        public String KangLaQiangDu
        {
            get { return kangLaQiangDu; }
            set
            {
                kangLaQiangDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("KangLaQiangDu"));
                }
            }
        }
        String kangYaQiangDu;
        public String KangYaQiangDu
        {
            get { return kangYaQiangDu; }
            set
            {
                kangYaQiangDu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("KangYaQiangDu"));
                }
            }
        }
        String tanXingMoLiang;
        public String TanXingMoLiang
        {
            get { return tanXingMoLiang; }
            set
            {
                tanXingMoLiang = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TanXingMoLiang"));
                }
            }
        }
        String boSonBi;
        public String BoSonBi
        {
            get { return boSonBi; }
            set
            {
                boSonBi = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BoSonBi"));
                }
            }
        }
        String neiMoCaJiao;
        public String NeiMoCaJiao
        {
            get { return neiMoCaJiao; }
            set
            {
                neiMoCaJiao = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NeiMoCaJiao"));
                }
            }
        }
        String nianJuLi;
        public String NianJuLi
        {
            get { return nianJuLi; }
            set
            {
                nianJuLi = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NianJuLi"));
                }
            }
        }

        String miaoShu;
        public String MiaoShu
        {
            get { return miaoShu; }
            set
            {
                miaoShu = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MiaoShu"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

    }


    public class PJXS
    {

        public PJXS()
        {

        }

        public PJXS(PJXS pjxs)
        {
            copy(pjxs);
        }


        public void reset()
        {
            copy(new PJXS());
        }

        public void copy(PJXS pjxs)
        {
            YanXing = pjxs.YanXing;
            Q0 = pjxs.Q0;
            Q1 = pjxs.Q1;
            Q2 = pjxs.Q2;

        }

        public String YanXing { get; set; }

        public String Q0 { get; set; }

        public String Q1 { get; set; }
        public String Q2 { get; set; }
    }

}
