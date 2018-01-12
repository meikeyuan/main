using GroundWellDesign.Properties;
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
            StreamWriter sw = null;
            try
            {
                var file = File.OpenWrite(path);
                sw = new StreamWriter(file);
                sw.Write(content);
            }
            catch(Exception e)
            {
                App.logger.Fatal(Resources.WriteFileError, e);
            }
            finally
            {
                if(sw != null)
                    sw.Close();
            }
        }
    }
}
