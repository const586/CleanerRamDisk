using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BotInterface.Utils.Forms
{
    public partial class InputBoxForm : Form
    {
        public event TextComparrer Comparator;
        public event TextInputCallbacker ValueCallback;
        public InputBoxForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void SetLabel(string text)
        {
            label1.Text = text;
        }

        public void SetDefaultValue(string text)
        {
            textBox1.Text = text;
        }

        private void InputBoxForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Comparator != null && !Comparator(e.KeyChar)))
            {
                if (e.KeyChar != 8)
                {
                    e.Handled = false;
                    e.KeyChar = (char) 0;
                    return;
                }
            }
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ValueCallback != null)
                label2.Text = ValueCallback(textBox1.Text);
        }

        public string GetValue() => textBox1.Text;
    }
}
