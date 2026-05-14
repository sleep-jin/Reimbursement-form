using OfficeOpenXml;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Spire.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using static 发票.MakeModes;
using Rectangle = System.Drawing.Rectangle;

namespace 发票
{
    public partial class Form1 : Form
    {
        // ========== 百度 OCR 配置 ==========
        // 服务层
        private OcrService _ocrService;
        private readonly TemplateService _templateService;
        private readonly FileExportService _exportService;
        private string _lastApiKey = "";
        private string _lastApiSecret = "";
        private Dictionary<string, List<TemplateItem>> classtemp = new Dictionary<string, List<TemplateItem>>();

        public Form1()
        {
            InitializeComponent();
            _templateService = new TemplateService(templatesDir);
            _exportService = new FileExportService();
            // _ocrService 将在 Start_Click 时根据 textBox3/textBox4 初始化
        }

        /// <summary>
        /// 初始化或刷新 OCR 服务（仅在 API Key 改变时重新创建）
        /// </summary>
        private void EnsureOcrService(string apiKey, string apiSecret)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
            {
                throw new ArgumentException("API Key 和 Secret 不能为空");
            }
            // 仅在密钥改变时重新创建实例，避免频繁创建
            if (_lastApiKey != apiKey || _lastApiSecret != apiSecret)
            {
                _ocrService = new OcrService(apiKey, apiSecret);
                _lastApiKey = apiKey;
                _lastApiSecret = apiSecret;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (textBox2.Text != null)
            {
                dialog.SelectedPath = textBox2.Text;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dialog.SelectedPath;
            }

        }

        ExcelWorksheet ws;
        ExcelPackage package;

        /// <summary>
        /// 打开文件夹并将其中的文件列入界面文件列表
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            PDFdata.Rows.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            try
            {
                if (textBox1.Text != null)
                {
                    dialog.SelectedPath = textBox1.Text;
                }
            }
            catch
            {
                throw;
            }
            if (DialogResult.OK == dialog.ShowDialog())
            {
                textBox1.Text = dialog.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(textBox1.Text);
                FileInfo[] files = info.GetFiles();
                foreach (FileInfo file in files)
                {
                    //Pdf(file.FullName);
                    PDFdata.Rows.Add(System.IO.Path.GetFileName(file.FullName), file.FullName, "1");
                }
            }
        }

        List<copyfile> list = new List<copyfile>();

        /// <summary>
        /// 从 PDF 中提取文本（未用于 UI 流程的辅助方法）
        /// </summary>
        public void Pdf(string path)
        {
            var txt = new System.Text.StringBuilder();
            using (var doc = UglyToad.PdfPig.PdfDocument.Open(path))
            {
                copyfile copyfile;
                copyfile.path = path;
                foreach (var page in doc.GetPages())
                {
                    foreach (var w in page.GetWords(NearestNeighbourWordExtractor.Instance))
                        txt.Append(w.Text).Append(' ');
                }
                string tt = CleanPdfText(txt.ToString());
            }
        }

        /// <summary>
        /// 清洗从 PDF 中提取的文本，去除多余空白与非法字符
        /// </summary>
        private string CleanPdfText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            text = Regex.Replace(text, @"\s+", "");
            text = text.Replace("：", ":")
                       .Replace("（", "(")
                       .Replace("）", ")")
                       .Replace("￥", "¥");
            text = Regex.Replace(text, @"[^\u4e00-\u9fa5A-Za-z0-9¥\.\-:]", "");
            return text;
        }

        private void MakeMode_Click(object sender, EventArgs e)
        {
            if (PDFdata.Rows.Count == 0 || listBox1.SelectedIndex < 0) return;

            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
            pdf.LoadFromFile(PDFdata.CurrentRow.Cells[1].Value.ToString());
            Image image = pdf.SaveAsImage(0, PdfImageType.Bitmap, 600, 600);
            MakeModes modes = new MakeModes(image, listBox1.SelectedItem.ToString());
            modes.ShowDialog();
        }

        private string templatesDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
        private void 编辑分类_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(templatesDir))
            {
                Directory.CreateDirectory(templatesDir);
            }

            using (var form = new Form())
            {
                form.Text = "修改发票类型分类";
                form.ClientSize = new System.Drawing.Size(500, 400);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var lblList = new Label { Text = "现有分类:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 25) };
                var listBox = new ListBox { Location = new System.Drawing.Point(20, 50), Size = new System.Drawing.Size(200, 280) };

                RefreshListBox(listBox);

                var btnAdd = new Button { Text = "添加分类", Location = new System.Drawing.Point(250, 50), Size = new System.Drawing.Size(120, 35) };
                var btnRename = new Button { Text = "重命名", Location = new System.Drawing.Point(250, 100), Size = new System.Drawing.Size(120, 35) };
                var btnDelete = new Button { Text = "删除分类", Location = new System.Drawing.Point(250, 150), Size = new System.Drawing.Size(120, 35) };
                var btnRefresh = new Button { Text = "刷新列表", Location = new System.Drawing.Point(250, 200), Size = new System.Drawing.Size(120, 35) };
                var btnClose = new Button { Text = "关闭", Location = new System.Drawing.Point(250, 300), Size = new System.Drawing.Size(120, 35), DialogResult = DialogResult.OK };

                btnAdd.Click += (s, ev) =>
                {
                    string newName = ShowInputDialog("请输入新分类名称:", "添加分类", "");
                    if (string.IsNullOrWhiteSpace(newName)) return;

                    string newDir = System.IO.Path.Combine(templatesDir, newName);
                    if (Directory.Exists(newDir))
                    {
                        MessageBox.Show("该分类已存在！", "错误");
                        return;
                    }

                    try
                    {
                        Directory.CreateDirectory(newDir);
                        listBox.Items.Add(newName);
                        MessageBox.Show("分类添加成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"添加失败: {ex.Message}", "错误");
                    }
                };

                btnRename.Click += (s, ev) =>
                {
                    if (listBox.SelectedItem == null)
                    {
                        MessageBox.Show("请先选择要重命名的分类！", "提示");
                        return;
                    }

                    string oldName = listBox.SelectedItem.ToString();
                    string newName = ShowInputDialog($"将 [{oldName}] 重命名为:", "重命名分类", oldName);
                    if (string.IsNullOrWhiteSpace(newName) || newName == oldName) return;

                    string oldDir = System.IO.Path.Combine(templatesDir, oldName);
                    string newDir = System.IO.Path.Combine(templatesDir, newName);

                    if (Directory.Exists(newDir))
                    {
                        MessageBox.Show("目标分类已存在！", "错误");
                        return;
                    }

                    try
                    {
                        Directory.Move(oldDir, newDir);
                        string oldJson = System.IO.Path.Combine(newDir, $"{oldName}.json");
                        string newJson = System.IO.Path.Combine(newDir, $"{newName}.json");
                        if (File.Exists(oldJson))
                        {
                            File.Move(oldJson, newJson);
                        }

                        int index = listBox.SelectedIndex;
                        listBox.Items[index] = newName;
                        MessageBox.Show("重命名成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"重命名失败: {ex.Message}", "错误");
                    }
                };

                btnDelete.Click += (s, ev) =>
                {
                    if (listBox.SelectedItem == null)
                    {
                        MessageBox.Show("请先选择要删除的分类！", "提示");
                        return;
                    }

                    string selectedName = listBox.SelectedItem.ToString();
                    if (MessageBox.Show($"确定删除分类 [{selectedName}] 吗？\n该分类下的所有模板将被删除！",
                        "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }

                    string dir = System.IO.Path.Combine(templatesDir, selectedName);
                    try
                    {
                        Directory.Delete(dir, true);
                        listBox.Items.Remove(selectedName);
                        MessageBox.Show("分类已删除！", "提示");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除失败: {ex.Message}", "错误");
                    }
                };

                btnRefresh.Click += (s, ev) =>
                {
                    RefreshListBox(listBox);
                };

                form.Controls.AddRange(new Control[] { lblList, listBox, btnAdd, btnRename, btnDelete, btnRefresh, btnClose });

                if (form.ShowDialog() == DialogResult.OK)
                {
                }
                RefreshListBox(listBox1);
            }
        }
        #region
        private void RefreshListBox(ListBox listBox)
        {
            listBox.Items.Clear();
            classtemp.Clear();
            foreach (var folder in Directory.GetDirectories(templatesDir))
            {
                string folderName = System.IO.Path.GetFileName(folder);
                listBox.Items.Add(folderName);
                classtemp.Add(folderName, new List<TemplateItem>());
            }
        }

        private string ShowInputDialog(string prompt, string title, string defaultValue)
        {
            using (var form = new Form())
            {
                form.Text = title;
                form.ClientSize = new System.Drawing.Size(400, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;

                var lbl = new Label { Text = prompt, Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(360, 25) };
                var txt = new TextBox { Text = defaultValue, Location = new System.Drawing.Point(20, 50), Size = new System.Drawing.Size(360, 25) };
                var btnOK = new Button { Text = "确定", Location = new System.Drawing.Point(100, 90), DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "取消", Location = new System.Drawing.Point(220, 90), DialogResult = DialogResult.Cancel };

                form.Controls.AddRange(new Control[] { lbl, txt, btnOK, btnCancel });

                return form.ShowDialog() == DialogResult.OK ? txt.Text : "";
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshListBox(listBox1);
            if (File.Exists(excelConfigPath))
            {
                string[] stringps = File.ReadAllLines(excelConfigPath);

                for (int i = 0; i < listBox1.Items.Count; i++)//读取表格格式
                {
                    try
                    {
                        string[] str = stringps.FirstOrDefault(s => s.Contains(listBox1.Items[i].ToString())).Split(':');
                        ExcelFormat.Add(str[0], str[1]);
                    }
                    catch
                    {
                        ExcelFormat.Add(listBox1.Items[i].ToString(), "");
                    }
                }


                string[] ss = File.ReadAllLines(pdfConfigPath);
                for (int i = 0; i < listBox1.Items.Count; i++)//读取Pdf格式
                {
                    try
                    {
                        string[] str = ss.FirstOrDefault(s => s.Contains(listBox1.Items[i].ToString())).Split(':');
                        PDFFormat.Add(str[0], str[1]);
                    }
                    catch
                    {
                        PDFFormat.Add(listBox1.Items[i].ToString(), "");
                    }
                }
            }
            // 加载模板通过服务
            classtemp = _templateService.LoadAll();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            // 验证并初始化 OCR 服务
            try
            {
                EnsureOcrService(textBox3.Text?.Trim() ?? "", textBox4.Text?.Trim() ?? "");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"初始化 OCR 失败: {ex.Message}", "错误");
                return;
            }

            // 读取模板
            classtemp = _templateService.LoadAll();
            if (classtemp.Count == 0)
            {
                MessageBox.Show("未找到模板，请先配置模板分类");
                return;
            }

            using var pdf = new Spire.Pdf.PdfDocument();
            //读取模板（通过服务）
            classtemp = _templateService.LoadAll();

            ExName.Clear(); PDFName.Clear();
            for (int i = 0; i < PDFdata.Rows.Count; i++)
            {
                var txt = new System.Text.StringBuilder();
                string path = PDFdata.Rows[i].Cells[1].Value.ToString();
                pdf.LoadFromFile(path);
                //文本化开始分类
                using (var doc = UglyToad.PdfPig.PdfDocument.Open(path))
                {
                    copyfile copyfile;
                    copyfile.path = path;
                    foreach (var page in doc.GetPages())
                    {
                        foreach (var w in page.GetWords(NearestNeighbourWordExtractor.Instance))
                            txt.Append(w.Text).Append(' ');
                    }
                    string tt = CleanPdfText(txt.ToString());
                    //分类
                    for (int j = 0; j < listBox1.Items.Count; j++)
                    {
                        if (tt.Contains(listBox1.Items[j].ToString()))
                        {
                            List<TemplateItem> templates = classtemp[listBox1.Items[j].ToString()];

                            using var image = pdf.SaveAsImage(0, PdfImageType.Bitmap, 600, 600);//图片化
                            List<string> strs = new List<string>();
                            for (int k = 0; k < templates.Count; k++)
                            {
                                string tempImagepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", listBox1.Items[j].ToString(), templates[k].ImageFile);
                                using var srcBmp = new Bitmap(image);
                                using var tempBmp = new Bitmap(tempImagepath);
                                using var bitmap = GetRectangle(srcBmp, tempBmp, templates[k], testMode.Checked);
                                // 使用服务层 OCR
                                strs.Add(_ocrService.Recognize(bitmap));
                            }
                            string Excelfomat = ExcelFormat[listBox1.Items[j].ToString()];
                            string Pdffomat = PDFFormat[listBox1.Items[j].ToString()];
                            try
                            {
                                //填入格式中
                                string ExcelFomat = string.Format(Excelfomat, strs.ToArray());
                                string pdffomat = string.Format(Pdffomat, strs.ToArray());

                                PDFdata.Rows[i].Selected = true;//界面上显示进度
                                ExName.Add(path, ExcelFomat);
                                PDFName.Add(path, pdffomat);
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show($"{listBox1.Items[j].ToString()}: 模板数量与格式填写的数量不相等,无法写入请正确填写");
                            }
                        }
                    }
                }
            }
            MessageBox.Show("识别结束");
        }
        Dictionary<string, string> ExName = new Dictionary<string, string>();
        Dictionary<string, string> PDFName = new Dictionary<string, string>();
        #region 运行OCR
        // 模板加载已移至 TemplateService
        /// <summary>
        /// 进行模板匹配 创建ROI后进行OCR识别
        /// </summary>
        public Bitmap GetRectangle(Bitmap image, Bitmap temp, TemplateItem template, bool test)
        {
            string[] strs = template.ROI.Split(',');
            OpenCvSharp.Point point = OpterCV.RetancROI(image, temp, test);
            Rectangle rectangle = new Rectangle(point.X + int.Parse(strs[0]), point.Y + int.Parse(strs[1]), int.Parse(strs[2]), int.Parse(strs[3]));
            return OpterCV.CropImage(image, rectangle);
        }
        /// <summary>
        /// 上传OCR
        /// </summary>
        public String RunOCR(Bitmap image)
        {
            // 旧方法保留但不再创建新实例，使用服务层 OCR
            return _ocrService.Recognize(image);
        }
        #endregion

        private void setTXTFomat_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                ExcelFormat[listBox1.SelectedItem.ToString()] = txtFomatbox.Text;
                PDFFormat[listBox1.SelectedItem.ToString()] = PDFfomat.Text;
                RefreshFormat();
            }
        }
        #region
        Dictionary<string, string> ExcelFormat = new Dictionary<string, string>();
        Dictionary<string, string> PDFFormat = new Dictionary<string, string>();

        string excelConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Config.txt");
        string pdfConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ConfigPDF.txt");
        /// <summary>
        /// 刷新并保存 Excel 和 PDF 输出格式配置
        /// </summary>
        private void RefreshFormat()
        {
            // 确保 Templates 目录存在（一次操作）
            string templatesDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            Directory.CreateDirectory(templatesDir);

            // 配置文件路径

            // 构建配置内容（Excel 格式与 PDF 文件名格式）
            var excelLines = new List<string>(listBox1.Items.Count);
            var pdfLines = new List<string>(listBox1.Items.Count);

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string key = listBox1.Items[i].ToString();
                string excelFormat = ExcelFormat.ContainsKey(key) ? ExcelFormat[key] : string.Empty;
                string pdfFormat = PDFFormat.ContainsKey(key) ? PDFFormat[key] : string.Empty;

                excelLines.Add($"{key}:{excelFormat}");
                pdfLines.Add($"{key}:{pdfFormat}");
            }

            // WriteAllLines 自动创建或覆盖文件，无需手动删除
            try
            {
                File.WriteAllLines(excelConfigPath, excelLines);
                File.WriteAllLines(pdfConfigPath, pdfLines);
                System.Diagnostics.Debug.WriteLine("配置文件已保存");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"保存配置文件失败: {ex.Message}", "错误");
            }
        }
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showFomat.Text = null;
            string key = listBox1.SelectedItem?.ToString();
            if (key == null) return;

            txtFomatbox.Text = ExcelFormat[key];
            PDFfomat.Text = PDFFormat[key];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(key + ": ");

            if (classtemp.TryGetValue(key, out List<TemplateItem> items))
            {
                for (int i = 0; i < items.Count; i++)
                {
                    sb.AppendLine($"{i} : {items[i].ClassName}");
                }
            }

            showFomat.Text = sb.ToString();  // 直接改 Text，不用 Lines
        }

        private async void outPDF_Click(object sender, EventArgs e)
        {
            if (PDFName == null || PDFName.Count == 0)
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            string path = textBox2.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("请选择目标目录");
                return;
            }

            try
            {
                outPDF.Enabled = false;
                // 在后台执行复制，避免阻塞 UI
                var result = await Task.Run(() => _exportService.CopyFiles(PDFName, path, overwrite: false));
                MessageBox.Show($"导出完成：成功 {result.success}，失败 {result.failed}");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出过程中出现错误：{ex.Message}");
            }
            finally
            {
                outPDF.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ExName.Count == 0)
            {
                MessageBox.Show("没有数据");
                return;
            }
            int max = 0;
            foreach (string s in ExcelFormat.Values)
            {
                max = s.Split('_').Length > max ? s.Split('_').Length : max;
            }
            ExcelShow excel = new ExcelShow(ExName, max);
            excel.ShowDialog();
        }
    }
    /// <summary>
    /// 模板根对象，用于从 JSON 反序列化模板列表
    /// </summary>
    public class TemplateRoot
    {
        public List<TemplateItem> Templates { get; set; }
    }
    /// <summary>
    /// 同步调用百度 OCR 服务的简易客户端（注意：建议将其改为异步并添加并发保护）
    /// </summary>
    // ========== 百度OCR同步客户端 ==========
    public class BaiduOcrSync
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        private string _accessToken = "";
        private DateTime _tokenExpireTime = DateTime.MinValue;
        private static readonly HttpClient _client = new HttpClient();

        public BaiduOcrSync(string apiKey, string secretKey)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;
        }

        /// <summary>
        /// 传入Bitmap，返回识别字符
        /// </summary>
        public string Recognize(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return RecognizeBytes(ms.ToArray());
            }
        }

        /// <summary>
        /// 传入图片路径，返回识别字符
        /// </summary>
        public string Recognize(string imagePath)
        {
            byte[] bytes = File.ReadAllBytes(imagePath);
            return RecognizeBytes(bytes);
        }

        /// <summary>
        /// 传入byte数组，返回识别字符
        /// </summary>
        public string RecognizeBytes(byte[] imageBytes)
        {
            string base64 = Convert.ToBase64String(imageBytes);
            return RecognizeBase64(base64);
        }

        private string RecognizeBase64(string base64Image)
        {
            // 获取有效Token
            string token = GetAccessToken();
            if (string.IsNullOrEmpty(token)) return "";

            string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token={token}";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("image", base64Image)
            });
            var response = _client.PostAsync(url, content).GetAwaiter().GetResult();//发送请求
            string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            return ParseText(json);
        }

        private string ParseText(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // 错误处理
                if (root.TryGetProperty("error_code", out _))
                {
                    System.Diagnostics.Debug.WriteLine($"OCR错误: {root.GetProperty("error_msg").GetString()}");
                    return "";
                }

                // 拼接字符
                var sb = new StringBuilder();
                foreach (var item in root.GetProperty("words_result").EnumerateArray())
                {
                    sb.Append(item.GetProperty("words").GetString());
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析失败: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// 获取百度Access Token（带缓存）
        /// </summary>
        private string GetAccessToken()
        {
            // 缓存有效直接返回
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpireTime)
            {
                return _accessToken;
            }

            try
            {
                string url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={_apiKey}&client_secret={_secretKey}";
                var response = _client.GetStringAsync(url).GetAwaiter().GetResult();
                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                _accessToken = root.GetProperty("access_token").GetString();
                int expiresIn = root.GetProperty("expires_in").GetInt32();

                // 提前1小时过期
                _tokenExpireTime = DateTime.Now.AddSeconds(expiresIn - 3600);

                System.Diagnostics.Debug.WriteLine($"Token获取成功，有效期{expiresIn}秒");
                return _accessToken;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取Token失败: {ex.Message}");
                return "";
            }
        }
    }
}

/// <summary>
/// 简单的文件路径结构体（用于临时传递路径）
/// </summary>
public struct copyfile
{
    public string path;
    public string toname;
}
