using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 发票
{
    public class ZoomImageBox : Control
    {
        private Image _image;
        private float _zoom = 1.0f;
        private Point _panOffset = Point.Empty;
        private bool _isPanning = false;
        private Point _panStart;
        private Point _panStartOffset;

        private bool _isSelecting = false;
        private Point _selectStart;
        private Point _selectEnd;
        private Rectangle _selectRect;
        private Rectangle _imageSelectRect;
        private Rectangle? _highlightRect = null;

        public Image Image => _image;
        public float Zoom => _zoom;
        public Point PanOffset => _panOffset;
        public Rectangle SelectedImageRect => _imageSelectRect;

        public event EventHandler SelectionCompleted;

        public ZoomImageBox()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.DarkGray;
        }

        public void SetImage(Image image)
        {
            _image = image;
            _zoom = 1.0f;
            _panOffset = Point.Empty;
            _selectRect = Rectangle.Empty;
            _imageSelectRect = Rectangle.Empty;
            _highlightRect = null;

            if (_image != null && this.Width > 0 && this.Height > 0)
                FitToWindow();

            Invalidate();
        }

        public void SetHighlightRect(Rectangle imgRect)
        {
            _highlightRect = imgRect;
            Invalidate();
        }

        public void ClearHighlight()
        {
            _highlightRect = null;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_image != null && this.Width > 0 && this.Height > 0)
                FitToWindow();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isSelecting = true;
                _isPanning = false;
                _selectStart = e.Location;
                _selectEnd = e.Location;
                _selectRect = Rectangle.Empty;
                _imageSelectRect = Rectangle.Empty;
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isPanning = true;
                _isSelecting = false;
                _panStart = e.Location;
                _panStartOffset = _panOffset;
                this.Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_isSelecting)
            {
                _selectEnd = e.Location;
                _selectRect = GetNormalizedRect(_selectStart, _selectEnd);
                Invalidate();
            }
            else if (_isPanning)
            {
                _panOffset.X = _panStartOffset.X + (e.X - _panStart.X);
                _panOffset.Y = _panStartOffset.Y + (e.Y - _panStart.Y);
                ClampPanOffset();
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && _isSelecting)
            {
                _isSelecting = false;
                _selectEnd = e.Location;
                _selectRect = GetNormalizedRect(_selectStart, _selectEnd);

                if (_selectRect.Width > 5 && _selectRect.Height > 5)
                {
                    _imageSelectRect = ScreenToImageRect(_selectRect);
                    _imageSelectRect.Intersect(new Rectangle(0, 0, _image.Width, _image.Height));

                    if (_imageSelectRect.Width > 0 && _imageSelectRect.Height > 0)
                    {
                        SelectionCompleted?.Invoke(this, EventArgs.Empty);
                    }
                }

                _selectRect = Rectangle.Empty;
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right && _isPanning)
            {
                _isPanning = false;
                this.Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (_image == null) return;

            PointF imgPosBefore = ScreenToImage(e.Location);

            if (e.Delta > 0)
                _zoom = Math.Min(_zoom * 1.1f, 10.0f);
            else
                _zoom = Math.Max(_zoom / 1.1f, 0.1f);

            PointF imgPosAfter = new PointF(imgPosBefore.X * _zoom, imgPosBefore.Y * _zoom);
            _panOffset.X = (int)(e.X - imgPosAfter.X);
            _panOffset.Y = (int)(e.Y - imgPosAfter.Y);

            ClampPanOffset();
            Invalidate();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            FitToWindow();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(this.BackColor);

            string debug = $"Control: {this.Width}x{this.Height}";
            e.Graphics.DrawString(debug, this.Font, Brushes.Yellow, 5, 5);

            if (_image == null)
            {
                string msg = "没有图片";
                SizeF size = e.Graphics.MeasureString(msg, this.Font);
                e.Graphics.DrawString(msg, this.Font, Brushes.White,
                    (this.Width - size.Width) / 2,
                    (this.Height - size.Height) / 2);
                return;
            }

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            int drawW = (int)(_image.Width * _zoom);
            int drawH = (int)(_image.Height * _zoom);

            string debug2 = $"Img: {_image.Width}x{_image.Height} | Zoom: {_zoom:F2} | Draw: {drawW}x{drawH}";
            e.Graphics.DrawString(debug2, this.Font, Brushes.Yellow, 5, 25);

            Rectangle destRect = new Rectangle(_panOffset.X, _panOffset.Y, drawW, drawH);
            e.Graphics.DrawImage(_image, destRect);

            if (_highlightRect.HasValue)
            {
                Rectangle screenHighlight = ImageToScreenRect(_highlightRect.Value);
                using (Brush brush = new SolidBrush(Color.FromArgb(80, Color.Green)))
                {
                    e.Graphics.FillRectangle(brush, screenHighlight);
                }
                using (Pen pen = new Pen(Color.Green, 3))
                {
                    e.Graphics.DrawRectangle(pen, screenHighlight);
                }
            }

            if (_isSelecting || _selectRect.Width > 0)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawRectangle(pen, _selectRect);
                }
                using (Brush brush = new SolidBrush(Color.FromArgb(50, Color.Red)))
                {
                    e.Graphics.FillRectangle(brush, _selectRect);
                }
            }
        }

        public Point ScreenToImage(Point screenPoint)
        {
            return new Point(
                (int)((screenPoint.X - _panOffset.X) / _zoom),
                (int)((screenPoint.Y - _panOffset.Y) / _zoom));
        }

        public Rectangle ScreenToImageRect(Rectangle screenRect)
        {
            Point imgLoc = ScreenToImage(screenRect.Location);
            return new Rectangle(imgLoc.X, imgLoc.Y,
                (int)(screenRect.Width / _zoom),
                (int)(screenRect.Height / _zoom));
        }

        public Point ImageToScreen(Point imgPoint)
        {
            return new Point(
                (int)(imgPoint.X * _zoom + _panOffset.X),
                (int)(imgPoint.Y * _zoom + _panOffset.Y));
        }

        public Rectangle ImageToScreenRect(Rectangle imgRect)
        {
            Point screenLoc = ImageToScreen(imgRect.Location);
            return new Rectangle(screenLoc.X, screenLoc.Y,
                (int)(imgRect.Width * _zoom),
                (int)(imgRect.Height * _zoom));
        }

        private Rectangle GetNormalizedRect(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p2.X - p1.X),
                Math.Abs(p2.Y - p1.Y));
        }

        private void ClampPanOffset()
        {
            if (_image == null) return;
            int imgW = (int)(_image.Width * _zoom);
            int imgH = (int)(_image.Height * _zoom);

            int minX = Math.Min(0, this.Width - imgW);
            int maxX = Math.Max(0, this.Width - imgW);
            int minY = Math.Min(0, this.Height - imgH);
            int maxY = Math.Max(0, this.Height - imgH);

            _panOffset.X = Math.Max(minX, Math.Min(_panOffset.X, maxX));
            _panOffset.Y = Math.Max(minY, Math.Min(_panOffset.Y, maxY));
        }

        public void FitToWindow()
        {
            if (_image == null || this.Width <= 0 || this.Height <= 0) return;

            float scaleX = (float)this.ClientSize.Width / _image.Width;
            float scaleY = (float)this.ClientSize.Height / _image.Height;
            _zoom = Math.Min(scaleX, scaleY);

            if (_zoom <= 0 || float.IsInfinity(_zoom) || float.IsNaN(_zoom))
                _zoom = 1.0f;

            int drawW = (int)(_image.Width * _zoom);
            int drawH = (int)(_image.Height * _zoom);
            _panOffset.X = (this.ClientSize.Width - drawW) / 2;
            _panOffset.Y = (this.ClientSize.Height - drawH) / 2;

            Invalidate();
        }

        public void ResetView()
        {
            _zoom = 1.0f;
            _panOffset = Point.Empty;
            Invalidate();
        }

        public void ClearSelection()
        {
            _imageSelectRect = Rectangle.Empty;
            _selectRect = Rectangle.Empty;
            Invalidate();
        }
    }
}
