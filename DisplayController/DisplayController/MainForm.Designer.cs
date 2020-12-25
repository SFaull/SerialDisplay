namespace DisplayController
{
    partial class MainForm
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnIDN = new System.Windows.Forms.Button();
            this.btnGetBuff = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.cbTimer = new System.Windows.Forms.CheckBox();
            this.btnIconMode = new System.Windows.Forms.Button();
            this.btnSetRotation = new System.Windows.Forms.Button();
            this.pbTileView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTileView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(60, 40);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(713, 24);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnIDN
            // 
            this.btnIDN.Location = new System.Drawing.Point(632, 24);
            this.btnIDN.Name = "btnIDN";
            this.btnIDN.Size = new System.Drawing.Size(75, 23);
            this.btnIDN.TabIndex = 3;
            this.btnIDN.Text = "IDN";
            this.btnIDN.UseVisualStyleBackColor = true;
            this.btnIDN.Click += new System.EventHandler(this.btnIDN_Click);
            // 
            // btnGetBuff
            // 
            this.btnGetBuff.Location = new System.Drawing.Point(551, 24);
            this.btnGetBuff.Name = "btnGetBuff";
            this.btnGetBuff.Size = new System.Drawing.Size(75, 23);
            this.btnGetBuff.TabIndex = 4;
            this.btnGetBuff.Text = "Get Buffer";
            this.btnGetBuff.UseVisualStyleBackColor = true;
            this.btnGetBuff.Click += new System.EventHandler(this.btnGetBuff_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(189, 40);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadImage.TabIndex = 5;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // pbPreview
            // 
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(86, 138);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(240, 240);
            this.pbPreview.TabIndex = 6;
            this.pbPreview.TabStop = false;
            // 
            // cbTimer
            // 
            this.cbTimer.AutoSize = true;
            this.cbTimer.Location = new System.Drawing.Point(285, 44);
            this.cbTimer.Name = "cbTimer";
            this.cbTimer.Size = new System.Drawing.Size(96, 17);
            this.cbTimer.TabIndex = 8;
            this.cbTimer.Text = "Screen Stream";
            this.cbTimer.UseVisualStyleBackColor = true;
            this.cbTimer.CheckedChanged += new System.EventHandler(this.cbTimer_CheckedChanged);
            // 
            // btnIconMode
            // 
            this.btnIconMode.Location = new System.Drawing.Point(189, 69);
            this.btnIconMode.Name = "btnIconMode";
            this.btnIconMode.Size = new System.Drawing.Size(75, 23);
            this.btnIconMode.TabIndex = 9;
            this.btnIconMode.Text = "Icon Mode";
            this.btnIconMode.UseVisualStyleBackColor = true;
            this.btnIconMode.Click += new System.EventHandler(this.btnIconMode_Click);
            // 
            // btnSetRotation
            // 
            this.btnSetRotation.Location = new System.Drawing.Point(713, 53);
            this.btnSetRotation.Name = "btnSetRotation";
            this.btnSetRotation.Size = new System.Drawing.Size(75, 23);
            this.btnSetRotation.TabIndex = 10;
            this.btnSetRotation.Text = "Rotate";
            this.btnSetRotation.UseVisualStyleBackColor = true;
            this.btnSetRotation.Click += new System.EventHandler(this.btnSetRotation_Click);
            // 
            // pbTileView
            // 
            this.pbTileView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbTileView.Location = new System.Drawing.Point(366, 138);
            this.pbTileView.Name = "pbTileView";
            this.pbTileView.Size = new System.Drawing.Size(240, 240);
            this.pbTileView.TabIndex = 11;
            this.pbTileView.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbTileView);
            this.Controls.Add(this.btnSetRotation);
            this.Controls.Add(this.btnIconMode);
            this.Controls.Add(this.cbTimer);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.btnGetBuff);
            this.Controls.Add(this.btnIDN);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTileView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnIDN;
        private System.Windows.Forms.Button btnGetBuff;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.CheckBox cbTimer;
        private System.Windows.Forms.Button btnIconMode;
        private System.Windows.Forms.Button btnSetRotation;
        private System.Windows.Forms.PictureBox pbTileView;
    }
}

