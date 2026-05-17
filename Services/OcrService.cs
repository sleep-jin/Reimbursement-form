using System.Drawing;

namespace 发票
{
    /// <summary>
    /// OCR 服务封装（同步包装，后续可改为异步）
    /// </summary>
    public class OcrService
    {
        private readonly BaiduOcrSync _client;

        public OcrService(string apiKey, string secret,bool choese=false)
        {
            _client = new BaiduOcrSync(apiKey, secret, choese);
        }

        public string Recognize(Bitmap bitmap)
        {
            return _client.Recognize(bitmap);
        }
    }
}
