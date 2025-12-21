namespace NonaWin
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblSelectedPath = new System.Windows.Forms.Label();
            this.chkFilterMultipleOf12 = new System.Windows.Forms.CheckBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpDirectories = new System.Windows.Forms.TabPage();
            this.lvImages = new System.Windows.Forms.ListView();
            this.imgListThumbnails = new System.Windows.Forms.ImageList(this.components);
            this.lblDirectoryInfo = new System.Windows.Forms.Label();
            this.tvDirectories = new System.Windows.Forms.TreeView();
            this.contextMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSetMainTabSource = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClearMainTabSource = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearAllDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCleanMultipleOf12 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRenumberAllFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.tpDuplicates = new System.Windows.Forms.TabPage();
            this.lblDuplicateInfo = new System.Windows.Forms.Label();
            this.lstDuplicates = new System.Windows.Forms.ListBox();
            this.panelMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tpDirectories.SuspendLayout();
            this.contextMenuTreeView.SuspendLayout();
            this.tpDuplicates.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSelectFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectFolder.FlatAppearance.BorderSize = 0;
            this.btnSelectFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectFolder.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSelectFolder.ForeColor = System.Drawing.Color.White;
            this.btnSelectFolder.Location = new System.Drawing.Point(48, 75);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(1104, 50);
            this.btnSelectFolder.TabIndex = 1;
            this.btnSelectFolder.Text = "📁 選擇來源目錄";
            this.btnSelectFolder.UseVisualStyleBackColor = false;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblSelectedPath
            // 
            this.lblSelectedPath.BackColor = System.Drawing.Color.White;
            this.lblSelectedPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedPath.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblSelectedPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblSelectedPath.Location = new System.Drawing.Point(48, 138);
            this.lblSelectedPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedPath.Name = "lblSelectedPath";
            this.lblSelectedPath.Padding = new System.Windows.Forms.Padding(13, 6, 13, 6);
            this.lblSelectedPath.Size = new System.Drawing.Size(1103, 37);
            this.lblSelectedPath.TabIndex = 2;
            this.lblSelectedPath.Text = "尚未選擇目錄";
            this.lblSelectedPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkFilterMultipleOf12
            // 
            this.chkFilterMultipleOf12.AutoSize = true;
            this.chkFilterMultipleOf12.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.chkFilterMultipleOf12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.chkFilterMultipleOf12.Location = new System.Drawing.Point(47, 676);
            this.chkFilterMultipleOf12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkFilterMultipleOf12.Name = "chkFilterMultipleOf12";
            this.chkFilterMultipleOf12.Size = new System.Drawing.Size(420, 24);
            this.chkFilterMultipleOf12.TabIndex = 3;
            this.chkFilterMultipleOf12.Text = "☑ 排除檔名為12倍數的圖檔 (如: 12.jpg, 24.png, 36.jpg)";
            this.chkFilterMultipleOf12.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnExecute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExecute.Enabled = false;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(48, 708);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(1104, 56);
            this.btnExecute.TabIndex = 4;
            this.btnExecute.Text = "🚀 開始複製圖檔";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblStatus.Location = new System.Drawing.Point(48, 815);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1104, 38);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "準備就緒";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(48, 777);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1104, 25);
            this.progressBar.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblTitle.Location = new System.Drawing.Point(40, 25);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(197, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "圖檔整理工具";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panelMain.Controls.Add(this.tabControl);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.btnSelectFolder);
            this.panelMain.Controls.Add(this.lblSelectedPath);
            this.panelMain.Controls.Add(this.chkFilterMultipleOf12);
            this.panelMain.Controls.Add(this.btnExecute);
            this.panelMain.Controls.Add(this.progressBar);
            this.panelMain.Controls.Add(this.lblStatus);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1200, 875);
            this.panelMain.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpDirectories);
            this.tabControl.Controls.Add(this.tpDuplicates);
            this.tabControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.Location = new System.Drawing.Point(48, 188);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1104, 480);
            this.tabControl.TabIndex = 3;
            // 
            // tpDirectories
            // 
            this.tpDirectories.Controls.Add(this.lvImages);
            this.tpDirectories.Controls.Add(this.lblDirectoryInfo);
            this.tpDirectories.Controls.Add(this.tvDirectories);
            this.tpDirectories.Location = new System.Drawing.Point(4, 28);
            this.tpDirectories.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpDirectories.Name = "tpDirectories";
            this.tpDirectories.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpDirectories.Size = new System.Drawing.Size(1096, 448);
            this.tpDirectories.TabIndex = 0;
            this.tpDirectories.Text = "📁 目錄預覽與預覽圖";
            this.tpDirectories.UseVisualStyleBackColor = true;
            // 
            // lvImages
            // 
            this.lvImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvImages.HideSelection = false;
            this.lvImages.LargeImageList = this.imgListThumbnails;
            this.lvImages.Location = new System.Drawing.Point(373, 44);
            this.lvImages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvImages.Name = "lvImages";
            this.lvImages.Size = new System.Drawing.Size(711, 386);
            this.lvImages.TabIndex = 2;
            this.lvImages.UseCompatibleStateImageBehavior = false;
            // 
            // imgListThumbnails
            // 
            this.imgListThumbnails.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgListThumbnails.ImageSize = new System.Drawing.Size(120, 120);
            this.imgListThumbnails.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblDirectoryInfo
            // 
            this.lblDirectoryInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDirectoryInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblDirectoryInfo.Location = new System.Drawing.Point(8, 12);
            this.lblDirectoryInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDirectoryInfo.Name = "lblDirectoryInfo";
            this.lblDirectoryInfo.Size = new System.Drawing.Size(1077, 25);
            this.lblDirectoryInfo.TabIndex = 0;
            this.lblDirectoryInfo.Text = "尚未選擇目錄";
            // 
            // tvDirectories
            // 
            this.tvDirectories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvDirectories.ContextMenuStrip = this.contextMenuTreeView;
            this.tvDirectories.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tvDirectories.HideSelection = false;
            this.tvDirectories.Location = new System.Drawing.Point(8, 44);
            this.tvDirectories.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tvDirectories.Name = "tvDirectories";
            this.tvDirectories.ShowLines = false;
            this.tvDirectories.ShowPlusMinus = false;
            this.tvDirectories.ShowRootLines = false;
            this.tvDirectories.Size = new System.Drawing.Size(357, 384);
            this.tvDirectories.TabIndex = 1;
            this.tvDirectories.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDirectories_AfterSelect);
            // 
            // contextMenuTreeView
            // 
            this.contextMenuTreeView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetMainTabSource,
            this.menuClearMainTabSource,
            this.menuSeparator,
            this.menuClearAllDirectory,
            this.menuCleanMultipleOf12,
            this.menuRenumberAllFiles});
            this.contextMenuTreeView.Name = "contextMenuTreeView";
            this.contextMenuTreeView.Size = new System.Drawing.Size(281, 106);
            this.contextMenuTreeView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTreeView_Opening);
            // 
            // menuSetMainTabSource
            // 
            this.menuSetMainTabSource.Name = "menuSetMainTabSource";
            this.menuSetMainTabSource.Size = new System.Drawing.Size(280, 24);
            this.menuSetMainTabSource.Text = "✓ 設為 Main/Tab 來源目錄";
            this.menuSetMainTabSource.Click += new System.EventHandler(this.menuSetMainTabSource_Click);
            // 
            // menuClearMainTabSource
            // 
            this.menuClearMainTabSource.Name = "menuClearMainTabSource";
            this.menuClearMainTabSource.Size = new System.Drawing.Size(280, 24);
            this.menuClearMainTabSource.Text = "✗ 清除 Main/Tab 來源";
            this.menuClearMainTabSource.Click += new System.EventHandler(this.menuClearMainTabSource_Click);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(277, 6);
            // 
            // menuClearAllDirectory
            // 
            this.menuClearAllDirectory.Name = "menuClearAllDirectory";
            this.menuClearAllDirectory.Size = new System.Drawing.Size(280, 24);
            this.menuClearAllDirectory.Text = "🗑️ 清空 ALL 目錄";
            this.menuClearAllDirectory.Click += new System.EventHandler(this.menuClearAllDirectory_Click);
            // 
            // menuCleanMultipleOf12
            // 
            this.menuCleanMultipleOf12.Name = "menuCleanMultipleOf12";
            this.menuCleanMultipleOf12.Size = new System.Drawing.Size(280, 24);
            this.menuCleanMultipleOf12.Text = "🧹 清理12倍數檔案並重新編號";
            this.menuCleanMultipleOf12.Click += new System.EventHandler(this.menuCleanMultipleOf12_Click);
            // 
            // menuRenumberAllFiles
            // 
            this.menuRenumberAllFiles.Name = "menuRenumberAllFiles";
            this.menuRenumberAllFiles.Size = new System.Drawing.Size(280, 22);
            this.menuRenumberAllFiles.Text = "🔢 重新編號所有檔案";
            this.menuRenumberAllFiles.Click += new System.EventHandler(this.menuRenumberAllFiles_Click);
            // 
            // tpDuplicates
            // 
            this.tpDuplicates.Controls.Add(this.lblDuplicateInfo);
            this.tpDuplicates.Controls.Add(this.lstDuplicates);
            this.tpDuplicates.Location = new System.Drawing.Point(4, 28);
            this.tpDuplicates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpDuplicates.Name = "tpDuplicates";
            this.tpDuplicates.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpDuplicates.Size = new System.Drawing.Size(1096, 443);
            this.tpDuplicates.TabIndex = 1;
            this.tpDuplicates.Text = "📑 重複檔案分析";
            this.tpDuplicates.UseVisualStyleBackColor = true;
            // 
            // lblDuplicateInfo
            // 
            this.lblDuplicateInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDuplicateInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblDuplicateInfo.Location = new System.Drawing.Point(8, 12);
            this.lblDuplicateInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDuplicateInfo.Name = "lblDuplicateInfo";
            this.lblDuplicateInfo.Size = new System.Drawing.Size(1077, 25);
            this.lblDuplicateInfo.TabIndex = 0;
            this.lblDuplicateInfo.Text = "請點擊按鈕開始分析";
            // 
            // lstDuplicates
            // 
            this.lstDuplicates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstDuplicates.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lstDuplicates.FormattingEnabled = true;
            this.lstDuplicates.ItemHeight = 20;
            this.lstDuplicates.Location = new System.Drawing.Point(8, 44);
            this.lstDuplicates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstDuplicates.Name = "lstDuplicates";
            this.lstDuplicates.Size = new System.Drawing.Size(1077, 380);
            this.lstDuplicates.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 875);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NonaWin - 圖檔整理工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tpDirectories.ResumeLayout(false);
            this.contextMenuTreeView.ResumeLayout(false);
            this.tpDuplicates.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.Label lblSelectedPath;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpDirectories;
        private System.Windows.Forms.Label lblDirectoryInfo;
        private System.Windows.Forms.TreeView tvDirectories;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeView;
        private System.Windows.Forms.ToolStripMenuItem menuSetMainTabSource;
        private System.Windows.Forms.ToolStripMenuItem menuClearMainTabSource;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuClearAllDirectory;
        private System.Windows.Forms.ListView lvImages;
        private System.Windows.Forms.ImageList imgListThumbnails;
        private System.Windows.Forms.TabPage tpDuplicates;
        private System.Windows.Forms.Label lblDuplicateInfo;
        private System.Windows.Forms.ListBox lstDuplicates;
        private System.Windows.Forms.CheckBox chkFilterMultipleOf12;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ToolStripMenuItem menuCleanMultipleOf12;
        private System.Windows.Forms.ToolStripMenuItem menuRenumberAllFiles;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
    }
}

