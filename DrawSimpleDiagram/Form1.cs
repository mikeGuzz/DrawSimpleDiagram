using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Configuration;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace DrawSimpleDiagram
{
    public partial class Form1 : Form
    {
        private readonly Pen borderPen = new Pen(Color.Black, 2);
        private readonly SolidBrush fontBrush = new SolidBrush(Color.Black);

        public Form1()
        {
            InitializeComponent();

            this.MinimumSize = new Size(300, 150);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (listBox1.Items.Count == 0)
                return;

            var g = e.Graphics;

            double sum = listBox1.Items.Cast<PieItem>().Sum(i => i.Count);

            float startAngle = 0;
            var min = Math.Min(pictureBox1.Size.Width, pictureBox1.Size.Height);
            var r = new Rectangle(pictureBox1.Size.Width / 2 - min / 2 + 15, pictureBox1.Size.Height / 2 - min / 2 + 15, min - 30, min - 30);
            min -= 30;
            var midPoint = new Point(r.X + r.Width / 2, r.Y + r.Height / 2);

            var t = new Matrix();
            t.RotateAt(trackBar1.Value, midPoint, MatrixOrder.Append);
            g.Transform = t;

            foreach (var ob in listBox1.Items)
            {
                var item = (PieItem)ob;
                float sweepAngle = 360f * (float)(item.Count / sum);

                g.FillPie(new SolidBrush(item.Color), r, startAngle, sweepAngle);
                g.DrawPie(borderPen, r, startAngle, sweepAngle);

                startAngle += sweepAngle;
            }
            startAngle = 0;
            
            foreach (var ob in listBox1.Items)
            {
                var item = (PieItem)ob;
                float sweepAngle = 360f * (float)(item.Count / sum);

                var radius = min / 2;
                var height = 12;
                var offsetX = radius / 2 - (item.Name.Length * height) / 2;

                if (radius < (height * item.Name.Length + offsetX))
                {
                    offsetX = 0;
                    height = radius / item.Name.Length;
                }

                if(height > 0)
                {
                    var rotateAngle = startAngle + (sweepAngle / 2);
                    t.RotateAt(rotateAngle, midPoint);
                    g.Transform = t;

                    g.DrawString(item.Name, new Font("Consolas", height), fontBrush, new Point(midPoint.X + offsetX, midPoint.Y - height / 2));

                    t.RotateAt(-rotateAngle, midPoint);
                    g.Transform = t;
                }

                startAngle += sweepAngle;
            }

            label1.Text = $"Total: {sum}";

            t.Dispose();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            var dialog = new EditPieItemDialog();
            dialog.NameCheck += NameValidCheck;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(dialog.GetValue());
                pictureBox1.Invalidate();
            }
        }

        private void edit_button_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 1)
                return;
            var dialog = new EditPieItemDialog((PieItem)listBox1.SelectedItems[0]);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items[listBox1.SelectedIndex] = dialog.GetValue();
                pictureBox1.Invalidate();
            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            pictureBox1.Invalidate();
            label1.Text = $"Total: 0";
        }

        private bool NameValidCheck(string name)
        {
            return !listBox1.Items.Cast<PieItem>().Any(i => i.Name == name);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void remove_button_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 1)
                return;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            pictureBox1.Invalidate();
        }
    }
}