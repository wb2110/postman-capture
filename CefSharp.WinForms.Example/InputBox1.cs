using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public partial class mybox : Form
    {
        public EventHandler onClose;
        public mybox(string url)
        {
            InitializeComponent();
            this.Text = url;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ea = new MyBoxEvent();
            ea.text = textBox1.Text;
            ea.bCanel = false;
            if (string.IsNullOrEmpty(ea.text)) {
                MessageBox.Show("输入个名字吧!");
            }

            this.onClose.Invoke(this, ea);
            this.Close();
        }
        public void setText(string s) {
            textBox1.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ea = new MyBoxEvent();
            ea.text = textBox1.Text;
            ea.bCanel = true;
            this.onClose.Invoke(this, ea);
            this.Close();
        }
    }
    public class MyBoxEvent : EventArgs {
        public string text;
        public bool bCanel;
    }
}
