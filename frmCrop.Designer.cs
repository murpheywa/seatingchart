namespace SeatingChart
{
    partial class frmCrop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCrop));
            this.pbCrop = new System.Windows.Forms.PictureBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdRotateRight = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCrop)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCrop
            // 
            this.pbCrop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCrop.Location = new System.Drawing.Point(12, 12);
            this.pbCrop.Name = "pbCrop";
            this.pbCrop.Size = new System.Drawing.Size(464, 323);
            this.pbCrop.TabIndex = 0;
            this.pbCrop.TabStop = false;
            this.pbCrop.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCrop_Paint);
            this.pbCrop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseDown);
            this.pbCrop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseMove);
            this.pbCrop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCrop_MouseUp);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(272, 355);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(88, 21);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(388, 355);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(88, 21);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdRotateRight
            // 
            this.cmdRotateRight.Location = new System.Drawing.Point(135, 355);
            this.cmdRotateRight.Name = "cmdRotateRight";
            this.cmdRotateRight.Size = new System.Drawing.Size(112, 21);
            this.cmdRotateRight.TabIndex = 1;
            this.cmdRotateRight.Text = "Rotate Right 90%";
            this.cmdRotateRight.UseVisualStyleBackColor = true;
            this.cmdRotateRight.Click += new System.EventHandler(this.cmdRotateRight_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 355);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 21);
            this.button1.TabIndex = 0;
            this.button1.Text = "Rotate Left 90%";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCrop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 388);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdRotateRight);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.pbCrop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCrop";
            this.Text = "frmCrop";
            this.Load += new System.EventHandler(this.frmCrop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCrop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCrop;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdRotateRight;
        private System.Windows.Forms.Button button1;
    }
}