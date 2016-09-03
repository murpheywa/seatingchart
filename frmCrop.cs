using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SeatingChart
{
    public partial class frmCrop : Form
    {
        private Image _croppedImage = null;
        private bool _selecting = false;
        private bool _moving = false;
        private Point _movingPoint;
        private Rectangle _selection;
        private Size _cropRatio;

        public DialogResult ShowCropDialog( IWin32Window owner, Size cropRatio)
        {
            _cropRatio = new Size();
            _cropRatio = cropRatio;
            DialogResult res = this.ShowDialog(owner);
            return res;
        }

        public frmCrop()
        {
            InitializeComponent();
        }

        private void frmCrop_Load(object sender, EventArgs e)
        {
            _selection = new Rectangle(0, 0, 0, 0);
        }

        public Image CroppedImage
        {
            get
            {
                return _croppedImage;
            }
        }

        public void SetImage(Image bm)
        {
            pbCrop.Image = bm;
            pbCrop.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void pbCrop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _selecting = false;
                _moving = false;
                if (_selection.Width > 0)
                {
                    // are we on lower left hand corner for resizing?
                    bool bLRHC = (Math.Abs(e.X - _selection.Right) < 4);
                    if (bLRHC)
                    {
                        bLRHC = (Math.Abs(e.Y - _selection.Bottom) < 4);
                    }
                    if (bLRHC)
                    {
                        _selecting = true;
                    }
                    // or are we within the selection?
                    else if ( _selection.Contains( e.Location ) )
                    {
                        _moving = true;
                        _movingPoint = new Point(e.X, e.Y);
                    }
                }
                if ( ! _moving && ! _selecting )
                {
                    _selecting = true;
                    _selection = new Rectangle(new Point(e.X, e.Y), new Size());
                }
            }

        }

        private void pbCrop_MouseMove(object sender, MouseEventArgs e)
        {
            // Update the actual size of the selection:
            if (_selecting)
            {

                // Maintain aspect ratio of 
                int iDeltaX = Math.Abs(e.X - _selection.X);
                int iDeltaY = Math.Abs(e.Y - _selection.Y);

                _selection.Width = e.X - _selection.X;
                _selection.Height = e.Y - _selection.Y;

                if (iDeltaX >= iDeltaY)
                {
                    // solve for Y
                    _selection.Width = e.X - _selection.X;
                    _selection.Height = (int)Math.Round((double)_selection.Width * (float)_cropRatio.Height / (float)_cropRatio.Width);
                }
                else
                {
                    // solve for x
                    _selection.Height = e.Y - _selection.Y;
                    _selection.Width = (int)Math.Round((double)_selection.Height * (float)_cropRatio.Width/ (float)_cropRatio.Height);
                }


                // Redraw the picturebox:
                pbCrop.Refresh();
            }
            else if (_moving)
            {
                _selection.X += (e.X - _movingPoint.X);
                _selection.Y += (e.Y - _movingPoint.Y);
                _movingPoint = new Point(e.X, e.Y);
                // Redraw the picturebox:
                pbCrop.Refresh();
            }
            else
            {
                Debug.Print("");
            }
        }

        private void pbCrop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_selecting)
                {
                    //// Create cropped image:
                    //Image img = pictureBox1.Image.Crop(_selection);

                    //// Fit image to the picturebox:
                    //pictureBox1.Image = img.Fit2PictureBox(pictureBox1);

                    _selecting = false;
                }
                else if (_moving)
                {
                    _moving = false;
                }
            }


        }

        private void pbCrop_Paint(object sender, PaintEventArgs e)
        {
            if (_selecting || _moving)
            {
                // Draw a rectangle displaying the current selection
                Pen pen = Pens.GreenYellow;
                e.Graphics.DrawRectangle(pen, _selection);
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                // assume we are OK
                DialogResult = System.Windows.Forms.DialogResult.OK;
                // but the user may not have cropped accidentally
                if (_selection.Width < 10)
                {
                    DialogResult res = MessageBox.Show("You haven't cropped the image. Are we done?", "", MessageBoxButtons.YesNo);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        _selection = pbCrop.ClientRectangle;
                    }
                    else
                    {
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
                if (DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    // Crop
                    // Create cropped image:
                    // scale
                    Rectangle rect = pbCrop.ClientRectangle;
                    /*
                    float xPct = (float)_selection.X / (float)pbCrop.Width;
                    float yPct = (float)_selection.Y / (float)pbCrop.Height;
                    float widPct = (float)_selection.Width / (float)pbCrop.Width;
                    float htPct = (float)_selection.Height / (float)pbCrop.Height;

                    Image src = pbCrop.Image;
                    Rectangle imgRect = new Rectangle();
                    imgRect.X = (int)((float)src.Width * xPct);
                    imgRect.Width = (int)((float)src.Width * widPct);
                    imgRect.Y = (int)((float)src.Height * yPct);
                    // Go off of passed in ratio
                    imgRect.Height = (int)((float)src.Height * htPct);
                    imgRect.Height = (int)((float)imgRect.Width * (float) _cropRatio.Height / (float)_cropRatio.Width);
                    */
                    Point imgUL = pbCrop.TranslateZoomMousePosition(new Point(_selection.Left, _selection.Top));
                    Point imgLR = pbCrop.TranslateZoomMousePosition(new Point(_selection.Right, _selection.Bottom));
                    Size sz = new System.Drawing.Size((imgLR.X - imgUL.X), (imgLR.Y - imgUL.Y));
                    Rectangle imgRect = new Rectangle(imgUL, sz);

                    Image img = pbCrop.Image.Crop(imgRect);
                    img.Save(@"C:\temp\crop.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    // Fit image to the picturebox:
                    //pictureBox1.Image = img.Fit2PictureBox(pictureBox1);

                    _croppedImage = img;

                    Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void cmdRotateRight_Click(object sender, EventArgs e)
        {
            pbCrop.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pbCrop.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pbCrop.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pbCrop.Refresh();
        }
    }

    internal static class Exex
    {
        /// <summary>
        /// Gets the mouse position over the image when the <see cref="PictureBox">PictureBox's
        /// </see> <see cref="PictureBox.SizeMode">SizeMode</see> is set to Zoom
        /// </summary>
        /// <param name="coordinates">Point to translate</param>
        /// <returns>A point relative to the top left corner of the 
        /// <see cref="PictureBox.Image">Image</see>
        /// If the Image is null, no translation is performed
        /// </returns>
        public static Point TranslateZoomMousePosition(this PictureBox me, Point coordinates)
        {
            // test to make sure our image is not null
            if (me.Image == null) return coordinates;
            // Make sure our control width and height are not 0 and our 
            // image width and height are not 0
            if (me.Width == 0 || me.Height == 0 || me.Image.Width == 0 || me.Image.Height == 0) return coordinates;
            // This is the one that gets a little tricky. Essentially, need to check 
            // the aspect ratio of the image to the aspect ratio of the control
            // to determine how it is being rendered
            float imageAspect = (float)me.Image.Width / me.Image.Height;
            float controlAspect = (float)me.Width / me.Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)me.Image.Width / me.Width;
                newX *= ratioWidth;
                float scale = (float)me.Width / me.Image.Width;
                float displayHeight = scale * me.Image.Height;
                float diffHeight = me.Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)me.Image.Height / me.Height;
                newY *= ratioHeight;
                float scale = (float)me.Height / me.Image.Height;
                float displayWidth = scale * me.Image.Width;
                float diffWidth = me.Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }
    }
}
