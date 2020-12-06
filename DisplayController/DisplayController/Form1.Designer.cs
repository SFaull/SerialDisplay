namespace DisplayController
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
            this.btnSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnIDN = new System.Windows.Forms.Button();
            this.btnGetBuff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(713, 415);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Start";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
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
            this.btnRead.Location = new System.Drawing.Point(692, 24);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnIDN
            // 
            this.btnIDN.Location = new System.Drawing.Point(585, 24);
            this.btnIDN.Name = "btnIDN";
            this.btnIDN.Size = new System.Drawing.Size(75, 23);
            this.btnIDN.TabIndex = 3;
            this.btnIDN.Text = "IDN";
            this.btnIDN.UseVisualStyleBackColor = true;
            this.btnIDN.Click += new System.EventHandler(this.btnIDN_Click);
            // 
            // btnGetBuff
            // 
            this.btnGetBuff.Location = new System.Drawing.Point(495, 24);
            this.btnGetBuff.Name = "btnGetBuff";
            this.btnGetBuff.Size = new System.Drawing.Size(75, 23);
            this.btnGetBuff.TabIndex = 4;
            this.btnGetBuff.Text = "Get Buffer";
            this.btnGetBuff.UseVisualStyleBackColor = true;
            this.btnGetBuff.Click += new System.EventHandler(this.btnGetBuff_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGetBuff);
            this.Controls.Add(this.btnIDN);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnIDN;
        private System.Windows.Forms.Button btnGetBuff;
    }
}

