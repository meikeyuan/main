using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_add;
using MathWorks.MATLAB.NET.Arrays;

namespace cal
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void cal_Click(object sender, EventArgs e)
        {
            MatlabAdd matlabAdd = new MatlabAdd();
            MWArray a = double.Parse(inputA.Text), b = double.Parse(inputB.Text);
            MWNumericArray c = (MWNumericArray)matlabAdd.test_add(a, b);
            Array arr=c.ToArray();

            result.Text = c.ToScalarDouble() + "";

            MWNumericArray x = new MWNumericArray(8,2,new double[16]{
                1,11,
                2,12,
                3,13,
                4,14,
                5,15,
                6,16,
                7,17,
                8,18
            });
            matlabAdd.test_plot(x);
        }
    }
}
