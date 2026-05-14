using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 发票
{
    public partial class ExcelShow : Form
    {
        public string exelpath;
        List<string> _data = new List<string>();
        int len = 9;
        public ExcelShow(Dictionary<string, string> dd, int length)
        {
            InitializeComponent();
            foreach (string item in dd.Values)
            {
                _data.Add(item);
            }
            len = length > len ? length + 2 : 9;
        }
        private void ExcelShow_Load(object sender, EventArgs e)
        {
            Creation(len);
        }
        /// <summary>
        /// 创建行列数量
        /// </summary>
        /// <param name="with"></param>
        public void Creation(int with)
        {
            for (int i = 0; i < with; i++)
            {
                dataGridView1.Columns.Add($"类型{i}", $"类型{i}");
            }
            for (int i = 0; i < _data.Count; i++)
            {
                string[] str = _data[i].Split('_');
                dataGridView1.Rows.Add(str);
            }
            dataGridView1.Rows.Add("");
            dataGridView1.Rows.Add("");
        }
        int startRow = 13;      // 数据开始行（模板行）
        int startCol = 1;       // 数据开始列
        int templateRow = 13;   // 模板所在行（用于复制格式）
        private void InputExcel_Click(object sender, EventArgs e)
        {
            if (exelpath == null) return;

            ExcelPackage.License.SetNonCommercialPersonal("-继续睡");
            var exfile = new FileInfo(exelpath);
            package = new ExcelPackage(exfile);
            ws = package.Workbook.Worksheets[0];

            startRow = int.Parse(textBox1.Text);      // 数据插入起始行
            startCol = int.Parse(textBox2.Text);      // 数据起始列
            templateRow = startRow;                    // 假设模板在 startRow 行
            InsertData();
            MessageBox.Show("写入完成");
        }
        ExcelWorksheet ws;
        ExcelPackage package;
        /// <summary>
        /// 插入表格
        /// </summary>
        /// <param name="str"></param>
        public void InsertData()
        {
            if (ws == null || _data == null || _data.Count == 0) return;

            // 从最后一行开始往前插入，这样前面的行号不会变
            for (int i = _data.Count - 1; i >= 0; i--)
            {
                int insertRow = startRow + 1;  // 始终在模板行下方插入

                // 插入新行（推挤下面的行往下，不覆盖）
                ws.InsertRow(insertRow, 1);

                // 复制模板格式
                ws.Cells[templateRow, 1, templateRow, ws.Dimension.End.Column]
                  .Copy(ws.Cells[insertRow, 1, insertRow, ws.Dimension.End.Column]);

                // 填入数据
                string[] parts = _data[i].Split('_');
                parts = new[] { i.ToString() }.Concat(parts).ToArray();
                for (int j = 0; j < parts.Length; j++)
                {
                    ws.SetValue(insertRow, startCol + j, parts[j]);
                }
            }

            package.Save();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = fileDialog.FileName;
                exelpath = fileDialog.FileName;
            }
        }
    }
}
