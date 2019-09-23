using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cg_lab3.Task1a
{
    public partial class Task1a : Form
    {
        private void drawPoint(Pen pen, int x, int y)
        {
            drawingGraphic.DrawEllipse(pen, x, y, pen.Width, pen.Width);
            workArea.Invalidate(); //обновляем картинку
        }

        private void fill(Pen pen, int startX, int startY)
        {
            Bitmap workAreaImage = workArea.Image as Bitmap;
            //получаем дескриптор Graphics для быстрого получения цветов
            Color oldColor = workAreaImage.GetPixel(startX, startY);
            Color newColor = currentColorPicture.BackColor;

            //обходим массив и заливаем
            Stack<Point> points = new Stack<Point>();
            points.Push(new Point(startX, startY));

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
                    drawingGraphic.DrawLine(pen, leftX, currentPoint.Y, rightX, currentPoint.Y);

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
}
