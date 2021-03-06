﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    class FileDialogHelper
    {
        // 通过打开文件对话框，获得保存路径。
        public static string getSavePath(string fileName, string filter)
        {
            System.Windows.Forms.SaveFileDialog fileDialog = new System.Windows.Forms.SaveFileDialog();
            fileDialog.Title = "保存文件";
            fileDialog.Filter = filter;
            fileDialog.FileName = fileName;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        // 通过打开文件对话框，获得打开路径。
        public static string getOpenPath(string filter)
        {
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Title = "打开文件";
            fileDialog.Filter = filter;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
