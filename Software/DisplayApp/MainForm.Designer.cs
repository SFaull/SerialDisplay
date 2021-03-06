﻿namespace DisplayApp
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
            this.btnIconMode = new System.Windows.Forms.Button();
            this.btnSetRotation = new System.Windows.Forms.Button();
            this.pbTileView = new System.Windows.Forms.PictureBox();
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.gbDisplayPreview = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.gbDiagnostics = new System.Windows.Forms.GroupBox();
            this.btnScreenStream = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTileView)).BeginInit();
            this.gbMain.SuspendLayout();
            this.gbDisplayPreview.SuspendLayout();
            this.gbControls.SuspendLayout();
            this.gbDiagnostics.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(179, 29);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnIDN
            // 
            this.btnIDN.Location = new System.Drawing.Point(98, 29);
            this.btnIDN.Name = "btnIDN";
            this.btnIDN.Size = new System.Drawing.Size(75, 23);
            this.btnIDN.TabIndex = 3;
            this.btnIDN.Text = "IDN";
            this.btnIDN.UseVisualStyleBackColor = true;
            this.btnIDN.Click += new System.EventHandler(this.btnIDN_Click);
            // 
            // btnGetBuff
            // 
            this.btnGetBuff.Location = new System.Drawing.Point(17, 29);
            this.btnGetBuff.Name = "btnGetBuff";
            this.btnGetBuff.Size = new System.Drawing.Size(75, 23);
            this.btnGetBuff.TabIndex = 4;
            this.btnGetBuff.Text = "Get Buffer";
            this.btnGetBuff.UseVisualStyleBackColor = true;
            this.btnGetBuff.Click += new System.EventHandler(this.btnGetBuff_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(6, 19);
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
            this.pbPreview.Location = new System.Drawing.Point(14, 34);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(240, 240);
            this.pbPreview.TabIndex = 6;
            this.pbPreview.TabStop = false;
            // 
            // btnIconMode
            // 
            this.btnIconMode.Location = new System.Drawing.Point(87, 19);
            this.btnIconMode.Name = "btnIconMode";
            this.btnIconMode.Size = new System.Drawing.Size(75, 23);
            this.btnIconMode.TabIndex = 9;
            this.btnIconMode.Text = "Icon Mode";
            this.btnIconMode.UseVisualStyleBackColor = true;
            this.btnIconMode.Click += new System.EventHandler(this.btnIconMode_Click);
            // 
            // btnSetRotation
            // 
            this.btnSetRotation.Location = new System.Drawing.Point(260, 29);
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
            this.pbTileView.Location = new System.Drawing.Point(260, 34);
            this.pbTileView.Name = "pbTileView";
            this.pbTileView.Size = new System.Drawing.Size(240, 240);
            this.pbTileView.TabIndex = 11;
            this.pbTileView.TabStop = false;
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.gbDisplayPreview);
            this.gbMain.Controls.Add(this.gbControls);
            this.gbMain.Controls.Add(this.gbDiagnostics);
            this.gbMain.Location = new System.Drawing.Point(12, 41);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(557, 461);
            this.gbMain.TabIndex = 12;
            this.gbMain.TabStop = false;
            this.gbMain.Visible = false;
            // 
            // gbDisplayPreview
            // 
            this.gbDisplayPreview.Controls.Add(this.label2);
            this.gbDisplayPreview.Controls.Add(this.label1);
            this.gbDisplayPreview.Controls.Add(this.pbPreview);
            this.gbDisplayPreview.Controls.Add(this.pbTileView);
            this.gbDisplayPreview.Location = new System.Drawing.Point(20, 78);
            this.gbDisplayPreview.Name = "gbDisplayPreview";
            this.gbDisplayPreview.Size = new System.Drawing.Size(520, 292);
            this.gbDisplayPreview.TabIndex = 14;
            this.gbDisplayPreview.TabStop = false;
            this.gbDisplayPreview.Text = "Preview";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Transferred:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Frame:";
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.btnScreenStream);
            this.gbControls.Controls.Add(this.btnLoadImage);
            this.gbControls.Controls.Add(this.btnIconMode);
            this.gbControls.Location = new System.Drawing.Point(20, 19);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(520, 53);
            this.gbControls.TabIndex = 13;
            this.gbControls.TabStop = false;
            this.gbControls.Text = "Controls";
            // 
            // gbDiagnostics
            // 
            this.gbDiagnostics.Controls.Add(this.btnGetBuff);
            this.gbDiagnostics.Controls.Add(this.btnIDN);
            this.gbDiagnostics.Controls.Add(this.btnSetRotation);
            this.gbDiagnostics.Controls.Add(this.btnRead);
            this.gbDiagnostics.Enabled = false;
            this.gbDiagnostics.Location = new System.Drawing.Point(20, 376);
            this.gbDiagnostics.Name = "gbDiagnostics";
            this.gbDiagnostics.Size = new System.Drawing.Size(520, 67);
            this.gbDiagnostics.TabIndex = 12;
            this.gbDiagnostics.TabStop = false;
            this.gbDiagnostics.Text = "Diagnostics";
            // 
            // btnScreenStream
            // 
            this.btnScreenStream.Location = new System.Drawing.Point(168, 19);
            this.btnScreenStream.Name = "btnScreenStream";
            this.btnScreenStream.Size = new System.Drawing.Size(122, 23);
            this.btnScreenStream.TabIndex = 10;
            this.btnScreenStream.Text = "Screen Stream Mode";
            this.btnScreenStream.UseVisualStyleBackColor = true;
            this.btnScreenStream.Click += new System.EventHandler(this.btnScreenStream_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 510);
            this.Controls.Add(this.gbMain);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainForm";
            this.Text = "Serial Display App";
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTileView)).EndInit();
            this.gbMain.ResumeLayout(false);
            this.gbDisplayPreview.ResumeLayout(false);
            this.gbDisplayPreview.PerformLayout();
            this.gbControls.ResumeLayout(false);
            this.gbDiagnostics.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnIDN;
        private System.Windows.Forms.Button btnGetBuff;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Button btnIconMode;
        private System.Windows.Forms.Button btnSetRotation;
        private System.Windows.Forms.PictureBox pbTileView;
        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.GroupBox gbDisplayPreview;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.GroupBox gbDiagnostics;
        private System.Windows.Forms.Button btnScreenStream;
    }
}

