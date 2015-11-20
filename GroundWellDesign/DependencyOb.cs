using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GroundWellDesign
{
    public delegate void GreetingDelegate(string name);

    public class GreetingManager
    {
        //这一次我们在这里声明一个事件
        public event GreetingDelegate MakeGreet;

        public void GreetPeople(string name)
        {
            MakeGreet(name);
        }
    }

    class Program
    {
        private static void EnglishGreeting(string name)
        {
            Console.WriteLine("Morning, " + name);
        }

        private static void ChineseGreeting(string name)
        {
            Console.WriteLine("早上好, " + name);
        }

        static void Main(string[] args)
        {
            GreetingManager gm = new GreetingManager();
            gm.MakeGreet += EnglishGreeting;         // 编译错误1
            gm.MakeGreet += ChineseGreeting;

            gm.GreetPeople("Jimmy Zhang");
        }
    }

}
