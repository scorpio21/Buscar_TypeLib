namespace TypeLibExporter_NET8
{
    partial class ListarJson
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListarJson));
            panelTop = new Panel();
            lblInfo = new Label();
            lblTitle = new Label();
            panelMain = new Panel();
            splitContainer = new SplitContainer();
            panelList = new Panel();
            lstLibraries = new ListBox();
            lblSearchResults = new Label();
            panelSearch = new Panel();
            btnClearSearch = new Button();
            txtSearch = new TextBox();
            lblSearch = new Label();
            panelListButtons = new Panel();
            btnEdit = new Button();
            btnDelete = new Button();
            lblListTitle = new Label();
            txtJsonDisplay = new TextBox();
            panelButtons = new Panel();
            btnClose = new Button();
            btnAddNew = new Button();
            btnSave = new Button();
            btnCopy = new Button();
            panelTop.SuspendLayout();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelList.SuspendLayout();
            panelSearch.SuspendLayout();
            panelListButtons.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(37, 99, 235);
            panelTop.Controls.Add(lblInfo);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(4, 5, 4, 5);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(29, 33, 29, 33);
            panelTop.Size = new Size(1714, 133);
            panelTop.TabIndex = 0;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Segoe UI", 10F);
            lblInfo.ForeColor = Color.FromArgb(200, 220, 255);
            lblInfo.Location = new Point(29, 83);
            lblInfo.Margin = new Padding(4, 0, 4, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(578, 28);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "Editor completo con buscador en tiempo real para filtrar librerías";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(29, 25);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(466, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📝 Editor JSON con Buscador";
            // 
            // panelMain
            // 
            panelMain.Controls.Add(splitContainer);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 133);
            panelMain.Margin = new Padding(4, 5, 4, 5);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(14, 17, 14, 17);
            panelMain.Size = new Size(1714, 901);
            panelMain.TabIndex = 1;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(14, 17);
            splitContainer.Margin = new Padding(4, 5, 4, 5);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelList);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(txtJsonDisplay);
            splitContainer.Size = new Size(1686, 867);
            splitContainer.SplitterDistance = 642;
            splitContainer.SplitterWidth = 6;
            splitContainer.TabIndex = 0;
            // 
            // panelList
            // 
            panelList.BackColor = Color.White;
            panelList.BorderStyle = BorderStyle.FixedSingle;
            panelList.Controls.Add(lstLibraries);
            panelList.Controls.Add(lblSearchResults);
            panelList.Controls.Add(panelSearch);
            panelList.Controls.Add(panelListButtons);
            panelList.Controls.Add(lblListTitle);
            panelList.Dock = DockStyle.Fill;
            panelList.Location = new Point(0, 0);
            panelList.Margin = new Padding(4, 5, 4, 5);
            panelList.Name = "panelList";
            panelList.Size = new Size(642, 867);
            panelList.TabIndex = 0;
            // 
            // lstLibraries
            // 
            lstLibraries.BorderStyle = BorderStyle.None;
            lstLibraries.Dock = DockStyle.Fill;
            lstLibraries.Font = new Font("Segoe UI", 9F);
            lstLibraries.ItemHeight = 25;
            lstLibraries.Location = new Point(0, 167);
            lstLibraries.Margin = new Padding(4, 5, 4, 5);
            lstLibraries.Name = "lstLibraries";
            lstLibraries.Size = new Size(640, 615);
            lstLibraries.TabIndex = 3;
            lstLibraries.DoubleClick += LstLibraries_DoubleClick;
            // 
            // lblSearchResults
            // 
            lblSearchResults.BackColor = Color.FromArgb(249, 250, 251);
            lblSearchResults.Dock = DockStyle.Top;
            lblSearchResults.Font = new Font("Segoe UI", 9F);
            lblSearchResults.ForeColor = Color.FromArgb(107, 114, 128);
            lblSearchResults.Location = new Point(0, 125);
            lblSearchResults.Margin = new Padding(4, 0, 4, 0);
            lblSearchResults.Name = "lblSearchResults";
            lblSearchResults.Padding = new Padding(14, 8, 14, 8);
            lblSearchResults.Size = new Size(640, 42);
            lblSearchResults.TabIndex = 2;
            lblSearchResults.Text = "📋 Mostrando todas las librerías";
            lblSearchResults.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.FromArgb(249, 250, 251);
            panelSearch.Controls.Add(btnClearSearch);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(lblSearch);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 67);
            panelSearch.Margin = new Padding(4, 5, 4, 5);
            panelSearch.Name = "panelSearch";
            panelSearch.Padding = new Padding(14, 13, 14, 13);
            panelSearch.Size = new Size(640, 58);
            panelSearch.TabIndex = 1;
            // 
            // btnClearSearch
            // 
            btnClearSearch.BackColor = Color.Red;
            btnClearSearch.FlatAppearance.BorderSize = 0;
            btnClearSearch.FlatStyle = FlatStyle.Flat;
            btnClearSearch.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            btnClearSearch.ForeColor = Color.White;
            btnClearSearch.Location = new Point(586, 6);
            btnClearSearch.Margin = new Padding(4, 5, 4, 5);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(38, 42);
            btnClearSearch.TabIndex = 2;
            btnClearSearch.Text = "✖";
            btnClearSearch.UseVisualStyleBackColor = false;
            btnClearSearch.Click += BtnClearSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(134, 8);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Escribe para filtrar (nombre, versión, GUID)...";
            txtSearch.Size = new Size(444, 34);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSearch.ForeColor = Color.FromArgb(31, 41, 55);
            lblSearch.Location = new Point(18, 8);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Padding = new Padding(22, 0, 0, 0);
            lblSearch.Size = new Size(108, 37);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Buscar:";
            lblSearch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelListButtons
            // 
            panelListButtons.BackColor = Color.FromArgb(249, 250, 251);
            panelListButtons.Controls.Add(btnEdit);
            panelListButtons.Controls.Add(btnDelete);
            panelListButtons.Dock = DockStyle.Bottom;
            panelListButtons.Location = new Point(0, 782);
            panelListButtons.Margin = new Padding(4, 5, 4, 5);
            panelListButtons.Name = "panelListButtons";
            panelListButtons.Padding = new Padding(14, 17, 14, 17);
            panelListButtons.Size = new Size(640, 83);
            panelListButtons.TabIndex = 4;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(37, 99, 235);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(14, 17);
            btnEdit.Margin = new Padding(4, 5, 4, 5);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(143, 50);
            btnEdit.TabIndex = 0;
            btnEdit.Text = "✏️ Editar";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(171, 17);
            btnDelete.Margin = new Padding(4, 5, 4, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(143, 50);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "🗑️ Eliminar";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;
            // 
            // lblListTitle
            // 
            lblListTitle.BackColor = Color.FromArgb(249, 250, 251);
            lblListTitle.Dock = DockStyle.Top;
            lblListTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblListTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblListTitle.Location = new Point(0, 0);
            lblListTitle.Margin = new Padding(4, 0, 4, 0);
            lblListTitle.Name = "lblListTitle";
            lblListTitle.Padding = new Padding(14, 17, 14, 17);
            lblListTitle.Size = new Size(640, 67);
            lblListTitle.TabIndex = 0;
            lblListTitle.Text = "📋 Lista de Librerías";
            lblListTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtJsonDisplay
            // 
            txtJsonDisplay.BackColor = Color.White;
            txtJsonDisplay.BorderStyle = BorderStyle.FixedSingle;
            txtJsonDisplay.Dock = DockStyle.Fill;
            txtJsonDisplay.Font = new Font("Consolas", 10F);
            txtJsonDisplay.Location = new Point(0, 0);
            txtJsonDisplay.Margin = new Padding(4, 5, 4, 5);
            txtJsonDisplay.Multiline = true;
            txtJsonDisplay.Name = "txtJsonDisplay";
            txtJsonDisplay.ReadOnly = true;
            txtJsonDisplay.ScrollBars = ScrollBars.Both;
            txtJsonDisplay.Size = new Size(1038, 867);
            txtJsonDisplay.TabIndex = 0;
            txtJsonDisplay.WordWrap = false;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(249, 250, 251);
            panelButtons.Controls.Add(btnClose);
            panelButtons.Controls.Add(btnAddNew);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnCopy);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 1034);
            panelButtons.Margin = new Padding(4, 5, 4, 5);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(29, 25, 29, 25);
            panelButtons.Size = new Size(1714, 133);
            panelButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(239, 68, 68);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1543, 33);
            btnClose.Margin = new Padding(4, 5, 4, 5);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(143, 67);
            btnClose.TabIndex = 3;
            btnClose.Text = "✖ Cerrar";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += BtnClose_Click;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = Color.FromArgb(168, 85, 247);
            btnAddNew.FlatAppearance.BorderSize = 0;
            btnAddNew.FlatStyle = FlatStyle.Flat;
            btnAddNew.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddNew.ForeColor = Color.White;
            btnAddNew.Location = new Point(457, 33);
            btnAddNew.Margin = new Padding(4, 5, 4, 5);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(214, 67);
            btnAddNew.TabIndex = 2;
            btnAddNew.Text = "➕ Agregar Nuevo";
            btnAddNew.UseVisualStyleBackColor = false;
            btnAddNew.Click += BtnAddNew_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(16, 185, 129);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(243, 33);
            btnSave.Margin = new Padding(4, 5, 4, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(200, 67);
            btnSave.TabIndex = 1;
            btnSave.Text = "💾 Guardar Todo";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCopy
            // 
            btnCopy.BackColor = Color.FromArgb(37, 99, 235);
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCopy.ForeColor = Color.White;
            btnCopy.Location = new Point(29, 33);
            btnCopy.Margin = new Padding(4, 5, 4, 5);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(200, 67);
            btnCopy.TabIndex = 0;
            btnCopy.Text = "📋 Copiar Filtrado";
            btnCopy.UseVisualStyleBackColor = false;
            btnCopy.Click += BtnCopy_Click;
            // 
            // ListarJson
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1714, 1167);
            Controls.Add(panelMain);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            Font = new Font("Segoe UI", 9F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MinimizeBox = false;
            Name = "ListarJson";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Editor JSON con Buscador";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelMain.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelList.ResumeLayout(false);
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            panelListButtons.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Label lblListTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Label lblSearchResults;
        private System.Windows.Forms.ListBox lstLibraries;
        private System.Windows.Forms.Panel panelListButtons;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtJsonDisplay;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCopy;
    }
}
