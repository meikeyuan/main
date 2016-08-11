﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GroundWellDesign
{
    /// <summary>
    /// SaveToDBWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SaveToDBWindow : Window
    {
        LayerBaseParams layer;
        List<String> items = new List<string>();
        public SaveToDBWindow(LayerBaseParams layer)
        {
            InitializeComponent();
            this.layer = layer;

            DataSaveAndRestore.getAllWellName(items);

            comboBox.ItemsSource = items;
            if(items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        private void saveOkBtn_Click(object sender, RoutedEventArgs e)
        {
            string wellName = comboBox.Text;
            if(wellName.Equals(""))
            {
                MessageBox.Show("请输入矿井名称");
                return;
            }

            string uuid = Guid.NewGuid().ToString();
            bool success = DataSaveAndRestore.saveToSqlite(layer, uuid, false, wellName, items.Contains(wellName));

            if (success)
            {
                layer.dataBaseKey = uuid;
                layer.wellNamePK = wellName;
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
            
            Close();
        }

        private void saveCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
