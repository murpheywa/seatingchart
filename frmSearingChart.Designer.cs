namespace SeatingChart
{
    partial class frmSearingChart
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearingChart));
            this.pb00 = new System.Windows.Forms.PictureBox();
            this.pbContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pbCut = new System.Windows.Forms.ToolStripMenuItem();
            this.pbCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.pbPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pbShow = new System.Windows.Forms.ToolStripMenuItem();
            this.pbHide = new System.Windows.Forms.ToolStripMenuItem();
            this.tb00 = new System.Windows.Forms.TextBox();
            this.txtLab01 = new System.Windows.Forms.TextBox();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFilePrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCleanupNames = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewFullSize = new System.Windows.Forms.ToolStripMenuItem();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.mnuCleanupNamesFirstLast = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCleanupNamesLastFirst = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pb00)).BeginInit();
            this.pbContextMenu.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb00
            // 
            this.pb00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb00.ContextMenuStrip = this.pbContextMenu;
            this.pb00.Location = new System.Drawing.Point(0, 27);
            this.pb00.Name = "pb00";
            this.pb00.Size = new System.Drawing.Size(53, 69);
            this.pb00.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb00.TabIndex = 0;
            this.pb00.TabStop = false;
            this.pb00.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb00_MouseDown);
            this.pb00.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb00_MouseMove);
            this.pb00.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb00_MouseUp);
            // 
            // pbContextMenu
            // 
            this.pbContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbCut,
            this.pbCopy,
            this.pbPaste,
            this.toolStripMenuItem2,
            this.pbShow,
            this.pbHide});
            this.pbContextMenu.Name = "pbPaste";
            this.pbContextMenu.Size = new System.Drawing.Size(104, 120);
            this.pbContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.pbContextMenu_Opening);
            // 
            // pbCut
            // 
            this.pbCut.Name = "pbCut";
            this.pbCut.Size = new System.Drawing.Size(103, 22);
            this.pbCut.Text = "Cut";
            this.pbCut.Click += new System.EventHandler(this.pbCut_Click);
            // 
            // pbCopy
            // 
            this.pbCopy.Name = "pbCopy";
            this.pbCopy.Size = new System.Drawing.Size(103, 22);
            this.pbCopy.Text = "Copy";
            this.pbCopy.Click += new System.EventHandler(this.pbCopy_Click);
            // 
            // pbPaste
            // 
            this.pbPaste.Name = "pbPaste";
            this.pbPaste.Size = new System.Drawing.Size(103, 22);
            this.pbPaste.Text = "Paste";
            this.pbPaste.Click += new System.EventHandler(this.pbPaste_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 6);
            // 
            // pbShow
            // 
            this.pbShow.Name = "pbShow";
            this.pbShow.Size = new System.Drawing.Size(103, 22);
            this.pbShow.Text = "Show";
            this.pbShow.Click += new System.EventHandler(this.pbShow_Click);
            // 
            // pbHide
            // 
            this.pbHide.Name = "pbHide";
            this.pbHide.Size = new System.Drawing.Size(103, 22);
            this.pbHide.Text = "Hide";
            this.pbHide.Click += new System.EventHandler(this.pbHide_Click);
            // 
            // tb00
            // 
            this.tb00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb00.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb00.Location = new System.Drawing.Point(0, 105);
            this.tb00.Multiline = true;
            this.tb00.Name = "tb00";
            this.tb00.Size = new System.Drawing.Size(53, 53);
            this.tb00.TabIndex = 0;
            this.tb00.Text = "A\r\nB\r\nC";
            this.tb00.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb00.TextChanged += new System.EventHandler(this.tb00_TextChanged);
            this.tb00.Leave += new System.EventHandler(this.tb00_Leave);
            // 
            // txtLab01
            // 
            this.txtLab01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLab01.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLab01.Location = new System.Drawing.Point(76, 595);
            this.txtLab01.Multiline = true;
            this.txtLab01.Name = "txtLab01";
            this.txtLab01.Size = new System.Drawing.Size(684, 102);
            this.txtLab01.TabIndex = 2;
            this.txtLab01.Text = "a\r\nb\r\nc";
            this.txtLab01.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuView});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(940, 24);
            this.mnuMain.TabIndex = 4;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.mnuFileClose,
            this.toolStripMenuItem3,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.mnuFileDelete,
            this.toolStripMenuItem4,
            this.mnuFilePrint,
            this.mnuFilePrintPreview,
            this.toolStripMenuItem1,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.Size = new System.Drawing.Size(152, 22);
            this.mnuFileNew.Text = "New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.Size = new System.Drawing.Size(152, 22);
            this.mnuFileOpen.Text = "Open";
            // 
            // mnuFileClose
            // 
            this.mnuFileClose.Name = "mnuFileClose";
            this.mnuFileClose.Size = new System.Drawing.Size(152, 22);
            this.mnuFileClose.Text = "Close";
            this.mnuFileClose.Click += new System.EventHandler(this.mnuFileClose_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSave.Text = "Save";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Name = "mnuFileSaveAs";
            this.mnuFileSaveAs.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSaveAs.Text = "Save As...";
            this.mnuFileSaveAs.Click += new System.EventHandler(this.mnuFileSaveAs_Click);
            // 
            // mnuFileDelete
            // 
            this.mnuFileDelete.Name = "mnuFileDelete";
            this.mnuFileDelete.Size = new System.Drawing.Size(152, 22);
            this.mnuFileDelete.Text = "Delete";
            this.mnuFileDelete.Click += new System.EventHandler(this.mnuFileDelete_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuFilePrint
            // 
            this.mnuFilePrint.Name = "mnuFilePrint";
            this.mnuFilePrint.Size = new System.Drawing.Size(152, 22);
            this.mnuFilePrint.Text = "Print...";
            this.mnuFilePrint.Click += new System.EventHandler(this.mnuFilePrint_Click);
            // 
            // mnuFilePrintPreview
            // 
            this.mnuFilePrintPreview.Name = "mnuFilePrintPreview";
            this.mnuFilePrintPreview.Size = new System.Drawing.Size(152, 22);
            this.mnuFilePrintPreview.Text = "Print Preview...";
            this.mnuFilePrintPreview.Click += new System.EventHandler(this.mnuFilePrintPreview_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(152, 22);
            this.mnuFileExit.Text = "Exit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCleanupNames});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "Edit";
            // 
            // mnuCleanupNames
            // 
            this.mnuCleanupNames.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCleanupNamesFirstLast,
            this.mnuCleanupNamesLastFirst});
            this.mnuCleanupNames.Name = "mnuCleanupNames";
            this.mnuCleanupNames.Size = new System.Drawing.Size(158, 22);
            this.mnuCleanupNames.Text = "Cleanup Names";
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewFullSize});
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(44, 20);
            this.mnuView.Text = "View";
            // 
            // mnuViewFullSize
            // 
            this.mnuViewFullSize.Name = "mnuViewFullSize";
            this.mnuViewFullSize.Size = new System.Drawing.Size(116, 22);
            this.mnuViewFullSize.Text = "Full Size";
            this.mnuViewFullSize.Click += new System.EventHandler(this.mnuViewFullSize_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            this.printDocument1.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.printDocument1_QueryPageSettings);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            this.printPreviewDialog1.Load += new System.EventHandler(this.printPreviewDialog1_Load);
            // 
            // mnuCleanupNamesFirstLast
            // 
            this.mnuCleanupNamesFirstLast.Name = "mnuCleanupNamesFirstLast";
            this.mnuCleanupNamesFirstLast.Size = new System.Drawing.Size(152, 22);
            this.mnuCleanupNamesFirstLast.Text = "First Last";
            this.mnuCleanupNamesFirstLast.Click += new System.EventHandler(this.mnuCleanupNamesFirstLast_Click);
            // 
            // mnuCleanupNamesLastFirst
            // 
            this.mnuCleanupNamesLastFirst.Name = "mnuCleanupNamesLastFirst";
            this.mnuCleanupNamesLastFirst.Size = new System.Drawing.Size(152, 22);
            this.mnuCleanupNamesLastFirst.Text = "Last, First";
            this.mnuCleanupNamesLastFirst.Click += new System.EventHandler(this.mnuCleanupNamesLastFirst_Click);
            // 
            // frmSearingChart
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(940, 700);
            this.Controls.Add(this.mnuMain);
            this.Controls.Add(this.txtLab01);
            this.Controls.Add(this.tb00);
            this.Controls.Add(this.pb00);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmSearingChart";
            this.Text = "Seating Chart";
            this.Activated += new System.EventHandler(this.frmSearingChart_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSearingChart_FormClosing);
            this.Load += new System.EventHandler(this.frmSearingChart_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmSearingChart_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmSearingChart_DragEnter);
            this.DragLeave += new System.EventHandler(this.frmSearingChart_DragLeave);
            this.Resize += new System.EventHandler(this.frmSearingChart_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pb00)).EndInit();
            this.pbContextMenu.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb00;
        private System.Windows.Forms.TextBox tb00;
        private System.Windows.Forms.TextBox txtLab01;
        private System.Windows.Forms.ContextMenuStrip pbContextMenu;
        private System.Windows.Forms.ToolStripMenuItem pbPaste;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNew;
        private System.Windows.Forms.ToolStripMenuItem mnuFileDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem pbCut;
        private System.Windows.Forms.ToolStripMenuItem mnuFileClose;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem pbCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pbShow;
        private System.Windows.Forms.ToolStripMenuItem pbHide;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuFilePrint;
        private System.Windows.Forms.ToolStripMenuItem mnuFilePrintPreview;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuViewFullSize;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuCleanupNames;
        private System.Windows.Forms.ToolStripMenuItem mnuCleanupNamesFirstLast;
        private System.Windows.Forms.ToolStripMenuItem mnuCleanupNamesLastFirst;
    }
}