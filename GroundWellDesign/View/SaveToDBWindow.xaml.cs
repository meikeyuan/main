using GroundWellDesign.Util;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GroundWellDesign
{

    public partial class SaveToDBWindow : Window
    {
        LayerBaseParams layer;
        List<String> items = null;
        public SaveToDBWindow(LayerBaseParams layer)
        {
            InitializeComponent();
            this.layer = layer;

            // 初始化矿井名下拉列表
            items = SQLDBHelper.getAllWellName();
            comboBox.ItemsSource = items;
            if(items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        private void saveOkBtn_Click(object sender, RoutedEventArgs e)
        {
            string wellName = comboBox.Text;
            if(wellName == null || wellName.Equals(""))
            {
                MessageBox.Show(GroundWellDesign.Properties.Resources.TypeWellNamePrompt);
                return;
            }

            string uuid = Guid.NewGuid().ToString();
            bool success = SQLDBHelper.SaveLayer(layer, uuid, false, wellName, items.Contains(wellName));

            if (success)
            {
                layer.DataBaseKey = uuid;
                layer.WellNamePK = wellName;
                MessageBox.Show(GroundWellDesign.Properties.Resources.OperateOk);
                Close();
            }
            else
            {
                MessageBox.Show(GroundWellDesign.Properties.Resources.OperateNotOk);
            }
            
        }

        private void saveCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
