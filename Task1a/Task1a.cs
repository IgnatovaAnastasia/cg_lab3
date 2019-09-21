using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cg_lab3.Task1a
{
    public partial class Task1a : Form
    {
        private bool isNowDrawing = false; //true, если зажата мышка и идет рисование
        private Pen drawingPen; //перо текущего рисования
        private Graphics drawingGraphic;
        private Point oldMousePos;
        public Task1a()
        {
            InitializeComponent();
            currentColorPicture.BackColor = Color.Black; //изначально выбран черный цвет для рисования
            workArea.Image = new Bitmap(workArea.Width, workArea.Height); //область для рисования изначально белая
            drawingGraphic = Graphics.FromImage(workArea.Image);
            drawingGraphic.FillRectangle(new SolidBrush(Color.White), 0, 0, workArea.Width, workArea.Height);
        }

        private void ChangeCurrentColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                currentColorPicture.BackColor = dialog.Color;
        }

        private void WorkArea_MouseDown(object sender, MouseEventArgs e)
        {
            drawingPen = new Pen(currentColorPicture.BackColor, 1);
            if (enableDrawing.Checked) //если включен режим рисования, начинаем рисование
            {
                isNowDrawing = true;
                
                oldMousePos = e.Location;
                drawingGraphic.DrawEllipse(drawingPen, e.X, e.Y, drawingPen.Width, drawingPen.Width);
                workArea.Invalidate(); //обновляем картинку
            }
            else //иначе выполяем заливку
            {
                Bitmap workAreaImage = workArea.Image as Bitmap;
                //получаем дескриптор Graphics для быстрого получения цветов
                Color oldColor = workAreaImage.GetPixel(e.X, e.Y);
                Color newColor = currentColorPicture.BackColor;

                //обходим массив и заливаем
                Stack<Point> points = new Stack<Point>();
                points.Push(e.Location);

                while (points.Count > 0)
                {
                    Point currentPoint = points.Pop();
                    //проверяем, что не вышли за границы картинки
                    if (currentPoint.X < 0 || currentPoint.Y >= workArea.Width)
                        continue;

                    Color currentPointColor = workAreaImage.GetPixel(currentPoint.X, currentPoint.Y);
                    if (currentPointColor == oldColor && currentPointColor != newColor)
                    {
                        //определяем границы линии для заливки
                        int leftX = currentPoint.X, rightX = currentPoint.X;
                        while (true)
                        {
                            if (leftX == 0) break;
                            Color tColor = workAreaImage.GetPixel(leftX - 1, currentPoint.Y);
                            if (tColor == oldColor && tColor != newColor) leftX--;
                            else break;
                        }
                        while (true)
                        {
                            if (rightX == workArea.Width - 1) break;
                            Color tColor = workAreaImage.GetPixel(rightX + 1, currentPoint.Y);
                            if (tColor == oldColor && tColor != newColor) rightX++;
                            else break;
                        }

                        if (leftX == rightX) continue;
                        drawingGraphic.DrawLine(drawingPen, leftX, currentPoint.Y, rightX, currentPoint.Y);

                        //добавляем в очередь пиксели выше и ниже текущей линии
                        if (currentPoint.Y > 0)
                            for (int x = leftX; x <= rightX; x++)
                            {
                                Point newPoint = new Point(x, currentPoint.Y - 1);
                                Color newPointColor = workAreaImage.GetPixel(x, newPoint.Y);
                                if (newPointColor == oldColor && newPointColor != newColor) points.Push(newPoint);
                            }
                        if (currentPoint.Y < workArea.Height - 1)
                            for (int x = leftX; x <= rightX; x++)
                            {
                                Point newPoint = new Point(x, currentPoint.Y + 1);
                                Color newPointColor = workAreaImage.GetPixel(x, newPoint.Y);
                                if (newPointColor == oldColor && newPointColor != newColor) points.Push(newPoint);
                            }
                    }
                }
                workArea.Invalidate(); //обновляем картинку
            }
        }

        private void WorkArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (!enableDrawing.Checked || !isNowDrawing) return; //если мы не в режиме рисования, ничего не делаем

            drawingGraphic.DrawLine(drawingPen, oldMousePos, e.Location);
            oldMousePos = e.Location;
            workArea.Invalidate(); //обновляем картинку
        }

        private void WorkArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (!enableDrawing.Checked || !isNowDrawing) return; //если мы не в режиме рисования, ничего не делаем
            isNowDrawing = false;
            drawingPen.Dispose();
        }

        private void EnableFilling_CheckedChanged(object sender, EventArgs e)
        {
            isNowDrawing = false;
        }

    }
}
