using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace 发票
{
    /// <summary>
    /// 负责模板加载与解析
    /// </summary>
    public class TemplateService
    {
        private readonly string _templatesDir;

        public TemplateService(string templatesDir)
        {
            _templatesDir = templatesDir;
        }

        public Dictionary<string, List<TemplateItem>> LoadAll()
        {
            var result = new Dictionary<string, List<TemplateItem>>();
            if (!Directory.Exists(_templatesDir)) return result;

            foreach (var folder in Directory.GetDirectories(_templatesDir))
            {
                string name = Path.GetFileName(folder);
                string jsonPath = Path.Combine(folder, $"{name}.json");
                if (!File.Exists(jsonPath)) continue;
                string json = File.ReadAllText(jsonPath);
                var root = JsonSerializer.Deserialize<TemplateConfig>(json);
                if (root?.Templates != null)
                {
                    result[name] = root.Templates;
                }
            }

            return result;
        }
    }
}
