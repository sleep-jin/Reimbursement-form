using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;

namespace 发票
{
    public class BaiduOCR
    {
        private readonly string _apiKey;
        private readonly string _secretKey;

        // Token 缓存
        private string _accessToken;
        private DateTime _tokenExpireTime;

        public BaiduOCR(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        /// <summary>
        /// OCR 识别
        /// </summary>
        public string ClientOCR(string base64Image)
        {
            string token = GetValidToken();
            string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token={token}";

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("image", base64Image)
            });

            // 同步发送
            var response = client.PostAsync(url, content).GetAwaiter().GetResult();
            string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return ParseText(json);
        }

        public string ClientOCR(Bitmap image)
        {
            string image64 = Base64Encode(image);
            return ClientOCR(image64);
        }

        /// <summary>
        /// 获取有效 Token（带缓存）
        /// </summary>
        private string GetValidToken()
        {
            // Token 有效期内直接返回
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpireTime)
            {
                return _accessToken;
            }

            // 重新获取
            using var client = new HttpClient();
            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _apiKey,
                ["client_secret"] = _secretKey
            });

            var response = client.PostAsync(
                "https://aip.baidubce.com/oauth/2.0/token",
                requestContent).GetAwaiter().GetResult();

            string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // 检查错误
            if (root.TryGetProperty("error", out var error))
            {
                throw new Exception($"获取Token失败: {error.GetString()}");
            }

            _accessToken = root.GetProperty("access_token").GetString();
            int expiresIn = root.GetProperty("expires_in").GetInt32(); // 秒

            // 提前5分钟过期，避免边界问题
            _tokenExpireTime = DateTime.Now.AddSeconds(expiresIn - 300);

            return _accessToken;
        }

        private string ParseText(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("error_code", out _))
                {
                    string errorMsg = root.GetProperty("error_msg").GetString();
                    Console.WriteLine($"OCR错误 [{root.GetProperty("error_code")}]: {errorMsg}");
                    return "";
                }

                var sb = new StringBuilder();
                foreach (var item in root.GetProperty("words_result").EnumerateArray())
                {
                    sb.Append(item.GetProperty("words").GetString());
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析失败: {ex.Message}");
                return "";
            }
        }

        public string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public string Base64Encode(Bitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return Base64Encode(ms.ToArray());
        }
    }
}
