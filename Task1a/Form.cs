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
                drawPoint(drawingPen, e.X, e.Y);
            }
            else //иначе выполяем заливку
            {
                fill(drawingPen, e.X, e.Y);
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
