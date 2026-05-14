using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using static 发票.MakeModes;

namespace 发票
{
    // ========== 数据模型 ==========
    public class TemplateConfig
    {
        public List<TemplateItem> Templates { get; set; } = new List<TemplateItem>();
    }

    public class TemplateItem
    {
        public string ClassName { get; set; }
        public string ImageFile { get; set; }
        public string ROI { get; set; }
    }

    // ========== 配置管理器 ==========
    public static class TemplateConfigManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static void Save(string configPath, List<TemplateInfo> templates)
        {
            string baseDir = Path.GetDirectoryName(configPath);
            string imageDir = Path.Combine(baseDir, "images");
            Directory.CreateDirectory(imageDir);

            var config = new TemplateConfig();

            for (int i = 0; i < templates.Count; i++)
            {
                var tpl = templates[i];
                string imgName = $"template_{i:D3}.png";
                string imgPath = Path.Combine(imageDir, imgName);

                tpl.TemplateImage?.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);

                config.Templates.Add(new TemplateItem
                {
                    ClassName = tpl.ClassName,
                    ImageFile = $"images/{imgName}",
                    ROI = tpl.ROI
                });
            }

            string json = JsonSerializer.Serialize(config, JsonOptions);
            File.WriteAllText(configPath, json);
        }

        public static List<TemplateInfo> Load(string configPath)
        {
            string baseDir = Path.GetDirectoryName(configPath);
            string json = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<TemplateConfig>(json, JsonOptions);

            var result = new List<TemplateInfo>();

            foreach (var item in config.Templates)
            {
                string imgPath = Path.Combine(baseDir, item.ImageFile);
                Image img = File.Exists(imgPath) ? Image.FromFile(imgPath) : null;

                result.Add(new TemplateInfo
                {
                    ClassName = item.ClassName,
                    Image = img,
                    TemplateImage = img,
                    ROI = item.ROI
                });
            }

            return result;
        }
    }
}
