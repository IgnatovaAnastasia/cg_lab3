using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cg_lab3.Task2
{
    public partial class Task2 : Form
    {
        Bitmap ImageBitmap;
        Color baseColor;

        public Task2()
        {
            InitializeComponent();
        }
        private void ChoosePictureButtonClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = (Bitmap)Bitmap.FromFile(dialog.FileName);// в dialog.FileName находится путь к выбранному файлу
                var task2 = new Task2();
                this.Close();
                task2.Picture1.Image = image;
                task2.ImageBitmap = image;
                task2.ShowDialog();
            }
        }   
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var location = e.Location;//выбранные пикселы 
            //выделение границ области для заливки
            List<Point> points = new List<Point>();
            var start = ImageBitmap.GetPixel(location.X, location.Y);
            int nextY = ImageBitmap.Height - 1;
            var nextPixel = ImageBitmap.GetPixel(location.X, nextY);
            baseColor = start;

            while (start != nextPixel)
            {
                --nextY;
                nextPixel = ImageBitmap.GetPixel(location.X, nextY);
            }
            int dir = 8;//кол-во направлений движения
            int prevDir = dir;
            Point startPoint = new Point(location.X, nextY + 1);
            Point nextPoint = startPoint;
            while (true)
            {
                dir += dir - 2 < 0 ? 6 : -2;
                while (true)
                {
                    switch (dir)
                    {
                        case 7: { nextPoint = new Point(startPoint.X + 1, startPoint.Y + 1); break; }//вверх вправо
                        case 6: { nextPoint = new Point(startPoint.X, startPoint.Y + 1); break; }//вправо
                        case 5: { nextPoint = new Point(startPoint.X - 1, startPoint.Y + 1); break; }//вниз вправо
                        case 4: { nextPoint = new Point(startPoint.X - 1, startPoint.Y); break; }//вниз
                        case 3: { nextPoint = new Point(startPoint.X - 1, startPoint.Y - 1); break; }//влево вниз
                        case 2: { nextPoint = new Point(startPoint.X, startPoint.Y - 1); break; }//вправо
                        case 1: { nextPoint = new Point(startPoint.X + 1, startPoint.Y - 1); break; }//вправо вверх
                        default: { nextPoint = new Point(startPoint.X + 1, startPoint.Y); break; }//вверх
                    }
                    if (ImageBitmap.GetPixel(nextPoint.X, nextPoint.Y) == baseColor)
                    {
                        startPoint = nextPoint;
                        break;
                    }
                    else
                        dir += 1;
                    dir = dir % 8;
                }
                if (points.Any(pt => (pt.X == startPoint.X && pt.Y == startPoint.Y)))
                    break;
                else
                {
                    points.Add(startPoint);
                    prevDir = dir;
                }
            }
            //конец выделения области для заливки
            var checkPoints = points.OrderByDescending(p => p.Y).ToArray();
            //заполнение пикселов внетри выделенной области
            for (var i = 0; i < checkPoints.Length; i++)
            {
                var first = checkPoints[i];
                var second = first;
                var temp = checkPoints.Where(y => y.Y == first.Y).ToArray();
                foreach (var x in temp)
                    if (x.X > second.X)
                        second = x;
                if (first.Y == second.Y)
                {
                    if (second.X < first.X)
                    {
                        var a = first;
                        first = second;
                        second = a;
                    }
                    var prevPixel = baseColor;
                    for (var j = first.X; j < second.X - 1; j++)
                    {
                        var pixel = ImageBitmap.GetPixel(j, first.Y);
                        nextPixel = ImageBitmap.GetPixel(j + 1, first.Y);
                        if (pixel != baseColor && nextPixel == baseColor || pixel == baseColor && nextPixel != baseColor)
                            points.Add(new Point(j, first.Y));
                        if (pixel == baseColor)
                            continue;
                    }
                }
            }
            checkPoints = points.OrderByDescending(p => p.X).ToArray();
            for (var i = 0; i < checkPoints.Length; i++)
            {
                var first = checkPoints[i];
                var second = first;
                var temp = checkPoints.Where(y => y.X == first.X).ToArray();
                foreach (var x in temp)
                    if (x.Y > second.Y)
                        second = x;

                if (first.X == second.X)
                {
                    if (second.Y < first.Y)//swap second and second
                    {
                        var a = first;
                        first = second;
                        second = a;
                    }
                    var prevPixel = baseColor;

                    for (var j = first.Y; j < second.Y - 1; j++)
                    {
                        var pixel = ImageBitmap.GetPixel(first.X, j);
                        nextPixel = ImageBitmap.GetPixel(first.X, j + 1);
                        if (pixel != baseColor && nextPixel == baseColor || pixel == baseColor && nextPixel != baseColor)
                            points.Add(new Point(first.X, j));
                        if (pixel == baseColor)
                            continue;
                    }
                }
            }
            //конец заполнения пикселов 
            //рисование клона изображения с новой заливкой
            var imgeClone = (Bitmap)ImageBitmap.Clone();
            foreach (Point pt in points)//проходим по всем пикселам границы
                imgeClone.SetPixel(pt.X, pt.Y, Color.Red);
            Picture1.Image = imgeClone;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                var newColor = colorDialog1.Color;
                points = points.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
                for (int i = 0; i < points.Count - 1; ++i)
                {
                    if (points[i].Y == points[i + 1].Y)
                    {
                        var brush = new Pen(newColor, 1);
                        if (points[i].X < points[i + 1].X && imgeClone.GetPixel(points[i].X + 1, points[i].Y) == baseColor)
                            using (var graphics = Graphics.FromImage(imgeClone))
                                graphics.DrawLine(brush, points[i].X, points[i].Y, points[i + 1].X, points[i].Y);
                    }
                }
            }
            Picture1.Image = imgeClone;
        }
    }
}
