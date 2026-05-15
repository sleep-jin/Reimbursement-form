
namespace 发票
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            textBox1 = new TextBox();
            button1 = new Button();
            PDFdata = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Fullname = new DataGridViewTextBoxColumn();
            classname = new DataGridViewTextBoxColumn();
            MakeMode = new Button();
            listBox1 = new ListBox();
            编辑分类 = new Button();
            Start = new Button();
            txtFomatbox = new TextBox();
            setTXTFomat = new Button();
            testMode = new CheckBox();
            showFomat = new TextBox();
            outPDF = new Button();
            button4 = new Button();
            button2 = new Button();
            textBox2 = new TextBox();
            label1 = new Label();
            PDFfomat = new TextBox();
            label2 = new Label();
            label3 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            label4 = new Label();
            label5 = new Label();
            progressBar1 = new ProgressBar();
            lblProgress = new Label();
            ((System.ComponentModel.ISupportInitialize)PDFdata).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(14, 15);
            textBox1.Margin = new Padding(5, 4, 5, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(471, 30);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(495, 13);
            button1.Margin = new Padding(5, 4, 5, 4);
            button1.Name = "button1";
            button1.Size = new Size(143, 32);
            button1.TabIndex = 1;
            button1.Text = "选择发票文件夹";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // PDFdata
            // 
            PDFdata.AllowUserToAddRows = false;
            PDFdata.AllowUserToDeleteRows = false;
            PDFdata.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PDFdata.Columns.AddRange(new DataGridViewColumn[] { Column1, Fullname, classname });
            PDFdata.Location = new Point(660, 12);
            PDFdata.Name = "PDFdata";
            PDFdata.RowHeadersWidth = 62;
            PDFdata.Size = new Size(512, 645);
            PDFdata.TabIndex = 7;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column1.DefaultCellStyle = dataGridViewCellStyle1;
            Column1.HeaderText = "文件名";
            Column1.MinimumWidth = 8;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Fullname
            // 
            Fullname.HeaderText = "路径";
            Fullname.MinimumWidth = 8;
            Fullname.Name = "Fullname";
            Fullname.Visible = false;
            Fullname.Width = 150;
            // 
            // classname
            // 
            classname.HeaderText = "类别";
            classname.MinimumWidth = 8;
            classname.Name = "classname";
            classname.Visible = false;
            classname.Width = 150;
            // 
            // MakeMode
            // 
            MakeMode.Location = new Point(14, 132);
            MakeMode.Margin = new Padding(5, 4, 5, 4);
            MakeMode.Name = "MakeMode";
            MakeMode.Size = new Size(143, 32);
            MakeMode.TabIndex = 8;
            MakeMode.Text = "制作模板";
            MakeMode.UseVisualStyleBackColor = true;
            MakeMode.Click += MakeMode_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(14, 171);
            listBox1.MultiColumn = true;
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(143, 268);
            listBox1.TabIndex = 9;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // 编辑分类
            // 
            编辑分类.Location = new Point(14, 446);
            编辑分类.Margin = new Padding(5, 4, 5, 4);
            编辑分类.Name = "编辑分类";
            编辑分类.Size = new Size(143, 33);
            编辑分类.TabIndex = 10;
            编辑分类.Text = "编辑分类";
            编辑分类.UseVisualStyleBackColor = true;
            编辑分类.Click += 编辑分类_Click;
            // 
            // Start
            // 
            Start.Location = new Point(495, 132);
            Start.Margin = new Padding(5, 4, 5, 4);
            Start.Name = "Start";
            Start.Size = new Size(143, 32);
            Start.TabIndex = 11;
            Start.Text = " 开始运行";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // txtFomatbox
            // 
            txtFomatbox.Location = new Point(275, 172);
            txtFomatbox.Margin = new Padding(5, 4, 5, 4);
            txtFomatbox.Name = "txtFomatbox";
            txtFomatbox.Size = new Size(363, 30);
            txtFomatbox.TabIndex = 12;
            txtFomatbox.Text = "打车- {0}_{1}-{2}_{3}";
            // 
            // setTXTFomat
            // 
            setTXTFomat.Location = new Point(165, 244);
            setTXTFomat.Margin = new Padding(5, 4, 5, 4);
            setTXTFomat.Name = "setTXTFomat";
            setTXTFomat.Size = new Size(171, 33);
            setTXTFomat.TabIndex = 13;
            setTXTFomat.Text = "设置格式";
            setTXTFomat.UseVisualStyleBackColor = true;
            setTXTFomat.Click += setTXTFomat_Click;
            // 
            // testMode
            // 
            testMode.AutoSize = true;
            testMode.Location = new Point(377, 136);
            testMode.Name = "testMode";
            testMode.Size = new Size(108, 28);
            testMode.TabIndex = 14;
            testMode.Text = "调试模板";
            testMode.UseVisualStyleBackColor = true;
            // 
            // showFomat
            // 
            showFomat.Location = new Point(165, 294);
            showFomat.Margin = new Padding(5, 4, 5, 4);
            showFomat.Multiline = true;
            showFomat.Name = "showFomat";
            showFomat.ReadOnly = true;
            showFomat.Size = new Size(473, 145);
            showFomat.TabIndex = 15;
            showFomat.Text = "  ";
            // 
            // outPDF
            // 
            outPDF.Location = new Point(167, 447);
            outPDF.Margin = new Padding(5, 4, 5, 4);
            outPDF.Name = "outPDF";
            outPDF.Size = new Size(143, 32);
            outPDF.TabIndex = 16;
            outPDF.Text = "导出新PDF";
            outPDF.UseVisualStyleBackColor = true;
            outPDF.Click += outPDF_Click;
            // 
            // button4
            // 
            button4.Location = new Point(495, 447);
            button4.Margin = new Padding(5, 4, 5, 4);
            button4.Name = "button4";
            button4.Size = new Size(143, 32);
            button4.TabIndex = 18;
            button4.Text = "预览表格";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button2
            // 
            button2.Location = new Point(495, 51);
            button2.Margin = new Padding(5, 4, 5, 4);
            button2.Name = "button2";
            button2.Size = new Size(143, 32);
            button2.TabIndex = 2;
            button2.Text = "选择导出的位置";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(14, 53);
            textBox2.Margin = new Padding(5, 4, 5, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(471, 30);
            textBox2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(421, 248);
            label1.Name = "label1";
            label1.Size = new Size(172, 24);
            label1.TabIndex = 19;
            label1.Text = "根据模板显示格式：";
            // 
            // PDFfomat
            // 
            PDFfomat.Location = new Point(275, 206);
            PDFfomat.Margin = new Padding(5, 4, 5, 4);
            PDFfomat.Name = "PDFfomat";
            PDFfomat.Size = new Size(363, 30);
            PDFfomat.TabIndex = 20;
            PDFfomat.Text = "打车- {0}_{1}-{2}_{3}";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(167, 172);
            label2.Name = "label2";
            label2.Size = new Size(100, 24);
            label2.TabIndex = 21;
            label2.Text = "表格的格式";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(168, 206);
            label3.Name = "label3";
            label3.Size = new Size(99, 24);
            label3.TabIndex = 22;
            label3.Text = "PDF的格式";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(14, 496);
            textBox3.Margin = new Padding(5, 4, 5, 4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(363, 30);
            textBox3.TabIndex = 23;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(14, 534);
            textBox4.Margin = new Padding(5, 4, 5, 4);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(363, 30);
            textBox4.TabIndex = 24;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(395, 502);
            label4.Name = "label4";
            label4.Size = new Size(143, 24);
            label4.TabIndex = 25;
            label4.Text = "BAIDU_API_KEY";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(395, 540);
            label5.Name = "label5";
            label5.Size = new Size(178, 24);
            label5.TabIndex = 26;
            label5.Text = "BAIDU_SECRET_KEY";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(14, 580);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(471, 25);
            progressBar1.TabIndex = 27;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(495, 582);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(82, 24);
            lblProgress.TabIndex = 28;
            lblProgress.Text = "0 / 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1197, 669);
            Controls.Add(label5);
            Controls.Add(progressBar1);
            Controls.Add(lblProgress);
            Controls.Add(label4);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(PDFfomat);
            Controls.Add(label1);
            Controls.Add(button4);
            Controls.Add(outPDF);
            Controls.Add(showFomat);
            Controls.Add(testMode);
            Controls.Add(setTXTFomat);
            Controls.Add(txtFomatbox);
            Controls.Add(Start);
            Controls.Add(编辑分类);
            Controls.Add(listBox1);
            Controls.Add(MakeMode);
            Controls.Add(PDFdata);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Margin = new Padding(5, 4, 5, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)PDFdata).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private DataGridView PDFdata;
        private Button MakeMode;
        private ListBox listBox1;
        private Button 编辑分类;
        private Button Start;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Fullname;
        private DataGridViewTextBoxColumn classname;
        private TextBox txtFomatbox;
        private Button setTXTFomat;
        private CheckBox testMode;
        private TextBox showFomat;
        private Button outPDF;
        private Button button4;
        private Button button2;
        private TextBox textBox2;
        private Label label1;
        private TextBox PDFfomat;
        private Label label2;
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label4;
        private Label label5;
        private ProgressBar progressBar1;
        private Label lblProgress;
    }
}
