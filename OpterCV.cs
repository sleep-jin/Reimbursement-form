using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;

namespace 发票
{
    public class OpterCV
    {
        /// <summary>
        /// 使用截好的图片做模板进行模板匹配,获取坐标后进行裁剪
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fromRoi"></param>
        /// <param name="toRoi"></param>
        public static Point RetancROI(Bitmap Mainimage,Bitmap tempImage,bool test)
        {
            Mat Mainmat = BitmapToMat(Mainimage);
            Mat TempMat = BitmapToMat(tempImage);
            Cv2.CvtColor(Mainmat, Mainmat, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(TempMat, TempMat, ColorConversionCodes.BGR2GRAY);
            return TemplateMatch(Mainmat, TempMat, test);
        }
        /// <summary>
        /// 执行模板匹配，返回最佳匹配位置
        /// </summary>
        private static OpenCvSharp.Point TemplateMatch(Mat src, Mat template,bool tiaoshi)
        {
            using (Mat result = new Mat())
            {
                // 归一化相关系数匹配法（对光照变化较鲁棒）
                Cv2.MatchTemplate(src, template, result, TemplateMatchModes.CCoeffNormed);

                // 找到最佳匹配位置
                Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);

                // 克隆显示
                if (tiaoshi)
                {
                    using (Mat debug = src.Clone())
                    {
                        Point br = new Point(maxLoc.X + template.Width, maxLoc.Y + template.Height);
                        debug.Rectangle(maxLoc, br, new Scalar(0, 0, 255), 10);
                        Cv2.NamedWindow("123", WindowFlags.Normal);
                        Cv2.ImShow("123", debug);
                        Cv2.WaitKey(0);  // 阻塞，方便看结果
                    }
                }

                return maxLoc;
            }
        }
        /// <summary>
        /// Bitmap 转 OpenCV Mat
        /// </summary>
        private static Mat BitmapToMat(Bitmap bitmap)
        {
            // 处理不同像素格式
            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                return BitmapConverter.ToMat(bitmap);
            }

            // 其他格式先转24位RGB
            using (Bitmap bmp24 = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bmp24))
                {
                    g.DrawImage(bitmap, 0, 0);
                }
                return BitmapConverter.ToMat(bmp24);
            }
        }
        /// <summary>
        /// 截去图像
        /// </summary>
        /// <param name="source"></param>
        /// <param name="roi"></param>
        /// <returns></returns>
        public static Bitmap CropImage(Bitmap source, Rectangle roi)
        {
            Bitmap target = new Bitmap(roi.Width, roi.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source,
                    new Rectangle(0, 0, roi.Width, roi.Height),
                    roi,
                    GraphicsUnit.Pixel);
            }
            return target;
        }
    }
}
