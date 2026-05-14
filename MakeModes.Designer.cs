namespace 发票
{
    partial class MakeModes
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
            add = new Button();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewImageColumn();
            classname = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            SaveMode = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // add
            // 
            add.Location = new Point(1509, 6);
            add.Name = "add";
            add.Size = new Size(112, 39);
            add.TabIndex = 1;
            add.Text = "添加模板";
            add.UseVisualStyleBackColor = true;
            add.Click += add_Click;
            // 
            // button1
            // 
            button1.Location = new Point(1653, 6);
            button1.Name = "button1";
            button1.Size = new Size(112, 39);
            button1.TabIndex = 3;
            button1.Text = "删除模板";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, classname, Column2 });
            dataGridView1.Location = new Point(1492, 57);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(712, 1011);
            dataGridView1.TabIndex = 4;
            // 
            // Column1
            // 
            Column1.HeaderText = "图片";
            Column1.MinimumWidth = 8;
            Column1.Name = "Column1";
            Column1.Width = 300;
            // 
            // classname
            // 
            classname.HeaderText = "分类";
            classname.MinimumWidth = 8;
            classname.Name = "classname";
            classname.Resizable = DataGridViewTriState.True;
            classname.SortMode = DataGridViewColumnSortMode.NotSortable;
            classname.Width = 150;
            // 
            // Column2
            // 
            Column2.HeaderText = "ROI";
            Column2.MinimumWidth = 8;
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.True;
            Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column2.Width = 200;
            // 
            // groupBox1
            // 
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1466, 1071);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // SaveMode
            // 
            SaveMode.Location = new Point(1806, 6);
            SaveMode.Name = "SaveMode";
            SaveMode.Size = new Size(112, 39);
            SaveMode.TabIndex = 6;
            SaveMode.Text = "保存";
            SaveMode.UseVisualStyleBackColor = true;
            SaveMode.Click += SaveMode_Click;
            // 
            // MakeModes
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2208, 1080);
            Controls.Add(SaveMode);
            Controls.Add(groupBox1);
            Controls.Add(dataGridView1);
            Controls.Add(button1);
            Controls.Add(add);
            Name = "MakeModes";
            Text = "MakeModes";
            Load += MakeModes_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button add;
        private Button button1;
        private DataGridView dataGridView1;
        private GroupBox groupBox1;
        private Button SaveMode;
        private DataGridViewImageColumn Column1;
        private DataGridViewTextBoxColumn classname;
        private DataGridViewTextBoxColumn Column2;
    }
}