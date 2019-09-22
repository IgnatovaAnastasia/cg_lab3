using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cg_lab3.Task1b
{
    public partial class Task1b : Form
    {
        Image imageToFill;
        string selectedFileName;
        Graphics g;
        Point from, start_fill, to;
        bool drawing = true;
        Bitmap fillingImage, bitmapToFill;
        List<Point> border;

        public Task1b()
        {
            InitializeComponent();
            imageToDrawBox.Image = new Bitmap(imageToDrawBox.Width, imageToDrawBox.Height);
            g = Graphics.FromImage(imageToDrawBox.Image);
            g.Clear(Color.White);
            border = new List<Point>();
        }

        private void Task1b_Load(object sender, EventArgs e)
        {

        }

        private void chooseFileButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = dialog.FileName;
            }
            else
            {
                selectedFileName = null;
            }
            dialog.Dispose();
            ShowSourceImage();
            imageToFillBox.Visible = true;
            actionGroupBox.Visible = true;
        }

        void ShowSourceImage()
        {
            if (selectedFileName != null)
            {
                imageToFill = Image.FromFile(selectedFileName);
                imageToFillBox.Image = imageToFill;
            }
            else
            {
                imageToFillBox.Image = null;
            }
        }

        private void fillRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void drawRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (drawRadioButton.Checked)
            {
                drawing = true;
                imageToDrawBox.Image = new Bitmap(imageToDrawBox.Width, imageToDrawBox.Height);
                g = Graphics.FromImage(imageToDrawBox.Image);
                g.Clear(Color.White);
                border = new List<Point>();
                imageToDrawBox.Refresh();
            }
        }

        void FillWithImage(int x, int y) // точка щелчка мышки
        {
            int step_x_right = 0, step_x_left = -1;
            bool stop_right = false,  stop_left = false;
            while ((!stop_right || !stop_left) && x > 0 && x < fillingImage.Width && y > 0 && y < fillingImage.Height)
            {
                if ((x + step_x_right) < fillingImage.Width && !border.Contains(new Point(x + step_x_right, y)) 
                    && !border.Contains(new Point(x + step_x_right, y - 1)) && !border.Contains(new Point(x + step_x_right, y + 1))
                    && fillingImage.GetPixel(x + step_x_right, y) == Color.FromArgb(255, 255, 255, 255))
                {
                    fillingImage.SetPixel(x + step_x_right, y, bitmapToFill.GetPixel((((x + step_x_right - start_fill.X) % bitmapToFill.Width) + bitmapToFill.Width) % bitmapToFill.Width, ((((y - start_fill.Y) % bitmapToFill.Height)) + bitmapToFill.Height) % bitmapToFill.Height));
                    //fillingImage.SetPixel(x + step_x_right, y, Color.Aquamarine);
                    step_x_right++;
                }
                else
                    stop_right = true;
                if ((x + step_x_left) > 0 && !border.Contains(new Point(x + step_x_left, y)) 
                    && !border.Contains(new Point(x + step_x_left, y - 1)) && !border.Contains(new Point(x + step_x_left, y + 1))
                    && fillingImage.GetPixel(x + step_x_left, y) == Color.FromArgb(255, 255, 255, 255)) // линия по х влево
                {
                    fillingImage.SetPixel(x + step_x_left, y, bitmapToFill.GetPixel((((x + step_x_left - start_fill.X) % bitmapToFill.Width) + bitmapToFill.Width) % bitmapToFill.Width, ((((y - start_fill.Y) % bitmapToFill.Height)) + bitmapToFill.Height) % bitmapToFill.Height));
                    //fillingImage.SetPixel(x + step_x_left, y, Color.Aquamarine);
                    step_x_left--;
                }
                else
                    stop_left = true;
            }
            imageToDrawBox.Image = fillingImage;
                for (int i = step_x_left + 3; i < step_x_right; i++)
                {
                    FillWithImage(x + i, y - 1);
                FillWithImage(x + i, y + 1);
                }
        }
        

        private void imageToDrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && drawing)
            {
                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
                g.DrawLine(pen, from, e.Location);
                imageToDrawBox.Refresh();
                from = e.Location;
            }
        }

        private void imageToDrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawing || drawRadioButton.Checked)
            {
                from = e.Location;
                border.Add(from);
                to = e.Location;
                drawing = true;
            }
        }

        private void imageToDrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;
                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));

                g.DrawLine(pen, from, to);
                imageToDrawBox.Refresh();
                fillingImage = new Bitmap(imageToDrawBox.Image);
                makeBorder(from);
            }
        }

        private void imageToDrawBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (fillRadioButton.Checked)
            {
                fillingImage = new Bitmap(imageToDrawBox.Image);
                bitmapToFill = new Bitmap(imageToFill);
                start_fill = e.Location;
                FillWithImage(e.X, e.Y);
            }
        }

        void makeBorder(Point possibleBorder)
        {
            Point temp = new Point(possibleBorder.X, possibleBorder.Y);
            if (temp.Y - 1 > 0
                && fillingImage.GetPixel(temp.X, temp.Y - 1) == Color.FromArgb(255, 0, 0, 0))
            {
                 temp.Y--;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.Y - 1 > 0 && temp.X + 1 < fillingImage.Width 
                && fillingImage.GetPixel(temp.X + 1, temp.Y - 1) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.Y--;
                temp.X++;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.X + 1 < fillingImage.Width
                && fillingImage.GetPixel(temp.X + 1, temp.Y) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.X++;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.X + 1 < fillingImage.Width && temp.Y + 1 < fillingImage.Height
                && fillingImage.GetPixel(temp.X + 1, temp.Y + 1) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.Y++;
                temp.X++;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.Y + 1 < fillingImage.Height
                && fillingImage.GetPixel(temp.X, temp.Y + 1) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.Y++;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.X - 1 > 0 && temp.Y + 1 < fillingImage.Height
                && fillingImage.GetPixel(temp.X - 1, temp.Y + 1) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.Y++;
                temp.X--;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.X - 1 > 0 
                && fillingImage.GetPixel(temp.X - 1, temp.Y) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.X--;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
            if (temp.X - 1 > 0 && temp.Y - 1 > 0
                && fillingImage.GetPixel(temp.X - 1, temp.Y - 1) == Color.FromArgb(255, 0, 0, 0))
            {
                temp.Y--;
                temp.X--;
                if (!border.Contains(temp))
                {
                    border.Add(temp);
                    makeBorder(temp);
                }
            }
        }
    }
}
