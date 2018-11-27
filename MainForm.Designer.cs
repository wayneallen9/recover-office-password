namespace RecoverExcelPassword
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanelWindows = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewWindows = new System.Windows.Forms.DataGridView();
            this.titleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.windowsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceForm = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanelWindows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWindows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceForm)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanelWindows);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(297, 201);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanelWindows
            // 
            this.tableLayoutPanelWindows.ColumnCount = 1;
            this.tableLayoutPanelWindows.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelWindows.Controls.Add(this.dataGridViewWindows, 0, 0);
            this.tableLayoutPanelWindows.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanelWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelWindows.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanelWindows.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelWindows.Name = "tableLayoutPanelWindows";
            this.tableLayoutPanelWindows.RowCount = 2;
            this.tableLayoutPanelWindows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelWindows.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelWindows.Size = new System.Drawing.Size(277, 181);
            this.tableLayoutPanelWindows.TabIndex = 0;
            // 
            // dataGridViewWindows
            // 
            this.dataGridViewWindows.AllowUserToAddRows = false;
            this.dataGridViewWindows.AllowUserToDeleteRows = false;
            this.dataGridViewWindows.AutoGenerateColumns = false;
            this.dataGridViewWindows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWindows.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.titleDataGridViewTextBoxColumn});
            this.dataGridViewWindows.DataSource = this.windowsBindingSource;
            this.dataGridViewWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewWindows.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewWindows.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWindows.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewWindows.Name = "dataGridViewWindows";
            this.dataGridViewWindows.ReadOnly = true;
            this.dataGridViewWindows.RowHeadersVisible = false;
            this.dataGridViewWindows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewWindows.Size = new System.Drawing.Size(277, 158);
            this.dataGridViewWindows.TabIndex = 0;
            this.dataGridViewWindows.SelectionChanged += new System.EventHandler(this.DataGridViewWindows_SelectionChanged);
            // 
            // titleDataGridViewTextBoxColumn
            // 
            this.titleDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.titleDataGridViewTextBoxColumn.DataPropertyName = "Title";
            this.titleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.titleDataGridViewTextBoxColumn.Name = "titleDataGridViewTextBoxColumn";
            this.titleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // windowsBindingSource
            // 
            this.windowsBindingSource.DataMember = "Windows";
            this.windowsBindingSource.DataSource = this.bindingSourceForm;
            // 
            // bindingSourceForm
            // 
            this.bindingSourceForm.DataSource = typeof(RecoverExcelPassword.MainFormViewModel);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonStart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 161);
            this.panel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(277, 20);
            this.panel2.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.bindingSourceForm, "CanStart", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStart.Location = new System.Drawing.Point(202, 0);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 20);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "&Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 201);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Recover Office Passwords";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanelWindows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWindows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceForm)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bindingSourceForm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelWindows;
        private System.Windows.Forms.DataGridView dataGridViewWindows;
        private System.Windows.Forms.DataGridViewTextBoxColumn titleDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource windowsBindingSource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonStart;
    }
}

