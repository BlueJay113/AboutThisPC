
namespace AboutThisPC
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.windowControls = new System.Windows.Forms.ToolStrip();
            this.versionImage = new System.Windows.Forms.PictureBox();
            this.versionTitle = new System.Windows.Forms.Label();
            this.verNum = new System.Windows.Forms.Label();
            this.model = new System.Windows.Forms.Label();
            this.processorName = new System.Windows.Forms.Label();
            this.memory = new System.Windows.Forms.Label();
            this.sdLabel = new System.Windows.Forms.Label();
            this.graphics = new System.Windows.Forms.Label();
            this.serial = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.versionImage)).BeginInit();
            this.SuspendLayout();
            // 
            // windowControls
            // 
            this.windowControls.BackColor = System.Drawing.Color.Black;
            this.windowControls.BackgroundImage = global::AboutThisPC.Properties.Resources.window;
            this.windowControls.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.windowControls.CanOverflow = false;
            this.windowControls.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.windowControls.ImageScalingSize = new System.Drawing.Size(12, 12);
            this.windowControls.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.windowControls.Location = new System.Drawing.Point(0, 0);
            this.windowControls.Name = "windowControls";
            this.windowControls.Padding = new System.Windows.Forms.Padding(0);
            this.windowControls.Size = new System.Drawing.Size(650, 31);
            this.windowControls.TabIndex = 0;
            this.windowControls.Text = "toolStrip1";
            this.windowControls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.windowControls_MouseDown);
            // 
            // versionImage
            // 
            this.versionImage.BackColor = System.Drawing.Color.Transparent;
            this.versionImage.InitialImage = null;
            this.versionImage.Location = new System.Drawing.Point(400, 100);
            this.versionImage.Name = "versionImage";
            this.versionImage.Size = new System.Drawing.Size(0, 0);
            this.versionImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.versionImage.TabIndex = 1;
            this.versionImage.TabStop = false;
            this.versionImage.WaitOnLoad = true;
            // 
            // versionTitle
            // 
            this.versionTitle.AutoSize = true;
            this.versionTitle.Font = new System.Drawing.Font("SF Pro Display", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionTitle.Location = new System.Drawing.Point(400, 100);
            this.versionTitle.Name = "versionTitle";
            this.versionTitle.Size = new System.Drawing.Size(0, 39);
            this.versionTitle.TabIndex = 2;
            // 
            // verNum
            // 
            this.verNum.AutoSize = true;
            this.verNum.Font = new System.Drawing.Font("SF Pro Display", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verNum.Location = new System.Drawing.Point(400, 100);
            this.verNum.Name = "verNum";
            this.verNum.Size = new System.Drawing.Size(0, 23);
            this.verNum.TabIndex = 3;
            // 
            // model
            // 
            this.model.AutoSize = true;
            this.model.Font = new System.Drawing.Font("SF Pro Display", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.model.Location = new System.Drawing.Point(400, 100);
            this.model.Name = "model";
            this.model.Size = new System.Drawing.Size(0, 23);
            this.model.TabIndex = 4;
            // 
            // processorName
            // 
            this.processorName.AutoSize = true;
            this.processorName.Font = new System.Drawing.Font("SF Pro Display", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processorName.Location = new System.Drawing.Point(400, 223);
            this.processorName.Name = "processorName";
            this.processorName.Size = new System.Drawing.Size(0, 23);
            this.processorName.TabIndex = 5;
            // 
            // memory
            // 
            this.memory.AutoSize = true;
            this.memory.Font = new System.Drawing.Font("SF Pro Display", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memory.Location = new System.Drawing.Point(288, 267);
            this.memory.Name = "memory";
            this.memory.Size = new System.Drawing.Size(0, 23);
            this.memory.TabIndex = 6;
            // 
            // sdLabel
            // 
            this.sdLabel.AutoSize = true;
            this.sdLabel.Location = new System.Drawing.Point(318, 323);
            this.sdLabel.Name = "sdLabel";
            this.sdLabel.Size = new System.Drawing.Size(0, 17);
            this.sdLabel.TabIndex = 7;
            // 
            // graphics
            // 
            this.graphics.AutoSize = true;
            this.graphics.Location = new System.Drawing.Point(301, 318);
            this.graphics.Name = "graphics";
            this.graphics.Size = new System.Drawing.Size(0, 17);
            this.graphics.TabIndex = 8;
            // 
            // serial
            // 
            this.serial.AutoSize = true;
            this.serial.Location = new System.Drawing.Point(295, 221);
            this.serial.Name = "serial";
            this.serial.Size = new System.Drawing.Size(0, 17);
            this.serial.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.BackgroundImage = global::AboutThisPC.Properties.Resources.border;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.serial);
            this.Controls.Add(this.graphics);
            this.Controls.Add(this.sdLabel);
            this.Controls.Add(this.memory);
            this.Controls.Add(this.processorName);
            this.Controls.Add(this.model);
            this.Controls.Add(this.verNum);
            this.Controls.Add(this.versionTitle);
            this.Controls.Add(this.versionImage);
            this.Controls.Add(this.windowControls);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "About this PC";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.versionImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip windowControls;
        private System.Windows.Forms.PictureBox versionImage;
        private System.Windows.Forms.Label versionTitle;
        private System.Windows.Forms.Label verNum;
        private System.Windows.Forms.Label model;
        private System.Windows.Forms.Label processorName;
        private System.Windows.Forms.Label memory;
        private System.Windows.Forms.Label sdLabel;
        private System.Windows.Forms.Label graphics;
        private System.Windows.Forms.Label serial;
    }
}

