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
        Point from, startFill, to;
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
            int stepXRight = 0, stepXLeft = -1;
            bool stopRight = false,  stopLeft = false;
            while ((!stopRight || !stopLeft) && x > 0 && x < fillingImage.Width && y > 0 && y < fillingImage.Height)
            {
                if ((x + stepXRight) < fillingImage.Width && !border.Contains(new Point(x + stepXRight, y)) 
                    && !border.Contains(new Point(x + stepXRight, y - 1)) && !border.Contains(new Point(x + stepXRight, y + 1))
                    && fillingImage.GetPixel(x + stepXRight, y) == Color.FromArgb(255, 255, 255, 255))
                {
                    fillingImage.SetPixel(x + stepXRight, y, bitmapToFill.GetPixel((((x + stepXRight - startFill.X) % bitmapToFill.Width) + bitmapToFill.Width) % bitmapToFill.Width, ((((y - startFill.Y) % bitmapToFill.Height)) + bitmapToFill.Height) % bitmapToFill.Height));
                    //fillingImage.SetPixel(x + step_x_right, y, Color.Aquamarine);
                    stepXRight++;
                }
                else
                    stopRight = true;
                if ((x + stepXLeft) > 0 && !border.Contains(new Point(x + stepXLeft, y)) 
                    && !border.Contains(new Point(x + stepXLeft, y - 1)) && !border.Contains(new Point(x + stepXLeft, y + 1))
                    && fillingImage.GetPixel(x + stepXLeft, y) == Color.FromArgb(255, 255, 255, 255)) // линия по х влево
                {
                    fillingImage.SetPixel(x + stepXLeft, y, bitmapToFill.GetPixel((((x + stepXLeft - startFill.X) % bitmapToFill.Width) + bitmapToFill.Width) % bitmapToFill.Width, ((((y - startFill.Y) % bitmapToFill.Height)) + bitmapToFill.Height) % bitmapToFill.Height));
                    //fillingImage.SetPixel(x + step_x_left, y, Color.Aquamarine);
                    stepXLeft--;
                }
                else
                    stopLeft = true;
            }
            imageToDrawBox.Image = fillingImage;
                for (int i = stepXLeft + 3; i < stepXRight; i++)
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
                startFill = e.Location;
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
