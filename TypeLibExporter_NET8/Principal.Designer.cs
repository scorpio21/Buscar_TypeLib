namespace TypeLibExporter_NET8
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            menuStrip = new MenuStrip();
            archivoMenuItem = new ToolStripMenuItem();
            utilidadesMenuItem = new ToolStripMenuItem();
            cargarJsonMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            cerrarMenuItem = new ToolStripMenuItem();
            panelHeader = new Panel();
            lblDescription = new Label();
            lblTitle = new Label();
            panelMain = new Panel();
            lstResults = new ListBox();
            lblResultsTitle = new Label();
            btnExportCombined = new Button();
            btnExportClsIds = new Button();
            btnExportVB6 = new Button();
            btnExportTypeLibs = new Button();
            lblExportTitle = new Label();
            btnSelectLocation = new Button();
            txtLocation = new TextBox();
            lblLocationTitle = new Label();
            panelFooter = new Panel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            menuStrip.SuspendLayout();
            panelHeader.SuspendLayout();
            panelMain.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Items.AddRange(new ToolStripItem[] { archivoMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(9, 3, 0, 3);
            menuStrip.Size = new Size(1429, 35);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // archivoMenuItem
            // 
            archivoMenuItem.DropDownItems.AddRange(new ToolStripItem[] { utilidadesMenuItem, toolStripSeparator1, cerrarMenuItem });
            archivoMenuItem.Name = "archivoMenuItem";
            archivoMenuItem.Size = new Size(88, 29);
            archivoMenuItem.Text = "&Archivo";
            // 
            // utilidadesMenuItem
            // 
            utilidadesMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cargarJsonMenuItem });
            utilidadesMenuItem.Name = "utilidadesMenuItem";
            utilidadesMenuItem.Size = new Size(226, 34);
            utilidadesMenuItem.Text = "&Utilidades";
            // 
            // cargarJsonMenuItem
            // 
            cargarJsonMenuItem.Name = "cargarJsonMenuItem";
            cargarJsonMenuItem.Size = new Size(214, 34);
            cargarJsonMenuItem.Text = "&Cargar JSON";
            cargarJsonMenuItem.Click += CargarJsonMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(223, 6);
            // 
            // cerrarMenuItem
            // 
            cerrarMenuItem.Name = "cerrarMenuItem";
            cerrarMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            cerrarMenuItem.Size = new Size(226, 34);
            cerrarMenuItem.Text = "&Cerrar";
            cerrarMenuItem.Click += CerrarMenuItem_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(37, 99, 235);
            panelHeader.Controls.Add(lblDescription);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 35);
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(43, 33, 43, 33);
            panelHeader.Size = new Size(1429, 167);
            panelHeader.TabIndex = 1;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Segoe UI", 11F);
            lblDescription.ForeColor = Color.FromArgb(200, 220, 255);
            lblDescription.Location = new Point(43, 92);
            lblDescription.Margin = new Padding(4, 0, 4, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(738, 30);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "Exporta solo archivos .DLL y .OCX del registro de Windows a formato JSON";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(43, 33);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(634, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🔧 Exportador de TypeLibs + CLSIDs";
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(248, 250, 252);
            panelMain.Controls.Add(lstResults);
            panelMain.Controls.Add(lblResultsTitle);
            panelMain.Controls.Add(btnExportCombined);
            panelMain.Controls.Add(btnExportClsIds);
            panelMain.Controls.Add(btnExportVB6);
            panelMain.Controls.Add(btnExportTypeLibs);
            panelMain.Controls.Add(lblExportTitle);
            panelMain.Controls.Add(btnSelectLocation);
            panelMain.Controls.Add(txtLocation);
            panelMain.Controls.Add(lblLocationTitle);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 202);
            panelMain.Margin = new Padding(4, 5, 4, 5);
            panelMain.Name = "panelMain";
            panelMain.Padding = new Padding(43, 50, 43, 50);
            panelMain.Size = new Size(1429, 965);
            panelMain.TabIndex = 2;
            // 
            // lstResults
            // 
            lstResults.BackColor = Color.White;
            lstResults.BorderStyle = BorderStyle.FixedSingle;
            lstResults.Font = new Font("Consolas", 9F);
            lstResults.ForeColor = Color.FromArgb(31, 41, 55);
            lstResults.ItemHeight = 22;
            lstResults.Location = new Point(43, 450);
            lstResults.Margin = new Padding(4, 5, 4, 5);
            lstResults.Name = "lstResults";
            lstResults.ScrollAlwaysVisible = true;
            lstResults.Size = new Size(1342, 398);
            lstResults.TabIndex = 9;
            // 
            // lblResultsTitle
            // 
            lblResultsTitle.AutoSize = true;
            lblResultsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblResultsTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblResultsTitle.Location = new Point(43, 400);
            lblResultsTitle.Margin = new Padding(4, 0, 4, 0);
            lblResultsTitle.Name = "lblResultsTitle";
            lblResultsTitle.Size = new Size(593, 32);
            lblResultsTitle.TabIndex = 8;
            lblResultsTitle.Text = "📋 Componentes DLL/OCX encontrados (filtrados)";
            // 
            // btnExportCombined
            // 
            btnExportCombined.BackColor = Color.FromArgb(239, 68, 68);
            btnExportCombined.FlatAppearance.BorderSize = 0;
            btnExportCombined.FlatStyle = FlatStyle.Flat;
            btnExportCombined.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnExportCombined.ForeColor = Color.White;
            btnExportCombined.Location = new Point(1071, 267);
            btnExportCombined.Margin = new Padding(4, 5, 4, 5);
            btnExportCombined.Name = "btnExportCombined";
            btnExportCombined.Size = new Size(314, 83);
            btnExportCombined.TabIndex = 7;
            btnExportCombined.Text = "🚀 Exportar TODO";
            btnExportCombined.UseVisualStyleBackColor = false;
            btnExportCombined.Click += BtnExportCombined_Click;
            // 
            // btnExportClsIds
            // 
            btnExportClsIds.BackColor = Color.FromArgb(147, 51, 234);
            btnExportClsIds.FlatAppearance.BorderSize = 0;
            btnExportClsIds.FlatStyle = FlatStyle.Flat;
            btnExportClsIds.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportClsIds.ForeColor = Color.White;
            btnExportClsIds.Location = new Point(729, 267);
            btnExportClsIds.Margin = new Padding(4, 5, 4, 5);
            btnExportClsIds.Name = "btnExportClsIds";
            btnExportClsIds.Size = new Size(329, 83);
            btnExportClsIds.TabIndex = 6;
            btnExportClsIds.Text = "🔧 Exportar CLSIDs";
            btnExportClsIds.UseVisualStyleBackColor = false;
            btnExportClsIds.Click += BtnExportClsIds_Click;
            // 
            // btnExportVB6
            // 
            btnExportVB6.BackColor = Color.FromArgb(16, 185, 129);
            btnExportVB6.FlatAppearance.BorderSize = 0;
            btnExportVB6.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 150, 105);
            btnExportVB6.FlatStyle = FlatStyle.Flat;
            btnExportVB6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportVB6.ForeColor = Color.White;
            btnExportVB6.Location = new Point(386, 267);
            btnExportVB6.Margin = new Padding(4, 5, 4, 5);
            btnExportVB6.Name = "btnExportVB6";
            btnExportVB6.Size = new Size(329, 83);
            btnExportVB6.TabIndex = 5;
            btnExportVB6.Text = "⭐ TypeLibs VB6";
            btnExportVB6.UseVisualStyleBackColor = false;
            btnExportVB6.Click += BtnExportVB6_Click;
            // 
            // btnExportTypeLibs
            // 
            btnExportTypeLibs.BackColor = Color.FromArgb(37, 99, 235);
            btnExportTypeLibs.FlatAppearance.BorderSize = 0;
            btnExportTypeLibs.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            btnExportTypeLibs.FlatStyle = FlatStyle.Flat;
            btnExportTypeLibs.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExportTypeLibs.ForeColor = Color.White;
            btnExportTypeLibs.Location = new Point(43, 267);
            btnExportTypeLibs.Margin = new Padding(4, 5, 4, 5);
            btnExportTypeLibs.Name = "btnExportTypeLibs";
            btnExportTypeLibs.Size = new Size(329, 83);
            btnExportTypeLibs.TabIndex = 4;
            btnExportTypeLibs.Text = "📚 Exportar TypeLibs";
            btnExportTypeLibs.UseVisualStyleBackColor = false;
            btnExportTypeLibs.Click += BtnExportTypeLibs_Click;
            // 
            // lblExportTitle
            // 
            lblExportTitle.AutoSize = true;
            lblExportTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExportTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblExportTitle.Location = new Point(43, 200);
            lblExportTitle.Margin = new Padding(4, 0, 4, 0);
            lblExportTitle.Name = "lblExportTitle";
            lblExportTitle.Size = new Size(569, 32);
            lblExportTitle.TabIndex = 3;
            lblExportTitle.Text = "⚡ Exportación filtrada (solo DLL y OCX válidos)";
            // 
            // btnSelectLocation
            // 
            btnSelectLocation.BackColor = Color.FromArgb(107, 114, 128);
            btnSelectLocation.FlatAppearance.BorderSize = 0;
            btnSelectLocation.FlatStyle = FlatStyle.Flat;
            btnSelectLocation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSelectLocation.ForeColor = Color.White;
            btnSelectLocation.Location = new Point(1214, 97);
            btnSelectLocation.Margin = new Padding(4, 5, 4, 5);
            btnSelectLocation.Name = "btnSelectLocation";
            btnSelectLocation.Size = new Size(171, 50);
            btnSelectLocation.TabIndex = 2;
            btnSelectLocation.Text = "Buscar...";
            btnSelectLocation.UseVisualStyleBackColor = false;
            btnSelectLocation.Click += BtnSelectLocation_Click;
            // 
            // txtLocation
            // 
            txtLocation.Font = new Font("Segoe UI", 10F);
            txtLocation.Location = new Point(43, 100);
            txtLocation.Margin = new Padding(4, 5, 4, 5);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(1141, 34);
            txtLocation.TabIndex = 1;
            // 
            // lblLocationTitle
            // 
            lblLocationTitle.AutoSize = true;
            lblLocationTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLocationTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblLocationTitle.Location = new Point(43, 50);
            lblLocationTitle.Margin = new Padding(4, 0, 4, 0);
            lblLocationTitle.Name = "lblLocationTitle";
            lblLocationTitle.Size = new Size(462, 32);
            lblLocationTitle.TabIndex = 0;
            lblLocationTitle.Text = "📁 Ubicación de guardado (recordada)";
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.FromArgb(249, 250, 251);
            panelFooter.Controls.Add(progressBar);
            panelFooter.Controls.Add(lblStatus);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 1034);
            panelFooter.Margin = new Padding(4, 5, 4, 5);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new Padding(43, 25, 43, 25);
            panelFooter.Size = new Size(1429, 133);
            panelFooter.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.ForeColor = Color.FromArgb(34, 197, 94);
            progressBar.Location = new Point(43, 83);
            progressBar.Margin = new Padding(4, 5, 4, 5);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1343, 25);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 10F);
            lblStatus.ForeColor = Color.FromArgb(107, 114, 128);
            lblStatus.Location = new Point(43, 33);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(314, 28);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Listo para exportar (solo DLL/OCX)";
            // 
            // Principal
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1429, 1167);
            Controls.Add(panelFooter);
            Controls.Add(panelMain);
            Controls.Add(panelHeader);
            Controls.Add(menuStrip);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "Principal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TypeLib + CLSID Exporter - .NET 8 (Filtrado DLL/OCX)";
            FormClosing += Principal_FormClosing;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem archivoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilidadesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarJsonMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cerrarMenuItem;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblLocationTitle;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnSelectLocation;
        private System.Windows.Forms.Label lblExportTitle;
        private System.Windows.Forms.Button btnExportTypeLibs;
        private System.Windows.Forms.Button btnExportVB6;
        private System.Windows.Forms.Button btnExportClsIds;
        private System.Windows.Forms.Button btnExportCombined;
        private System.Windows.Forms.Label lblResultsTitle;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
