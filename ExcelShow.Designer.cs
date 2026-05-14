namespace 发票
{
    partial class ExcelShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            InputExcel = new Button();
            button3 = new Button();
            textBox4 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(1330, 625);
            dataGridView1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(118, 645);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(80, 30);
            textBox1.TabIndex = 1;
            textBox1.Text = "12";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 648);
            label1.Name = "label1";
            label1.Size = new Size(100, 24);
            label1.TabIndex = 2;
            label1.Text = "导入起始行";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(213, 648);
            label2.Name = "label2";
            label2.Size = new Size(100, 24);
            label2.TabIndex = 4;
            label2.Text = "导入起始列";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(319, 645);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(80, 30);
            textBox2.TabIndex = 3;
            textBox2.Text = "1";
            // 
            // InputExcel
            // 
            InputExcel.Location = new Point(444, 645);
            InputExcel.Margin = new Padding(5, 4, 5, 4);
            InputExcel.Name = "InputExcel";
            InputExcel.Size = new Size(143, 32);
            InputExcel.TabIndex = 18;
            InputExcel.Text = "导入表格";
            InputExcel.UseVisualStyleBackColor = true;
            InputExcel.Click += InputExcel_Click;
            // 
            // button3
            // 
            button3.Location = new Point(1078, 645);
            button3.Margin = new Padding(5, 4, 5, 4);
            button3.Name = "button3";
            button3.Size = new Size(143, 32);
            button3.TabIndex = 20;
            button3.Text = "选择表格";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(597, 648);
            textBox4.Margin = new Padding(5, 4, 5, 4);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(471, 30);
            textBox4.TabIndex = 19;
            // 
            // ExcelShow
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1336, 710);
            Controls.Add(button3);
            Controls.Add(textBox4);
            Controls.Add(InputExcel);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(dataGridView1);
            Name = "ExcelShow";
            Text = "ExcelShow";
            Load += ExcelShow_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
        private Button InputExcel;
        private Button button3;
        private TextBox textBox4;
    }
}