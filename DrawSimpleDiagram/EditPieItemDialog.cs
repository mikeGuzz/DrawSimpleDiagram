using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawSimpleDiagram
{
    public partial class EditPieItemDialog : Form
    {
        private event Predicate<string>? nameCheck;

        public event Predicate<string> NameCheck
        {
            add => nameCheck += value;
            remove => nameCheck -= value;
        }

        public EditPieItemDialog()
        {
            InitializeComponent();
        }

        public EditPieItemDialog(PieItem item)
        {
            InitializeComponent();

            textBox1.Text = item.Name;
            pictureBox1.BackColor = item.Color;
            numericUpDown1.Value = (decimal)item.Count;
        }

        public PieItem GetValue()
        {
            return new PieItem(textBox1.Text, (double)numericUpDown1.Value, pictureBox1.BackColor);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("You must fill in all fields", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (nameCheck != null && !nameCheck.Invoke(textBox1.Text))
                MessageBox.Show($"Name '{textBox1.Text}' is invalid", ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void pickColor_button_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            dialog.Color = pictureBox1.BackColor;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = dialog.Color;
            }
        }
    }
}
