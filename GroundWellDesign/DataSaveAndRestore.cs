using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GroundWellDesign
{
    class DataSaveAndRestore
    {
        public static void saveObj(object obj, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

            //通过formatter对象以二进制格式将obj对象序列化后到文件中
            formatter.Serialize(stream, obj);
            stream.Close();
        }


        public static object restoreObj(string path)
        {
            //重新取回数据 
            IFormatter formatter = new BinaryFormatter();
            Stream stream2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);

            object obj = formatter.Deserialize(stream2);
            stream2.Close();

            return obj;
        }


   


        [Serializable]
        public class DataToSave
        {
            public ObservableCollection<BaseParams> Layers
            {
                get;
                set;
            }
            
            public List<int> KeyLayerNbr
            {
                get;
                set;
            }

            
            public ObservableCollection<BaseKeyParams> KeyLayers
            {
                get;
                set;
            }

        }
    }
}
