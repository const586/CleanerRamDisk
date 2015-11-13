using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BotInterface.Utils.Forms;

namespace BotInterface.Utils
{
    public delegate bool TextComparrer(char ch);

    public delegate string TextInputCallbacker(string text);

    public class InputBox
    {
        public static bool ShowDialog(string title, string message, ref string value, TextComparrer comporator,
            TextInputCallbacker description)
        {
            var form = new InputBoxForm {Text = title};
            form.ValueCallback += description;
            form.Comparator += comporator;
            form.SetLabel(message);
            form.SetDefaultValue(value);
            var res = form.ShowDialog();
            if (res == DialogResult.OK)
                value = form.GetValue();
            return res == DialogResult.OK;
        }
    }
}
