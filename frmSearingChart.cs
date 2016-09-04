using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CtrlCloneTst;
using System.IO;
using System.Diagnostics;
using SeatingChart.Properties;
using System.Text.RegularExpressions;

namespace SeatingChart
{
    public partial class frmSearingChart : Form
    {
        private string _sWorkspacePath = "";
        private SeatingChartItem _openItem = null;
        private Size _lastClientSize;
        private bool _bInResize = false;
        private bool _bInTextChanged = false;
        private int _smallSpaceCount = 8;
        private int _smallSpaceWidth = 2;
        private int _largeSpaceCount = 3;
        private int _largeSpaceWidth = 14;
        private int _rowSpaceHeight = 30;
        private int _pictureColumnCount = 12;
        private int _pictureRowCount = 3;
        private int _refPictureWidth = 74;
        private int _refPictureHeight = 96;
        private List<PictureBox> _pictureBoxList = new List<PictureBox>();
        private List<TextBox> _textBoxList = new List<TextBox>();
        private float _HRes = 0.0f;
        private float _VRes = 0.0f;
        private float _topBorderInches = 0.7f;
        private float __leftBorderInches = 0.7f;
        private int _formFrameWidth = 0;
        private int _formFrameHeight = 0;
        private Bitmap _printBitmap = null;
        private BorderStyle _textBoxBorderStyle = BorderStyle.None;
        private PictureBox _dragSrc = null;
        private Point _dragSrcLocation;
        private PictureBox _dragDst = null;
        private bool _bPreviewBeenhere = false;
        private Regex _countNewLineRegex = null;
        private Regex _countBlanksRegex = null;
        private int _iFullSizeWidth = 0;

        private void frmSearingChart_Load(object sender, EventArgs e)
        {
            _countNewLineRegex = new Regex(@"\n", RegexOptions.Compiled);
            _countBlanksRegex = new Regex(@" ", RegexOptions.Compiled);
            printDocument1.DefaultPageSettings.Landscape = true;
            System.Drawing.Printing.Margins margs = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            printDocument1.OriginAtMargins = false;
            printPreviewDialog1.Document = printDocument1;

            _textBoxBorderStyle = txtLab01.BorderStyle;

            Calibrate();
            _sWorkspacePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _sWorkspacePath = Path.Combine(_sWorkspacePath, "SeatingCharts");
            if ( ! Directory.Exists( _sWorkspacePath ))
            {
                try
                {
                    Directory.CreateDirectory(_sWorkspacePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't create app data: " + ex.Message);
                    Close();
                }
            }

            if (_pictureBoxList.Count == 0)
            {
                DoPopulate();
            }

            string sChartName = Settings.Default.OpenChart;
            string sChartPath = SeatingChartXmlPath(sChartName);
            if (File.Exists(sChartPath))
            {
                DoOpen(sChartPath);
            }
            else
            {
                _openItem = null;
            }
            UpdateUIForm();
            UpdateUIFileMenu();

            // set initial form size
            _iFullSizeWidth = DoZoom(1.0f);
        }

        private void Calibrate()
        {
            GetFormFrameSize();

            using (Graphics g = this.CreateGraphics())
            {
                _HRes = g.DpiX; // Horizontal Resolution
                _VRes = g.DpiY; // Vertical Resolution
            }
        }

        private void GetFormFrameSize()
        {
            _formFrameHeight = this.Height - ClientRectangle.Height;
            _formFrameWidth = this.Width - ClientRectangle.Width;
        }

        private Point ClientRectLocation
        {
            get
            {
                Point pt = new Point();
                pt.X = SystemInformation.Border3DSize.Width;
                pt.X = SystemInformation.BorderSize.Width;
                pt.X = SystemInformation.FrameBorderSize.Width;

                pt.Y = SystemInformation.CaptionHeight + pt.X;

                int iTestX = Width - ClientRectangle.Width;
                int iTestY = Height - ClientRectangle.Height;

                return pt;
            }
        }

        private int SmallSpacePixels
        {
            get { return _smallSpaceCount * _smallSpaceWidth; }
        }

        private int LargeSpacePixels
        {
            get { return _largeSpaceCount * _largeSpaceWidth; }
        }

        private int TopBorderPixels(int iFormClientHeight)
        {
            // what percent of 8.5" is the client height?
            float pct = (float)iFormClientHeight / (float)(_VRes * 8.5f);
            int pixels = (int)(_topBorderInches * _VRes * pct );

            return pixels;
        }

        private int LeftBorderPixels(int iFormClientWidth)
        {
            // what percent of 11.0" is the client height?
            float pct = (float)iFormClientWidth / (float)(_VRes * 11.0f);
            int pixels = (int)(__leftBorderInches * _HRes * pct);

            return pixels;
        }

        private int PicturesWidth(int iFormClientWidth)
        {
            int border = LeftBorderPixels(iFormClientWidth) * 2;
            return iFormClientWidth - border - SmallSpacePixels - LargeSpacePixels;
        }

        private int PictureWidth(int iFormClientWidth)
        {
            return PicturesWidth(iFormClientWidth) / _pictureColumnCount;
        }

        private int PictureHeight(int iFormClientWidth)
        {
            return (int)( (double)PictureWidth( iFormClientWidth ) * (double)_refPictureHeight / (double)_refPictureWidth );
        }

        private int PictureCount
        {
            get { return _pictureRowCount * _pictureColumnCount; }
        }

        int SelectedIndex
        {
            get
            {
                string sName = pbContextMenu.SourceControl.Name;
                int idx = Int32.Parse(sName.Substring(2));
                return idx;
            }
        }

        PictureBox SelectedPictureBox
        {
            get { return _pictureBoxList[SelectedIndex]; }
        }

        TextBox SelectedPictureLabel
        {
            get { return _textBoxList[SelectedIndex]; }
        }


        public frmSearingChart()
        {
            InitializeComponent();
        }

        void DeselectActiveText()
        {
            TextBox tb = this.ActiveControl as TextBox;
            if (tb != null)
            {
                tb.SelectionStart = 0;
                tb.SelectionLength = 0;
            }
        }

        private void DoPopulate()
        {
            tb00.Text = "";
            txtLab01.Text = "";

            _pictureBoxList.Add(pb00);
            _textBoxList.Add(tb00);

            int iCursor = pb00.Left + pb00.Width;

            for (int idx = 1; idx < _pictureColumnCount * _pictureRowCount; idx++)
            {
                PictureBox pb = ControlFactory.CloneCtrl(pb00) as PictureBox;
                Debug.Assert(pb != null);
                pb.MouseDown += new MouseEventHandler(pb00_MouseDown);
                pb.MouseUp += new MouseEventHandler(pb00_MouseUp);
                pb.MouseMove += new MouseEventHandler(pb00_MouseMove);

                pb.ContextMenuStrip = pbContextMenu;
                pb.Name = string.Format("pb{0:00}", idx);
                this.Controls.Add(pb);
                _pictureBoxList.Add(pb);

                TextBox tb = ControlFactory.CloneCtrl(tb00) as TextBox;
                Debug.Assert(tb != null);
                tb.Name = string.Format("tb{0:00}", idx);
                tb.TextChanged += new System.EventHandler(this.tb00_TextChanged);
                tb.Leave += new System.EventHandler(this.tb00_Leave);
                tb.TabIndex = 1;

                this.Controls.Add(tb);
                _textBoxList.Add(tb);
                pb.Show();
            }
        }

        private void frmSearingChart_Resize(object sender, EventArgs e)
        {
            DoResize();
        }

        private void DoResize()
        {
            if (_pictureBoxList.Count == 0)
            {
                DoPopulate();
            }

            if (!_bInResize)
            {
                _bInResize = true;
                DoResizeForm();
                _bInResize = false;
            }
        }

        private void DoResizeForm()
        {
            GetFormFrameSize();

            int iClientWidth = ClientRectangle.Width;
            int iClientHeight = ClientRectangle.Height;
            // Maintain aspect ratio of 11 x 8.5
            int iDeltaX = Math.Abs(iClientWidth - _lastClientSize.Width);
            int iDeltaY = Math.Abs(iClientHeight - _lastClientSize.Height);
            _lastClientSize = new Size( iClientWidth, iClientHeight );

            if (iDeltaX >= iDeltaY)
            {
                // solve for Y
                iClientHeight = (int)Math.Round((double)iClientWidth * 8.5 / 11.0);
                Height = iClientHeight + _formFrameHeight + mnuFile.Height;
            }
            else
            {
                // solve for x
                iClientHeight -= mnuFile.Height;
                iClientWidth = (int)Math.Round((double)iClientHeight * 11.0 / 8.5);
                Width = iClientWidth + _formFrameWidth;
            }

            DoResizeControls(ClientRectangle);
        }

        private void DoResizeControls( Rectangle clientRect )
        {
            int topBorderPixels = TopBorderPixels(clientRect.Height);
            int leftBorderPixels = LeftBorderPixels(clientRect.Width);

            txtLab01.Left = 0;
            txtLab01.Width = clientRect.Width;
            //txtLab01.Top = clientRect.Height - txtLab01.Height - topBorderPixels;
            // lab height is 1 inch, which is 1 / 8.5 of the form height
            float fTxtLabPctOfHeight = 1.0f / 8.5f;
            txtLab01.Height = (int)((float)(clientRect.Height - mnuMain.Height) * fTxtLabPctOfHeight);
            int iLabHeight = (int)(1.0f /*inch*/ * _VRes);
            txtLab01.Top = clientRect.Height - txtLab01.Height - topBorderPixels;

            int iTop = 0 + topBorderPixels + mnuMain.Height;
            int iPictureWidth = PictureWidth(clientRect.Width);
            int iPictureHeight = PictureHeight(clientRect.Width);

            int idx = 0;
            for (int rowIdx = 0; rowIdx < _pictureRowCount; rowIdx++)
            {
                int iCursor = 0 + LeftBorderPixels(clientRect.Width);
                for (int colIdx = 0; colIdx < _pictureColumnCount; colIdx++)
                {
                    PictureBox pb = _pictureBoxList[idx];
                    pb.Width = iPictureWidth;
                    pb.Height = iPictureHeight;
                    pb.Top = iTop;

                    TextBox tb = _textBoxList[idx++];
                    tb.Width = iPictureWidth;
                    tb.Top = iTop + iPictureHeight + 1;
                    if (colIdx == 0)
                    {
                        iCursor = 0 + leftBorderPixels;
                    }
                    else if (colIdx == 3 || colIdx == 6 || colIdx == 9)
                    {
                        iCursor += _largeSpaceWidth;
                    }
                    else
                    {
                        iCursor += _smallSpaceWidth;
                    }
                    pb.Left = iCursor;
                    tb.Left = iCursor;
                    iCursor += iPictureWidth;
                }

                iTop += iPictureHeight + tb00.Height + 1 + _rowSpaceHeight;
            }
        }

        public string PictureBoxTagText(PictureBox pb)
        {
            string sText =
                pb.Tag == null
                ? ""
                : pb.Tag.ToString();

            return sText;
        }

        private void DeleteImageFile(PictureBox pb)
        {
            string sImageLocation = PictureBoxTagText( pb );
            pb.Tag = null;
            //pb.ImageLocation = null;
            // pb.Image = null;
            SafeReleaseImage(pb);
        }

        private void DoImageCutCopy(bool bCut)
        {
            PictureBox pb = SelectedPictureBox;

            if (pb.Image != null)
            {
                Clipboard.SetImage(pb.Image);
                if (bCut)
                {
                    DeleteImageFile( pb );
                }
            }
        }

        private void NewImageFor(PictureBox pb, Image img)
        {
            try
            {
                string sNewPath = NewImagePath();
                if (File.Exists(sNewPath))
                {
                    File.Delete(sNewPath);
                }
                img.Save(sNewPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                //DeleteImageFile(pb);
                SafeReleaseImage(pb);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Tag = sNewPath;
                //pb.ImageLocation = sNewPath;
                //pb.Load();
                pb.Image = NonLockingOpen(sNewPath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void pbCut_Click(object sender, EventArgs e)
        {
            DoImageCutCopy(true);
        }

        private void pbCopy_Click(object sender, EventArgs e)
        {
            DoImageCutCopy(false);
        }

        private void pbPaste_Click(object sender, EventArgs e)
        {
            bool bOK = false;
            IDataObject oClipboard = Clipboard.GetDataObject() as IDataObject;

            if (oClipboard != null)
            {
                string[] sFormats = oClipboard.GetFormats(); // debug only

                if (oClipboard.GetDataPresent(DataFormats.Bitmap))
                {
                    Image image = (Image)oClipboard.GetData(DataFormats.Bitmap, true);

                    if (image == null)
                    {
                        MessageBox.Show("No image on clipboard");
                    }
                    else
                    {
                        bOK = CropAndUpdate(SelectedPictureBox, image);
                    }
                }
                else if (InternalClipboardFilePathPresent())
                {
                    string sPath = Clipboard.GetText();
                    bOK = CropAndUpdate(SelectedPictureBox, sPath);
                }
            }
        }

        private void pbShow_Click(object sender, EventArgs e)
        {
            SelectedPictureLabel.Visible = true;
            UpdateUIPictureBox(SelectedIndex);
        }

        private void pbHide_Click(object sender, EventArgs e)
        {
            SelectedPictureLabel.Visible = false;
            UpdateUIPictureBox(SelectedIndex);
        }

        private Image Crop(Image img)
        {
            // accept image width, crop height
            Rectangle rect = new Rectangle( 0, 0, img.Width, img.Height);
            int iCroppedY = rect.Width * _refPictureHeight / _refPictureWidth;
            // if the cropped value is smaller than the Y pixels we have, then crop it
            if (iCroppedY < rect.Height)
            {
                rect.Height = iCroppedY;
                return CropImage(img, rect);
            }
            else
            {
                return img;
            }
        }

        public static Image NonLockingOpen(string filename)
        {
            Image result;

            #region Save file to byte array

            long size = (new FileInfo(filename)).Length;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[size];
            try
            {
                fs.Read(data, 0, (int)size);
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

            #endregion

            #region Convert bytes to image

            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, (int)size);
            result = new Bitmap(ms);
            ms.Close();

            #endregion

            return result;
        }        

        private void UpdateUIPictureBox(int idx)
        {
            PictureBox pb = _pictureBoxList[idx];
            bool bShow = _textBoxList[idx].Visible;
            string sImageLocation = PictureBoxTagText(pb);
            if (bShow && !string.IsNullOrWhiteSpace(sImageLocation))
            {
                //pb.ImageLocation = sImageLocation;
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                //pb.Load();
                pb.Image = NonLockingOpen(sImageLocation);
            }
            else
            {
                //pb.ImageLocation = null;
                //pb.ImageLocation = _sEmptyImageLocation;
                //pb.Image = null;
                SafeReleaseImage(pb);
            }
        }

        private static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        private string SeatingChartXmlPath(string sName)
        {
            string sFolderPath = Path.Combine(_sWorkspacePath, sName);
            string sXmlName = sName + ".xml";
            string sXmlPath = Path.Combine(sFolderPath, sXmlName);

            return sXmlPath;
        }

        private void UpdateUIFileMenu()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_sWorkspacePath);
            List<string> items = new List<string>();
            foreach (DirectoryInfo iterDirInfo in dirInfo.GetDirectories())
            {
                if (File.Exists(SeatingChartXmlPath(iterDirInfo.Name)))
                {
                    items.Add(iterDirInfo.Name);
                }
            }
            SetRecentMenuItems(mnuFileOpen, items.ToArray(), new EventHandler(fileOpen_Click));

            mnuFileOpen.Enabled = (mnuFileOpen.DropDownItems.Count > 0);
            mnuFileDelete.Enabled = (_openItem != null);
            mnuFileSave.Enabled = (_openItem != null);
            mnuFileClose.Enabled = (_openItem != null);
            mnuFilePrint.Enabled = (_openItem != null);
            mnuFilePrintPreview.Enabled = (_openItem != null); 

            // TODO:
            pbCut.Enabled = (_openItem != null);
            pbCopy.Enabled = (_openItem != null);
            pbPaste.Enabled = (_openItem != null);

        }

        private void UpdateUIForm()
        {
            bool bIsOpen = (_openItem != null);

            ReleaseImages(bIsOpen);
            if (bIsOpen)
            {
                txtLab01.Text = _openItem.FooterText;
                txtLab01.SelectionStart = 0;

                foreach (DataRow row in _openItem.Rows)
                {
                    string sImagePath = row[SeatingChartItem.S.ImagePath].ToString();
                    sImagePath = ToAbsolutePath(sImagePath);
                    int idx = (int)row[SeatingChartItem.S.Index];
                    string sName = row[SeatingChartItem.S.Name].ToString();
                    // XML elides the <CR> from <CR><LF>
                    sName = Regex.Replace(sName, @"(\r){0,1}\n", "\r\n");
                    bool bShow = (bool)row[SeatingChartItem.S.Show];

                    TextBox tb = _textBoxList[idx];
                    tb.Visible = bShow;
                    tb.Text = sName;

                    PictureBox pb = _pictureBoxList[idx];
                    pb.Visible = true;
                    pb.Tag = sImagePath;
                    Debug.Print( "Image({0},{1})={2}",_openItem.Name, idx, sImagePath );
                    UpdateUIPictureBox(idx);
                }

                this.Text = "Seating Chart - " + _openItem.Name;
            }
            else
            {
                this.Text = "Seating Chart";
            }
        }

        private void SafeReleaseImage(PictureBox pb)
        {
            if (pb.Image != null)
            {
                pb.Image.Dispose();
                pb.Image = null;
            }
        }

        private void ReleaseImages(bool bIsOpen)
        {
            txtLab01.Visible = bIsOpen;
            for (int idx = 0; idx < PictureCount; idx++)
            {
                _pictureBoxList[idx].Visible = bIsOpen;
                _pictureBoxList[idx].Tag = null;
                //_pictureBoxList[idx].Image = null;
                //_pictureBoxList[idx].ImageLocation = null;
                SafeReleaseImage(_pictureBoxList[idx]);
                _textBoxList[idx].Clear();
                _textBoxList[idx].Visible = bIsOpen;
            }
        }

        private void SetRecentMenuItems(ToolStripMenuItem parentItem, string[] sItems, EventHandler handler)
        {
            AddRecentMenuItems(parentItem, sItems, handler, true);
        }

        private void AddRecentMenuItems(ToolStripMenuItem parentItem, string[] sItems, EventHandler handler, bool bClearList)
        {
            if (bClearList)
            {
                parentItem.DropDownItems.Clear();
            }

            int count = parentItem.DropDownItems.Count;

            ToolStripItem[] items = new ToolStripItem[sItems.Length];
            for (int idx = 0; idx < sItems.Length; idx++)
            {
                ToolStripItem item = null;
                string sText = sItems[idx];
                if (sText == "-")
                {
                    item = new ToolStripSeparator();
                }
                else
                {
                    item = new ToolStripMenuItem();
                    item.Name = "Recent" + (idx + count).ToString();
                    item.Text = Elide(sText);
                    item.Tag = sText;
                    item.Click += handler;
                }
                item.Size = new System.Drawing.Size(161, 22);
                items[idx] = item;
            }
            parentItem.DropDownItems.AddRange(items);
        }

        private string Elide(string sPath)
        {
            const int maxLen = 60;
            const char BS = '\\';

            string sRet = sPath;
            if (sRet.Length > maxLen)
            {
                string sHead = "";
                string sTail = "";
                int sp = sPath.IndexOf(BS);
                if (sp >= 0)
                {
                    if (sp == 0)
                    {
                        sp = sPath.IndexOf(BS, 1);
                    }
                    sHead = sPath.Substring(0, sp) + @"\...";
                    sp++;
                }
                else
                {
                    sHead = "...";
                    sp = 0;
                }

                // tail
                int ep = sPath.Length - (maxLen - sHead.Length);
                int idx = sPath.IndexOf(BS, ep);
                if (idx >= ep)
                {
                    // use it
                    ep = idx;
                }
                sTail = sPath.Substring(ep);
                sRet = sHead + sTail;
            }

            return sRet;
        }


        private void DoOpen(string sChartPath)
        {
            try
            {
                _openItem = new SeatingChartItem(sChartPath);
                _openItem.Load();
                UpdateUIForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected: " + ex.Message);
            }
        }

        private void fileOpen_Click(object sender, EventArgs e)
        {
            DialogResult res = QuerySaveDirty(MessageBoxButtons.YesNoCancel);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                string sName = item.Text;
                DoOpen(SeatingChartXmlPath(sName));
                UpdateUIFileMenu();
            }
        }

        private void mnuFileClose_Click(object sender, EventArgs e)
        {
            DialogResult res = QuerySaveDirty(MessageBoxButtons.YesNo);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                _openItem = null;
                UpdateUIForm();
            }
        }

        private void mnuFileDelete_Click(object sender, EventArgs e)
        {
            string sName = _openItem.Name;
            DialogResult res = MessageBox.Show("Delete " + sName + "?", "Seating Chart", MessageBoxButtons.YesNo);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    ReleaseImages(false);
                    Directory.Delete(_openItem.FolderPath, true);
                    _openItem = null;
                    UpdateUIForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't delete " + sName + ": " + ex.Message);
                }
            }
            UpdateUIFileMenu();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            DialogResult res = QuerySaveDirty(MessageBoxButtons.YesNoCancel);
            if (res != System.Windows.Forms.DialogResult.Cancel)
            {
                string sChartName = "";
                res = InputBox("Seating Chart", "Seating Chart Name", ref sChartName);
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    string sChartPath = SeatingChartXmlPath(sChartName);
                    if (File.Exists(sChartPath))
                    {
                        MessageBox.Show(sChartName + " Already exists");
                    }
                    else
                    {
                        try
                        {
                            SeatingChartItem item = new SeatingChartItem(sChartPath);
                            item.Save();
                            UpdateUIFileMenu();
                            DoOpen(sChartPath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                UpdateUIFileMenu();
            }
        }

        private void DoSaveCommon(SeatingChartItem item)
        {
            UpdateSeatingChartItem(item);
            item.Save();
            DoPendingFileDeletes(item);
        }

        private void DoSaveCommon()
        {
            DoSaveCommon(_openItem);
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            DoSaveCommon();
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            string sChartName = "";
            DialogResult res = InputBox("Seating Chart", "Seating Chart Name", ref sChartName);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    SeatingChartItem item = new SeatingChartItem(SeatingChartXmlPath(sChartName));
                    SaveAsPrep(item);
                    DoSaveCommon(item);
                    // cleanup the image files for the one we didn't save to
                    DoPendingFileDeletes(_openItem);
                    _openItem = item;
                    this.Text = "Seating Chart - " + _openItem.Name;
                    UpdateUIFileMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Save failed: " + ex.Message, "Seating Chart", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void SaveAsPrep(SeatingChartItem item)
        {
            if (!Directory.Exists(item.FolderPath))
            {
                Directory.CreateDirectory(item.FolderPath);
            }
            // fix up references
            for (int idx = 0; idx < PictureCount; idx++)
            {
                PictureBox pb = _pictureBoxList[idx];
                string sImagePath = PictureBoxTagText(pb);
                if (!string.IsNullOrWhiteSpace(sImagePath))
                {
                    string sDstPath = Path.Combine(item.FolderPath, Path.GetFileName(sImagePath));
                    File.Copy(sImagePath, sDstPath);
                    pb.Tag = sDstPath;
                }
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
          Form form = new Form();
          Label label = new Label();
          TextBox textBox = new TextBox();
          Button buttonOk = new Button();
          Button buttonCancel = new Button();

          form.Text = title;
          label.Text = promptText;
          textBox.Text = value;

          buttonOk.Text = "OK";
          buttonCancel.Text = "Cancel";
          buttonOk.DialogResult = DialogResult.OK;
          buttonCancel.DialogResult = DialogResult.Cancel;

          label.SetBounds(9, 20, 372, 13);
          textBox.SetBounds(12, 36, 372, 20);
          buttonOk.SetBounds(228, 72, 75, 23);
          buttonCancel.SetBounds(309, 72, 75, 23);

          label.AutoSize = true;
          textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
          buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
          buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

          form.ClientSize = new Size(396, 107);
          form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
          form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
          form.FormBorderStyle = FormBorderStyle.FixedDialog;
          form.StartPosition = FormStartPosition.CenterScreen;
          form.MinimizeBox = false;
          form.MaximizeBox = false;
          form.AcceptButton = buttonOk;
          form.CancelButton = buttonCancel;

          DialogResult dialogResult = form.ShowDialog();
          value = textBox.Text;
          return dialogResult;
        }

        private DialogResult QuerySaveDirty(MessageBoxButtons buttons)
        {
            DialogResult res = System.Windows.Forms.DialogResult.Yes;
            if (_openItem != null)
            {
                SeatingChartItem item = new SeatingChartItem(_openItem.FullName);
                UpdateSeatingChartItem(item);
                string sItemText = item.ToString();
                string sOpenItemText = _openItem.ToString();
                File.WriteAllText(@"c:\temp\1.xml", sItemText);
                File.WriteAllText(@"c:\temp\2.xml", sOpenItemText);

                if (item.ToString() != _openItem.ToString())
                {
                    res = MessageBox.Show("Save changes to " + _openItem.Name + "?", "Seating Chart", buttons);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        DoSaveCommon(item);
                        _openItem = item;
                        UpdateUIFileMenu();
                    }
                }
            }

            return res;
        }

        private void UpdateSeatingChartItem(SeatingChartItem item)
        {
            item.FooterText = txtLab01.Text.Trim(new char[] { ' ', '\t', '\r', '\n' });

            item.Rows.Clear();
            for (int idx = 0; idx < PictureCount; idx++)
            {
                PictureBox pb = _pictureBoxList[idx];
                TextBox tb = _textBoxList[idx];

                // Only update if I have any non-default data
                if (! tb.Visible || !string.IsNullOrWhiteSpace(tb.Text) || !string.IsNullOrWhiteSpace(PictureBoxTagText(pb)))
                {
                    DataRow row = item.NewRow();
                    // XML elides the <CR> from <CR><LF>
                    //row[SeatingChartItem.S.Name] = tb.Text.ToString();
                    string sName = tb.Text.ToString();
                    sName = sName.Replace("\r\n", "\n");
                    row[SeatingChartItem.S.Name] = sName;
                    row[SeatingChartItem.S.ImagePath] = ToRelativePath( PictureBoxTagText(pb) );
                    row[SeatingChartItem.S.Index] = idx;
                    row[SeatingChartItem.S.Show] = tb.Visible;
                    item.Rows.Add(row);
                }
            }
        }

        private void DoPendingFileDeletes(SeatingChartItem item)
        {
            if (item != null)
            {
                Dictionary<string, bool> imgFileDict = new Dictionary<string,bool>();
                DirectoryInfo dirInfo = new DirectoryInfo( item.FolderPath );
                foreach( FileInfo fInfo in dirInfo.GetFiles("*.jpg" ) )
                {
                    imgFileDict.Add( fInfo.FullName.ToLower(), true );
                }

                foreach (DataRow row in item.Rows)
                {
                    string sPath = row[SeatingChartItem.S.ImagePath].ToString();
                    sPath = ToAbsolutePath(sPath).ToLower();
                    if (imgFileDict.ContainsKey(sPath))
                    {
                        imgFileDict.Remove(sPath);
                    }
                }

                foreach (string sPath in imgFileDict.Keys)
                {
                    if (File.Exists(sPath))
                    {
                        File.Delete(sPath);
                    }
                }
            }
        }

        private void frmSearingChart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_openItem != null)
            {
                DialogResult res = QuerySaveDirty(MessageBoxButtons.YesNoCancel);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                string sChartName = "";
                if ( _openItem != null )
                {
                    sChartName = _openItem.Name;
                }
                Settings.Default.OpenChart = sChartName;
                Settings.Default.Save();
            }
        }

        private string NewImagePath()
        {
            string sPath = "";

            DateTime dt = DateTime.Now.ToUniversalTime();
            string sBaseName = dt.ToString("yyyyMMdd.HHmmss");
            int idx = 0;
            while (true)
            {
                string sName = string.Format("{0}.{1:d}.jpg",
                    sBaseName, idx);
                string sIterPath = Path.Combine(_openItem.FolderPath, sName);
                if (! File.Exists(sIterPath))
                {
                    sPath = sIterPath;
                    break;
                }
                idx++;
            }

            return sPath;
        }

        private void pbContextMenu_Opening(object sender, CancelEventArgs e)
        {
            IDataObject oClipboard = Clipboard.GetDataObject() as IDataObject;
            bool bEnabled = (oClipboard != null );
            if ( bEnabled )
            {
                string[] sFormats = oClipboard.GetFormats(); // DEBUG ONLY
                bEnabled =
                    oClipboard.GetDataPresent(DataFormats.Bitmap)
                    || oClipboard.GetDataPresent(DataFormats.FileDrop)
                    || InternalClipboardFilePathPresent();
            }

            pbPaste.Enabled = bEnabled;

            PictureBox pb = SelectedPictureBox;
            pbCut.Enabled = (pb.Image != null);
            pbCopy.Enabled = (pb.Image != null);

            TextBox tb = SelectedPictureLabel;
            pbHide.Visible = tb.Visible;
            pbShow.Visible = !tb.Visible; // Can't use pbHide here because it's Visile state does not change right away

        }

        private bool InternalClipboardFilePathPresent()
        {
            IDataObject oClipboard = Clipboard.GetDataObject() as IDataObject;
            bool bFilePathDrop = (oClipboard != null && oClipboard.GetDataPresent(DataFormats.Text));
            if (bFilePathDrop)
            {
                string sPath = Clipboard.GetText();
                try
                {
                    // Unquote
                    sPath = InternalUnquote(sPath);
                    bFilePathDrop = (! string.IsNullOrWhiteSpace( Path.GetDirectoryName(sPath)));
                    if ( bFilePathDrop )
                    {
                        bFilePathDrop = File.Exists(sPath);
                    }
                }
                catch (Exception)
                {
                    bFilePathDrop = false;
                }
            }

            return bFilePathDrop;
        }

        private string InternalUnquote(string sText)
        {
            string sUnQuote = "";
            try
            {
                sUnQuote = Regex.Replace(sText, "^\"([^\"]*)\"$", @"$1", RegexOptions.Singleline);

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            } 
            return sUnQuote;
        }

        private void mnuFilePrint_Click(object sender, EventArgs e)
        {
            DeselectActiveText();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.PrinterSettings = printDialog1.PrinterSettings;
                printDocument1.DefaultPageSettings.Landscape = true;
                printDocument1.OriginAtMargins = false;
                printDocument1.Print();
            }
        }

        private void mnuFilePrintPreview_Click(object sender, EventArgs e)
        {
            DeselectActiveText();
            printDocument1.DefaultPageSettings.Landscape = true;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //e.PageSettings.Landscape = true;
            Rectangle pageBounds = e.PageBounds;
            Rectangle marginBounds = e.MarginBounds;
            CaptureForm(e);
            e.Graphics.DrawImage(_printBitmap, 0, 0);
        }

        private void CaptureForm(System.Drawing.Printing.PrintPageEventArgs e)
        {
            string sTemp = @"C:\temp\temp.bmp";
            if (File.Exists(sTemp))
            {
                File.Delete(sTemp);
            }

            // HACK
            // Prepare for printing
            UpdateUIPrintView( true );
            Rectangle formRect = new Rectangle(new Point(0,0), Size);
            Bitmap formBitmap = new Bitmap(Width, Height);
            this.DrawToBitmap(formBitmap, formRect);
            UpdateUIPrintView(false);
            //formBitmap.Save(sTemp);

            Rectangle clientRect = new Rectangle(ClientRectLocation, ClientRectangle.Size );
            // account for hard left margin
            float fHardLeft = e.PageSettings.HardMarginX; //in units of 100th inch
            int iHardLeft = (int)(_HRes * fHardLeft / 100.0f);
            clientRect.X += iHardLeft;
            clientRect.Width -= iHardLeft;

            // account for hard top margin
            float fHardTop = e.PageSettings.HardMarginY; //in units of 100th inch
            int iHardTop = (int)(_VRes * fHardTop / 100.0f);
            clientRect.Y += iHardTop;
            clientRect.Height -= iHardTop;

            // and eliminate the menu strip in the client area
            clientRect.Y += mnuFile.Height; ;
            clientRect.Height -= mnuFile.Height; ;
            _printBitmap = formBitmap.Clone(clientRect, System.Drawing.Imaging.PixelFormat.DontCare);
            _printBitmap.Save(sTemp);
            
        }

        private void UpdateUIPrintView(bool bPrint)
        {
            BorderStyle textBoxBorderStyle =
                bPrint
                ? BorderStyle.None
                : _textBoxBorderStyle;

            txtLab01.BorderStyle = textBoxBorderStyle;
            for (int idx = 0; idx < PictureCount; idx++)
            {
                _textBoxList[idx].BorderStyle = textBoxBorderStyle;
                _pictureBoxList[idx].Visible = ( ! bPrint || _textBoxList[idx].Visible );
            }
        }

        private void mnuViewFullSize_Click(object sender, EventArgs e)
        {
            DoZoom(1.0f);

        }

        private int DoZoom(float pct)
        {
            int iWidth = (int)(_HRes * pct * 11.0f );
            // add outer stuff
            iWidth += _formFrameWidth;
            Width = iWidth;

            return iWidth;
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            e.PageSettings.Landscape = true;
        }

        private void pb00_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pb00.Focus();
                PictureBox pb = sender as PictureBox;
                if (pb.Image != null)
                {
                    //Debug.Assert(pb.Tag != null);
                    Debug.Assert(pb.Image != null);
                    //Debug.Assert(! string.IsNullOrWhiteSpace(pb.ImageLocation.ToString()));

                    _dragSrc = pb;
                    _dragSrcLocation = new Point(e.X, e.Y);
                    _dragDst = null;
                }
            }
        }

        private void pb00_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pb00_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                PictureBox pb = sender as PictureBox;
                Control ctl = null;
                Point curpos = Cursor.Position;
                Point clientPoint = this.PointToClient(curpos);
                ctl = this.GetChildAtPoint(clientPoint);
                pb = ctl as PictureBox;
                _dragDst = pb;
                if (_dragDst != null && _dragSrc != null)
                {
                    int iSrc = IndexOf(_dragSrc);
                    int iDst = IndexOf(_dragDst);
                    if (iSrc != iDst)
                    {
                        DeleteImageFile(_dragDst);
                        _dragDst.Tag = _dragSrc.Tag;
                        _dragDst.ImageLocation = PictureBoxTagText( _dragDst );
                        _dragDst.SizeMode = PictureBoxSizeMode.Zoom;
                        _dragDst.Load();
                        _textBoxList[iDst].Text = _textBoxList[iSrc].Text;

                        _textBoxList[iSrc].Clear();
                        _dragSrc.Tag = null;
                        _dragSrc.ImageLocation = null;
                        _dragSrc.ImageLocation = null;
                    }
                }

            }
            // ANY mouse up should kill it
            _dragSrc = null;
            _dragDst = null;
        }

        private int IndexOf(PictureBox pb)
        {
            string sName = pb.Name;
            int idx = Int32.Parse(sName.Substring(2));
            return idx;
        }

        private void frmSearingChart_DragDrop(object sender, DragEventArgs e)
        {

            Point curpos = Cursor.Position;
            Point clientPoint = this.PointToClient(curpos);
            Control ctl = this.GetChildAtPoint(clientPoint);
            PictureBox pb = ctl as PictureBox;

            if (pb == null)
            {
                MessageBox.Show("Can't drop it here");
            }
            else
            {
                bool bOK = false;
                if (e.Data.GetDataPresent(typeof(Bitmap)))
                {
                    e.Effect = DragDropEffects.Copy;
                    Bitmap bm = (Bitmap)e.Data.GetData(typeof(Bitmap));
                    bOK = CropAndUpdate(pb, bm);
                }
                else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

                    if (a != null)
                    {
                        // Extract string from first array element
                        // (ignore all files except first if number of files are dropped).
                        string s = a.GetValue(0).ToString();
                        // CropAndUpdate will figure out if this is a valid image file
                        // and tell the user if it is not
                        bOK = CropAndUpdate(pb, s);
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void frmSearingChart_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void frmSearingChart_DragLeave(object sender, EventArgs e)
        {
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {
            if (! _bPreviewBeenhere)
            {
                _bPreviewBeenhere = true;
                printPreviewDialog1.Width *= 2;
                printPreviewDialog1.Height *= 2;
            }
        }

        private void tb00_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            string sText = tb.Text;
            string sTrimmedText = tb.Text.TrimEnd( new char[] { ' ', '\t', '\r', '\n' } );
            if ( sText.Length != sTrimmedText.Length )
            {
                _bInTextChanged = true;
                try
                {
                    tb.Text = sTrimmedText;
                    _bInTextChanged = false;

                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
                finally
                {
                    _bInTextChanged = false;
                }
            }
        }


        bool CropAndUpdate(PictureBox pb, Image img)
        {
            bool bOK = false;

            frmCrop frm = new frmCrop();
            try
            {
                frm.SetImage(img);
                DialogResult res = frm.ShowCropDialog(this, new Size(_refPictureWidth, _refPictureHeight));
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    NewImageFor(pb, frm.CroppedImage);
                    bOK = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                // Image.FromFile will throw OutOfMemoryException if file is invalid.
                // Don't ask me why.
                MessageBox.Show("Not a valid image file");
            }

            return bOK;
        }

        bool CropAndUpdate(PictureBox pb, string sSrcFilePath)
        {
            Image newImage = Image.FromFile(sSrcFilePath);
            return CropAndUpdate( pb, newImage );
        }

        public string ToRelativePath(string sFullName)
        {
            string sTmpFullName = sFullName;
            string sRelativePath = "";
            if (!string.IsNullOrWhiteSpace(sFullName))
            {
                if ( ! Path.IsPathRooted( sFullName ) )
                {
                    sTmpFullName = Path.GetFullPath(sFullName);
                }
                sRelativePath = PathUtils.RelativePath(sTmpFullName, _sWorkspacePath);
            }

            return sRelativePath;
        }

        public string ToAbsolutePath(string sRelativePath)
        {
            string sAbsolute = "";

            if (!string.IsNullOrWhiteSpace(sRelativePath))
            {

                if (Path.IsPathRooted(sRelativePath))
                {
                    sAbsolute = sRelativePath;

                    // HACK
                    string sArgAppDataFullName = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(sRelativePath)));
                    string sExpectedAppDataFullName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    if (string.Compare(sArgAppDataFullName, sExpectedAppDataFullName, true) != 0)
                    {
                        // re-parent
                        sAbsolute = sExpectedAppDataFullName + sRelativePath.Substring(sArgAppDataFullName.Length);
                    }
                }
                else
                {
                    sAbsolute = Path.Combine(_sWorkspacePath, sRelativePath);
                }
            }

            return sAbsolute;
        }

        private void mnuEditCleanupNames_Click(object sender, EventArgs e)
        {

        }

        private void DoTextChanged(TextBox tb)
        {
            int iCurs = tb.SelectionStart;
            if (iCurs == tb.TextLength)
            {
                int iCharIdx = iCurs - 1;
                int iLine = tb.GetLineFromCharIndex(iCharIdx);
                int iNLCount = _countNewLineRegex.Matches(tb.Text).Count;
                if (iLine > iNLCount)
                {
                    string sText = tb.Text;
                    for (int idx = iCharIdx; idx > 0; idx--)
                    {
                        char ch = sText[idx];
                        if ( ch == ' ' || ch == '\t' )
                        {
                            string sNewText = sText.Substring(0, idx );
                            sNewText += "\r\n" + sText.Substring( idx+1 );
                            tb.Text = sNewText;
                            tb.SelectionStart = sNewText.Length;
                            break;
                        }
                    }
                }
            }
        }


        private void tb00_TextChanged(object sender, EventArgs e)
        {
            if ( ! _bInTextChanged && Width == _iFullSizeWidth)
            {
                _bInTextChanged = true;
                TextBox tb = sender as TextBox;
                DoTextChanged(tb);
                _bInTextChanged = false;

            }
        }

        private void mnuCleanupNamesFirstLast_Click(object sender, EventArgs e)
        {
            _bInTextChanged = true;

            try
            {
                for (int idx = 0; idx < PictureCount; idx++)
                {
                    TextBox tb = _textBoxList[idx];
                    string sText = tb.Text.Trim(new char[] { ' ', '\t', '\r', '\n' });
                    if (sText.Length > 0)
                    {


                        // blow away consecutive new lines and spaces, replace with a single space
                        string sNewText = Regex.Replace(sText, @"[ \t\r\n]+", " ", RegexOptions.Singleline);
                        string sNewName = "";

                        // see if we have a comma
                        string[] sLF = sText.Trim().Split(',');
                        if (sLF.Length > 2)
                        {
                            throw new Exception(string.Format("don't know what to do with two commas in '{0}'",
                                sText)
                            );
                        }
                        if (sLF.Length > 1)
                        {
                            string sSepCh = "";
                            string[] sWords = sLF[1].Trim().Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (sWords.Length != 1)
                            {
                                Debug.Print("");
                            }
                            foreach (string sIterWord in sWords)
                            {
                                sNewName += sSepCh + sIterWord.Trim();
                                sSepCh = " ";
                            }
                            sNewName += "\r\n" + sLF[0];
                            tb.Text = sNewName;
                        }
                        else
                        {
                            // assume only one 
                            sNewName = Regex.Replace(sText, @"\s([^\s]+)$", "\r\n$1", RegexOptions.Singleline);
                            tb.Text = sNewName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            finally
            {
                _bInTextChanged = false;
            }

        }

        private void mnuCleanupNamesLastFirst_Click(object sender, EventArgs e)
        {
            _bInTextChanged = true;

            try
            {
                for (int idx = 0; idx < PictureCount; idx++)
                {
                    TextBox tb = _textBoxList[idx];
                    string sText = tb.Text.Trim(new char[] { ' ', '\t', '\r', '\n' });
                    if (sText.Length > 0)
                    {
                        // blow away consecutive new lines and spaces, replace with a single space
                        string sNewText = Regex.Replace(sText, @"[ \t\r\n]+", " ", RegexOptions.Singleline);
                        string sNewName = "";

                        // see if we have a comma
                        string[] sLF = sText.Trim().Split(',');
                        if (sLF.Length > 2)
                        {
                            throw new Exception(string.Format("don't know what to do with two commas in '{0}'",
                                sText)
                            );
                        }
                        if (sLF.Length > 1)
                        {
                            string sSepCh = "";
                            string[] sWords = sLF[1].Trim().Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (sWords.Length != 1)
                            {
                                Debug.Print("");
                            }
                            foreach (string sIterWord in sWords)
                            {
                                sNewName += sSepCh + sIterWord.Trim();
                                sSepCh = " ";
                            }
                            sNewName = sLF[0] + "," + "\r\n" + sNewName;
                            tb.Text = sNewName;
                        }
                        else
                        {
                            // assume last word is last name
                            string sSepCh = "";
                            string[] sWords = sText.Trim().Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            int iLastWordIdx = sWords.Length - 1;
                            for (int iWordIdx = 0; iWordIdx < iLastWordIdx; iWordIdx++)
                            {
                                sNewName += sSepCh + sWords[iWordIdx];
                                sSepCh = " ";
                            }
                            sNewName = sWords[iLastWordIdx] + ",\r\n" + sNewName;

                            tb.Text = sNewName;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            finally
            {
                _bInTextChanged = false;
            }


        }

        private void frmSearingChart_Activated(object sender, EventArgs e)
        {
            DeselectActiveText();
        }

		private void mnuOpenProjectFolder_Click(object sender, EventArgs e)
		{
			Process p = new Process();
			p.StartInfo.FileName = @"explorer.exe";
			p.StartInfo.Arguments = @"file:\\\" + _sWorkspacePath;
			p.Start();
		}
	}
}
