using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign.Util
{
    public class FileHelper
    {
        public static void WriteFile(string path, string content)
        {
            var file = File.OpenWrite(path);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(content);
            sw.Close();
        }
    }
}
