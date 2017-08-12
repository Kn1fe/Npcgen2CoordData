namespace Npcgen2CoordData
{
    partial class Form1
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
            this.load_coord = new System.Windows.Forms.Button();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.save_coord = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            this.import = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // load_coord
            // 
            this.load_coord.Location = new System.Drawing.Point(12, 12);
            this.load_coord.Name = "load_coord";
            this.load_coord.Size = new System.Drawing.Size(157, 23);
            this.load_coord.TabIndex = 0;
            this.load_coord.Text = "Открыть coord_data.txt";
            this.load_coord.UseVisualStyleBackColor = true;
            this.load_coord.Click += new System.EventHandler(this.load_coord_Click);
            // 
            // Progress
            // 
            this.Progress.Location = new System.Drawing.Point(12, 83);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(320, 23);
            this.Progress.TabIndex = 1;
            // 
            // save_coord
            // 
            this.save_coord.Location = new System.Drawing.Point(175, 12);
            this.save_coord.Name = "save_coord";
            this.save_coord.Size = new System.Drawing.Size(157, 23);
            this.save_coord.TabIndex = 2;
            this.save_coord.Text = "Сохранить coord_data.txt";
            this.save_coord.UseVisualStyleBackColor = true;
            this.save_coord.Click += new System.EventHandler(this.save_coord_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 67);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(38, 13);
            this.Status.TabIndex = 3;
            this.Status.Text = "Ждем";
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(12, 41);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(320, 23);
            this.import.TabIndex = 4;
            this.import.Text = "Импортировать npcgen";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 115);
            this.Controls.Add(this.import);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.save_coord);
            this.Controls.Add(this.Progress);
            this.Controls.Add(this.load_coord);
            this.Name = "Form1";
            this.Text = "Npcgen to coord_data.txt importer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load_coord;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Button save_coord;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Button import;
    }
}

