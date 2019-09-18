namespace cg_lab3
{
    partial class Loader
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Task1a = new System.Windows.Forms.Button();
            this.Task1b = new System.Windows.Forms.Button();
            this.Task2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Task1a
            // 
            this.Task1a.Location = new System.Drawing.Point(12, 12);
            this.Task1a.Name = "Task1a";
            this.Task1a.Size = new System.Drawing.Size(215, 67);
            this.Task1a.TabIndex = 0;
            this.Task1a.Text = "Task1a";
            this.Task1a.UseVisualStyleBackColor = true;
            this.Task1a.Click += new System.EventHandler(this.Task1aClick);
            // 
            // Task1b
            // 
            this.Task1b.Location = new System.Drawing.Point(12, 101);
            this.Task1b.Name = "Task1b";
            this.Task1b.Size = new System.Drawing.Size(215, 67);
            this.Task1b.TabIndex = 1;
            this.Task1b.Text = "Task1b";
            this.Task1b.UseVisualStyleBackColor = true;
            this.Task1b.Click += new System.EventHandler(this.Task1bClick);
            // 
            // Task2
            // 
            this.Task2.Location = new System.Drawing.Point(12, 201);
            this.Task2.Name = "Task2";
            this.Task2.Size = new System.Drawing.Size(215, 67);
            this.Task2.TabIndex = 2;
            this.Task2.Text = "Task2";
            this.Task2.UseVisualStyleBackColor = true;
            this.Task2.Click += new System.EventHandler(this.Task2Click);
            // 
            // Loader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 296);
            this.Controls.Add(this.Task2);
            this.Controls.Add(this.Task1b);
            this.Controls.Add(this.Task1a);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Loader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Task1a;
        private System.Windows.Forms.Button Task1b;
        private System.Windows.Forms.Button Task2;
    }
}

