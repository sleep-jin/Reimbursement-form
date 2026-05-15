namespace 发票
{
    /// <summary>
    /// 模板根对象，用于从 JSON 反序列化模板列表
    /// </summary>
    public class TemplateRoot
    {
        public List<TemplateItem> Templates { get; set; } = new List<TemplateItem>();
    }

    /// <summary>
    /// 简单的文件路径结构体（用于临时传递路径）
    /// </summary>
    public struct copyfile
    {
        public string path;
        public string toname;
    }
}
