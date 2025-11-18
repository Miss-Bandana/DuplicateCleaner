namespace Duplicate_Cleaner
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Export = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.LoadEntities = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SearchRecords = new System.Windows.Forms.ToolStripButton();
            this.ExportButton = new System.Windows.Forms.ToolStripButton();
            this.Clearoldest = new System.Windows.Forms.ToolStripButton();
            this.ClearLatest = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteLastRow = new System.Windows.Forms.Button();
            this.btnAddFilter = new System.Windows.Forms.Button();
            this.SelectfieldsGrid = new System.Windows.Forms.DataGridView();
            this.LogicalName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Criteria = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NumberofCharacters = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SelectEntities = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DuplicateSetsGrid = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PotentialsDuplecatesGrid = new System.Windows.Forms.DataGridView();
            this.Export.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SelectfieldsGrid)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DuplicateSetsGrid)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PotentialsDuplecatesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Export
            // 
            this.Export.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Export.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.LoadEntities,
            this.tssSeparator1,
            this.SearchRecords,
            this.ExportButton,
            this.Clearoldest,
            this.ClearLatest});
            this.Export.Location = new System.Drawing.Point(0, 0);
            this.Export.Name = "Export";
            this.Export.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.Export.Size = new System.Drawing.Size(1615, 31);
            this.Export.TabIndex = 4;
            this.Export.Text = "toolStrip1";
            this.Export.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Export_ItemClicked);
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::Duplicate_Cleaner.Properties.Resources.ico_Close;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(73, 28);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // LoadEntities
            // 
            this.LoadEntities.Image = global::Duplicate_Cleaner.Properties.Resources.ico_16_0;
            this.LoadEntities.ImageTransparentColor = System.Drawing.Color.Black;
            this.LoadEntities.Name = "LoadEntities";
            this.LoadEntities.Size = new System.Drawing.Size(122, 28);
            this.LoadEntities.Text = "Load Entities";
            this.LoadEntities.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LoadEntities.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // SearchRecords
            // 
            this.SearchRecords.Image = global::Duplicate_Cleaner.Properties.Resources.search_icon;
            this.SearchRecords.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchRecords.Name = "SearchRecords";
            this.SearchRecords.Size = new System.Drawing.Size(155, 28);
            this.SearchRecords.Text = "Serach Duplicates";
            this.SearchRecords.Click += new System.EventHandler(this.SearchRecords_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Image = global::Duplicate_Cleaner.Properties.Resources.ico_16_9507_Excel;
            this.ExportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(137, 28);
            this.ExportButton.Text = "Export Records";
            this.ExportButton.ToolTipText = "Export Duplicates Records To Csv";
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // Clearoldest
            // 
            this.Clearoldest.Image = global::Duplicate_Cleaner.Properties.Resources.icons8_delete_64;
            this.Clearoldest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Clearoldest.Name = "Clearoldest";
            this.Clearoldest.Size = new System.Drawing.Size(175, 28);
            this.Clearoldest.Text = "Clear Oldest Records";
            this.Clearoldest.ToolTipText = "Keep Latest One\'s And Delete Oldest One ";
            this.Clearoldest.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // ClearLatest
            // 
            this.ClearLatest.Image = global::Duplicate_Cleaner.Properties.Resources.icons8_delete_50;
            this.ClearLatest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearLatest.Name = "ClearLatest";
            this.ClearLatest.Size = new System.Drawing.Size(171, 28);
            this.ClearLatest.Text = "Clear Latest Records";
            this.ClearLatest.ToolTipText = "Keep Oldest One And Delete latest One\'s";
            this.ClearLatest.Click += new System.EventHandler(this.ClearLatest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteLastRow);
            this.groupBox1.Controls.Add(this.btnAddFilter);
            this.groupBox1.Controls.Add(this.SelectfieldsGrid);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SelectEntities);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 43);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(1615, 422);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detection Rule ";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnDeleteLastRow
            // 
            this.btnDeleteLastRow.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnDeleteLastRow.Location = new System.Drawing.Point(759, 358);
            this.btnDeleteLastRow.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteLastRow.Name = "btnDeleteLastRow";
            this.btnDeleteLastRow.Size = new System.Drawing.Size(57, 31);
            this.btnDeleteLastRow.TabIndex = 13;
            this.btnDeleteLastRow.Text = "-Del";
            this.btnDeleteLastRow.UseVisualStyleBackColor = false;
            this.btnDeleteLastRow.Click += new System.EventHandler(this.btnDeleteLastRow_Click);
            // 
            // btnAddFilter
            // 
            this.btnAddFilter.Location = new System.Drawing.Point(759, 329);
            this.btnAddFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddFilter.Name = "btnAddFilter";
            this.btnAddFilter.Size = new System.Drawing.Size(55, 23);
            this.btnAddFilter.TabIndex = 12;
            this.btnAddFilter.Text = "+Add";
            this.btnAddFilter.UseVisualStyleBackColor = true;
            this.btnAddFilter.Click += new System.EventHandler(this.button1_Click);
            // 
            // SelectfieldsGrid
            // 
            this.SelectfieldsGrid.AllowUserToAddRows = false;
            this.SelectfieldsGrid.AllowUserToResizeColumns = false;
            this.SelectfieldsGrid.AllowUserToResizeRows = false;
            this.SelectfieldsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SelectfieldsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LogicalName,
            this.Criteria,
            this.NumberofCharacters});
            this.SelectfieldsGrid.Location = new System.Drawing.Point(7, 101);
            this.SelectfieldsGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SelectfieldsGrid.Name = "SelectfieldsGrid";
            this.SelectfieldsGrid.RowHeadersVisible = false;
            this.SelectfieldsGrid.RowHeadersWidth = 51;
            this.SelectfieldsGrid.RowTemplate.Height = 24;
            this.SelectfieldsGrid.Size = new System.Drawing.Size(829, 319);
            this.SelectfieldsGrid.TabIndex = 11;
            this.SelectfieldsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // LogicalName
            // 
            this.LogicalName.FillWeight = 280.7487F;
            this.LogicalName.HeaderText = "Logical Name";
            this.LogicalName.MinimumWidth = 8;
            this.LogicalName.Name = "LogicalName";
            this.LogicalName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LogicalName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LogicalName.Width = 330;
            // 
            // Criteria
            // 
            this.Criteria.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Criteria.FillWeight = 9.625671F;
            this.Criteria.HeaderText = "Criteria";
            this.Criteria.Items.AddRange(new object[] {
            "Exact Match",
            "Same First Characters",
            "Same Last Characters"});
            this.Criteria.MinimumWidth = 6;
            this.Criteria.Name = "Criteria";
            // 
            // NumberofCharacters
            // 
            this.NumberofCharacters.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NumberofCharacters.FillWeight = 9.625671F;
            this.NumberofCharacters.HeaderText = "Number of Characters";
            this.NumberofCharacters.MinimumWidth = 6;
            this.NumberofCharacters.Name = "NumberofCharacters";
            this.NumberofCharacters.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NumberofCharacters.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(927, 44);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(419, 208);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Text Comparison Criteria ";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(5, 59);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(159, 20);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Exclude Blank Values";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CausesValidation = false;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(5, 33);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(189, 20);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Trim Unnecessary Spaces";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Choose Fields ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // SelectEntities
            // 
            this.SelectEntities.FormattingEnabled = true;
            this.SelectEntities.Location = new System.Drawing.Point(4, 44);
            this.SelectEntities.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SelectEntities.Name = "SelectEntities";
            this.SelectEntities.Size = new System.Drawing.Size(833, 24);
            this.SelectEntities.TabIndex = 8;
            this.SelectEntities.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Select Entity       ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DuplicateSetsGrid);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(7, 485);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(1608, 299);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Duplicate Sets";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // DuplicateSetsGrid
            // 
            this.DuplicateSetsGrid.AllowUserToAddRows = false;
            this.DuplicateSetsGrid.AllowUserToResizeColumns = false;
            this.DuplicateSetsGrid.AllowUserToResizeRows = false;
            this.DuplicateSetsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DuplicateSetsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DuplicateSetsGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.DuplicateSetsGrid.Location = new System.Drawing.Point(8, 21);
            this.DuplicateSetsGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DuplicateSetsGrid.MultiSelect = false;
            this.DuplicateSetsGrid.Name = "DuplicateSetsGrid";
            this.DuplicateSetsGrid.ReadOnly = true;
            this.DuplicateSetsGrid.RowHeadersVisible = false;
            this.DuplicateSetsGrid.RowHeadersWidth = 51;
            this.DuplicateSetsGrid.RowTemplate.Height = 24;
            this.DuplicateSetsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DuplicateSetsGrid.Size = new System.Drawing.Size(520, 273);
            this.DuplicateSetsGrid.TabIndex = 1;
            this.DuplicateSetsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.DuplicateSetsGrid.SelectionChanged += new System.EventHandler(this.DuplicateSetsGrid_SelectionChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PotentialsDuplecatesGrid);
            this.groupBox3.Location = new System.Drawing.Point(577, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(1028, 299);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Potential Duplicates";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // PotentialsDuplecatesGrid
            // 
            this.PotentialsDuplecatesGrid.AllowUserToAddRows = false;
            this.PotentialsDuplecatesGrid.AllowUserToResizeColumns = false;
            this.PotentialsDuplecatesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.PotentialsDuplecatesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PotentialsDuplecatesGrid.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.PotentialsDuplecatesGrid.Location = new System.Drawing.Point(7, 21);
            this.PotentialsDuplecatesGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PotentialsDuplecatesGrid.Name = "PotentialsDuplecatesGrid";
            this.PotentialsDuplecatesGrid.RowHeadersVisible = false;
            this.PotentialsDuplecatesGrid.RowHeadersWidth = 51;
            this.PotentialsDuplecatesGrid.RowTemplate.Height = 24;
            this.PotentialsDuplecatesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PotentialsDuplecatesGrid.Size = new System.Drawing.Size(619, 273);
            this.PotentialsDuplecatesGrid.TabIndex = 0;
            this.PotentialsDuplecatesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PotentialsDuplecatesGrid_CellContentClick);
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Export);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1615, 786);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.Export.ResumeLayout(false);
            this.Export.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SelectfieldsGrid)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DuplicateSetsGrid)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PotentialsDuplecatesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip Export;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripButton LoadEntities;
        private System.Windows.Forms.ToolStripButton ExportButton;
        private System.Windows.Forms.ToolStripButton SearchRecords;
        private System.Windows.Forms.ToolStripButton Clearoldest;
        private System.Windows.Forms.ToolStripButton ClearLatest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SelectEntities;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView SelectfieldsGrid;
        private System.Windows.Forms.DataGridView DuplicateSetsGrid;
        private System.Windows.Forms.DataGridView PotentialsDuplecatesGrid;
        private System.Windows.Forms.Button btnAddFilter;
        private System.Windows.Forms.DataGridViewComboBoxColumn LogicalName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Criteria;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberofCharacters;
        private System.Windows.Forms.Button btnDeleteLastRow;
    }
}
