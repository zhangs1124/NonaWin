using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonaWin
{
    public partial class Form1 : Form
    {
        private string selectedDirectory = string.Empty;
        private string mainTabSourceDirectory = ""; // 儲存被指定為 main/tab 來源的目錄
        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 載入上次選擇的目錄
            if (!string.IsNullOrEmpty(Properties.Settings.Default.SourceDirectory) &&
                Directory.Exists(Properties.Settings.Default.SourceDirectory))
            {
                selectedDirectory = Properties.Settings.Default.SourceDirectory;
                lblSelectedPath.Text = selectedDirectory;
                btnExecute.Enabled = true;
                lblStatus.Text = "已載入上次選擇的目錄";
                lblStatus.ForeColor = Color.FromArgb(52, 152, 219);
                
                // 自動載入目錄資訊
                LoadDirectoryInfo();
                AnalyzeDuplicatesAsync();
            }
        }


        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "請選擇包含圖檔的來源目錄";
                dialog.ShowNewFolderButton = false;
                
                if (!string.IsNullOrEmpty(selectedDirectory))
                {
                    dialog.SelectedPath = selectedDirectory;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedDirectory = dialog.SelectedPath;
                    lblSelectedPath.Text = selectedDirectory;
                    
                    // 載入目錄資訊
                    LoadDirectoryInfo();
                    
                    // 分析重複檔案
                    AnalyzeDuplicatesAsync();
                    
                    btnExecute.Enabled = true;
                    lblStatus.Text = "已選擇目錄，點擊「開始複製圖檔」執行";
                    lblStatus.ForeColor = Color.FromArgb(52, 152, 219);
                }
            }
        }


        private void LoadDirectoryInfo()
        {
            tvDirectories.Nodes.Clear();

            try
            {
                // 🔥 新增：檢查是否為 ZIP 處理模式
                if (chkProcessZip.Checked)
                {
                    LoadZipFilesList();
                    return;
                }

                var subDirectories = Directory.GetDirectories(selectedDirectory);
                
                if (subDirectories.Length == 0)
                {
                    lblDirectoryInfo.Text = "目錄預覽（沒有找到子目錄）";
                    tvDirectories.Nodes.Add("沒有找到子目錄");
                    return;
                }

                int totalImageCount = 0;
                int validDirCount = 0;

                // 先處理 ALL 目錄（如果存在）
                string allDirPath = Path.Combine(selectedDirectory, "ALL");
                if (Directory.Exists(allDirPath))
                {
                    var allImageFiles = Directory.GetFiles(allDirPath)
                        .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                        .ToList();
                    
                    string displayText = $"ALL (目標資料夾 - {allImageFiles.Count} 個圖檔)";
                    TreeNode allNode = new TreeNode(displayText);
                    allNode.Tag = allDirPath;
                    allNode.ForeColor = Color.FromArgb(39, 174, 96); // 綠色標示
                    allNode.NodeFont = new Font(tvDirectories.Font, FontStyle.Italic);
                    tvDirectories.Nodes.Add(allNode);
                }

                foreach (var subDir in subDirectories)
                {
                    // 跳過 ALL 目錄
                    string dirName = Path.GetFileName(subDir);
                    if (dirName.Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // 計算該目錄的圖檔數量
                    var imageFiles = Directory.GetFiles(subDir)
                        .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                        .ToList();

                    if (imageFiles.Count > 0)
                    {
                        // 計算會被複製的檔案數量
                        int willCopyCount = 0;
                        bool isMainTabSource = !string.IsNullOrEmpty(mainTabSourceDirectory) && 
                                               subDir.Equals(mainTabSourceDirectory, StringComparison.OrdinalIgnoreCase);
                        
                        for (int i = 0; i < imageFiles.Count; i++)
                        {
                            string file = imageFiles[i];
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            bool isLast = (i == imageFiles.Count - 1);

                            // 檢查是否為 main 或 tab
                            bool isMainOrTab = fileName.Equals("main", StringComparison.OrdinalIgnoreCase) ||
                                              fileName.Equals("tab", StringComparison.OrdinalIgnoreCase);

                            // 如果是 main/tab，只有當此目錄是指定的來源目錄時才會被複製
                            if (isMainOrTab)
                            {
                                if (isMainTabSource)
                                {
                                    willCopyCount++; // 這個目錄的 main/tab 會被複製
                                }
                                // 否則不計入
                                continue;
                            }

                            // 排除最後一個檔案
                            if (isLast)
                            {
                                continue;
                            }

                            willCopyCount++;
                        }

                        totalImageCount += willCopyCount;
                        validDirCount++;

                        // 顯示格式：目錄名稱 - 總圖檔數 (會複製數)
                        string displayText = $"{dirName} ({imageFiles.Count} 個圖檔, {willCopyCount} 複製)";
                        TreeNode node = new TreeNode(displayText);
                        node.Tag = subDir; // 將完整路徑儲存於 Tag 屬性，避免解析錯誤！
                        
                        // 如果這是 main/tab 來源目錄，使用粗體顯示
                        if (isMainTabSource)
                        {
                            node.NodeFont = new Font(tvDirectories.Font, FontStyle.Bold);
                            node.ForeColor = Color.FromArgb(41, 128, 185); // 藍色標示
                        }
                        
                        tvDirectories.Nodes.Add(node);
                    }
                    else
                    {
                        TreeNode node = new TreeNode($"{dirName} (0)");
                        node.Tag = subDir;
                        tvDirectories.Nodes.Add(node);
                    }
                }

                lblDirectoryInfo.Text = $"目錄預覽（共 {validDirCount} 個目錄，預計複製 {totalImageCount} 個圖檔）";
            }
            catch (Exception ex)
            {
                lblDirectoryInfo.Text = "目錄預覽（載入失敗）";
                tvDirectories.Nodes.Add($"錯誤：{ex.Message}");
            }
        }

        private void LoadZipFilesList()
        {
            try
            {
                // 掃描所有ZIP檔案
                var zipFiles = Directory.GetFiles(selectedDirectory, "*.zip")
                    .Where(f => !Path.GetFileName(f).Equals("ALL.zip", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(f => Path.GetFileName(f))
                    .ToList();

                if (zipFiles.Count == 0)
                {
                    lblDirectoryInfo.Text = "ZIP 模式（沒有找到 ZIP 檔案）";
                    tvDirectories.Nodes.Add("沒有找到 ZIP 檔案");
                    return;
                }

                int totalFiles = 0;

                foreach (var zipFile in zipFiles)
                {
                    string zipName = Path.GetFileName(zipFile);
                    long fileSize = new FileInfo(zipFile).Length;
                    string fileSizeStr = fileSize > 1024 * 1024 
                        ? $"{fileSize / 1024.0 / 1024.0:F2} MB" 
                        : $"{fileSize / 1024.0:F2} KB";

                    // 嘗試讀取 ZIP 內的圖檔數量
                    int imageCount = 0;
                    try
                    {
                        using (ZipArchive archive = ZipFile.OpenRead(zipFile))
                        {
                            imageCount = archive.Entries
                                .Where(e => ImageExtensions.Contains(Path.GetExtension(e.Name).ToLower()))
                                .Count();
                        }
                    }
                    catch
                    {
                        imageCount = -1; // 表示無法讀取
                    }

                    string displayText = imageCount >= 0 
                        ? $"📦 {zipName} ({fileSizeStr}, {imageCount} 個圖檔)"
                        : $"📦 {zipName} ({fileSizeStr}, 無法讀取)";

                    TreeNode node = new TreeNode(displayText);
                    node.Tag = zipFile;
                    node.ForeColor = Color.FromArgb(142, 68, 173); // 紫色標示 ZIP
                    tvDirectories.Nodes.Add(node);

                    if (imageCount > 0)
                        totalFiles += imageCount;
                }

                lblDirectoryInfo.Text = $"ZIP 模式（共 {zipFiles.Count} 個 ZIP 檔案，預估 {totalFiles} 個圖檔）";
            }
            catch (Exception ex)
            {
                lblDirectoryInfo.Text = "ZIP 模式（載入失敗）";
                tvDirectories.Nodes.Add($"錯誤：{ex.Message}");
            }
        }

        private async void AnalyzeDuplicatesAsync()
        {
            lstDuplicates.Items.Clear();
            lblDuplicateInfo.Text = "正在分析重複檔案...";
            lblDuplicateInfo.ForeColor = Color.FromArgb(52, 152, 219);

            try
            {
                var duplicates = await Task.Run(() => FindDuplicates());
                
                if (duplicates.Count == 0)
                {
                    lblDuplicateInfo.Text = "分析完成：未發現內容重複的檔案";
                    lblDuplicateInfo.ForeColor = Color.FromArgb(46, 204, 113);
                    lstDuplicates.Items.Add("  ✅ 所有圖檔內容皆不重複");
                    return;
                }

                int totalSets = duplicates.Count;
                int totalFiles = duplicates.Values.Sum(v => v.Count);
                lblDuplicateInfo.Text = $"分析完成：發現 {totalSets} 組重複檔案（共 {totalFiles} 個檔案）";
                lblDuplicateInfo.ForeColor = Color.FromArgb(231, 76, 60);

                foreach (var entry in duplicates)
                {
                    lstDuplicates.Items.Add($"📍 重複組 (Hash: {entry.Key.Substring(0, 8)}...)");
                    foreach (var path in entry.Value)
                    {
                        string relPath = path.Replace(selectedDirectory, "");
                        lstDuplicates.Items.Add($"    📄 {relPath}");
                    }
                    lstDuplicates.Items.Add("");
                }
            }
            catch (Exception ex)
            {
                lblDuplicateInfo.Text = "分析失敗";
                lblDuplicateInfo.ForeColor = Color.FromArgb(231, 76, 60);
                lstDuplicates.Items.Add($"  錯誤：{ex.Message}");
            }
        }

        private Dictionary<string, List<string>> FindDuplicates()
        {
            var hashToPaths = new Dictionary<string, List<string>>();
            var subDirectories = Directory.GetDirectories(selectedDirectory);

            foreach (var subDir in subDirectories)
            {
                if (Path.GetFileName(subDir).Equals("ALL", StringComparison.OrdinalIgnoreCase)) continue;

                var files = Directory.GetFiles(subDir)
                    .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()));

                foreach (var file in files)
                {
                    string hash = GetFileHash(file);
                    if (!hashToPaths.ContainsKey(hash))
                    {
                        hashToPaths[hash] = new List<string>();
                    }
                    hashToPaths[hash].Add(file);
                }
            }

            // 只回傳有重複的 (Count > 1)
            return hashToPaths.Where(kvp => kvp.Value.Count > 1)
                              .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private string GetFileHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }


        private async void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedDirectory) || !Directory.Exists(selectedDirectory))
            {
                MessageBox.Show("請先選擇有效的來源目錄", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 禁用按鈕避免重複點擊
            btnExecute.Enabled = false;
            btnSelectFolder.Enabled = false;
            progressBar.Value = 0;

            bool wasProcessingZip = chkProcessZip.Checked; // 記錄是否為 ZIP 模式

            try
            {
                // 檢查ZIP處理模式
                if (chkProcessZip.Checked)
                {
                    await Task.Run(() => ProcessZipFiles());
                }
                else
                {
                    await Task.Run(() => ProcessImages());
                }
                
                // 儲存設定
                Properties.Settings.Default.SourceDirectory = selectedDirectory;
                Properties.Settings.Default.Save();

                // 🔥 新增：如果是 ZIP 模式，處理完成後自動取消勾選，讓用戶看到結果
                if (wasProcessingZip)
                {
                    chkProcessZip.Checked = false; // 這會自動觸發 CheckedChanged 事件，重新載入目錄列表
                }

                MessageBox.Show($"圖檔複製完成！\n\n目標目錄：{Path.Combine(selectedDirectory, "ALL")}", 
                    "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus($"錯誤：{ex.Message}", Color.FromArgb(231, 76, 60));
            }
            finally
            {
                btnExecute.Enabled = true;
                btnSelectFolder.Enabled = true;
            }
        }

        private void ProcessImages()
        {
            // 建立 ALL 目錄
            string allDirectory = Path.Combine(selectedDirectory, "ALL");
            if (!Directory.Exists(allDirectory))
            {
                Directory.CreateDirectory(allDirectory);
            }

            // 取得所有子目錄
            var subDirectories = Directory.GetDirectories(selectedDirectory);
            
            if (subDirectories.Length == 0)
            {
                UpdateStatus("找不到任何子目錄", Color.FromArgb(231, 76, 60));
                return;
            }

            int totalCopied = 0;
            int totalProcessed = 0;

            UpdateProgress(0, subDirectories.Length);

            // 🔥 新邏輯：如果勾選過濾，先收集所有檔案再統一處理
            if (chkFilterMultipleOf12.Checked)
            {
                // 收集所有要複製的檔案
                var allFilesToCopy = new List<string>();
                var mainTabFiles = new List<string>(); // 分開存放 main/tab 檔案
                
                // 如果沒有指定 main/tab 來源，預設使用第一個目錄
                if (string.IsNullOrEmpty(mainTabSourceDirectory))
                {
                    var firstDir = subDirectories
                        .FirstOrDefault(d => !Path.GetFileName(d).Equals("ALL", StringComparison.OrdinalIgnoreCase));
                    if (firstDir != null)
                    {
                        mainTabSourceDirectory = firstDir;
                    }
                }

                foreach (var subDir in subDirectories)
                {
                    // 跳過 ALL 目錄本身
                    if (Path.GetFileName(subDir).Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    UpdateStatus($"掃描目錄：{Path.GetFileName(subDir)}...", Color.FromArgb(52, 152, 219));

                    // 取得目錄中所有圖檔（按檔名排序）
                    var imageFiles = Directory.GetFiles(subDir)
                        .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                        .OrderBy(f => Path.GetFileName(f))
                        .ToList();

                    if (imageFiles.Count == 0)
                    {
                        continue;
                    }

                    // 過濾檔案
                    bool isMainTabSource = !string.IsNullOrEmpty(mainTabSourceDirectory) && 
                                           subDir.Equals(mainTabSourceDirectory, StringComparison.OrdinalIgnoreCase);
                    
                    for (int i = 0; i < imageFiles.Count; i++)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(imageFiles[i]);
                        bool isLast = (i == imageFiles.Count - 1);

                        // 檢查是否為 main 或 tab
                        bool isMainOrTab = fileName.Equals("main", StringComparison.OrdinalIgnoreCase) ||
                                          fileName.Equals("tab", StringComparison.OrdinalIgnoreCase);

                        // 規則 1：main/tab 檔案只有在此目錄是指定來源時才複製
                        if (isMainOrTab)
                        {
                            if (!isMainTabSource)
                            {
                                continue; // 不是來源目錄，跳過 main/tab
                            }
                            // 是來源目錄，則加入 main/tab 專用清單
                            mainTabFiles.Add(imageFiles[i]);
                            continue;
                        }

                        // 規則 2：排除每個目錄的最後一個檔案（非 main/tab 的情況）
                        if (isLast)
                        {
                            continue;
                        }

                        // 規則 3：過濾12倍數檔案
                        if (IsMultipleOf12Filename(Path.GetFileName(imageFiles[i])))
                        {
                            continue; // 跳過12倍數
                        }

                        allFilesToCopy.Add(imageFiles[i]);
                    }
                }

                // 重新編號並複製
                int newNumber = 1;
                
                // 先複製 main/tab 檔案（保持原名）
                foreach (var sourceFile in mainTabFiles)
                {
                    string fileName = Path.GetFileName(sourceFile);
                    string destFile = Path.Combine(allDirectory, fileName);
                    
                    UpdateStatus($"複製 main/tab：{fileName}", Color.FromArgb(52, 152, 219));
                    
                    File.Copy(sourceFile, destFile, true);
                    totalCopied++;
                }
                
                // 再複製數字檔案（重新編號）
                foreach (var sourceFile in allFilesToCopy)
                {
                    string ext = Path.GetExtension(sourceFile);
                    string newFileName = $"{newNumber:D2}{ext}";
                    string destFile = Path.Combine(allDirectory, newFileName);

                    UpdateStatus($"複製並重新編號：{Path.GetFileName(sourceFile)} → {newFileName}", Color.FromArgb(52, 152, 219));

                    File.Copy(sourceFile, destFile, true);
                    totalCopied++;
                    newNumber++;
                }

                UpdateProgress(subDirectories.Length, subDirectories.Length);
            }
            else
            {
                // 原始邏輯：不勾選時，保留原檔名複製
                foreach (var subDir in subDirectories)
                {
                    // 跳過 ALL 目錄本身
                    if (Path.GetFileName(subDir).Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    UpdateStatus($"處理目錄：{Path.GetFileName(subDir)}...", Color.FromArgb(52, 152, 219));

                    // 取得目錄中所有圖檔（按檔名排序）
                    var imageFiles = Directory.GetFiles(subDir)
                        .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                        .OrderBy(f => Path.GetFileName(f))
                        .ToList();

                    if (imageFiles.Count == 0)
                    {
                        totalProcessed++;
                        UpdateProgress(totalProcessed, subDirectories.Length);
                        continue;
                    }

                    // 過濾檔案
                    var filesToCopy = new List<string>();
                    bool isMainTabSource = !string.IsNullOrEmpty(mainTabSourceDirectory) && 
                                           subDir.Equals(mainTabSourceDirectory, StringComparison.OrdinalIgnoreCase);
                    
                    for (int i = 0; i < imageFiles.Count; i++)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(imageFiles[i]);
                        bool isLast = (i == imageFiles.Count - 1);

                        // 檢查是否為 main 或 tab
                        bool isMainOrTab = fileName.Equals("main", StringComparison.OrdinalIgnoreCase) ||
                                          fileName.Equals("tab", StringComparison.OrdinalIgnoreCase);

                        // 規則 1：main/tab 檔案只有在此目錄是指定來源時才複製
                        if (isMainOrTab)
                        {
                            if (!isMainTabSource)
                            {
                                continue; // 不是來源目錄，跳過 main/tab
                            }
                            // 是來源目錄，則加入複製清單
                            filesToCopy.Add(imageFiles[i]);
                            continue;
                        }

                        // 規則 2：排除每個目錄的最後一個檔案（非 main/tab 的情況）
                        if (isLast)
                        {
                            continue;
                        }

                        filesToCopy.Add(imageFiles[i]);
                    }

                    // 複製檔案到 ALL 目錄
                    foreach (var sourceFile in filesToCopy)
                    {
                        string fileName = Path.GetFileName(sourceFile);
                        string destFile = Path.Combine(allDirectory, fileName);
                        
                        // 如果目標檔案已存在，加上目錄前綴避免衝突
                        if (File.Exists(destFile))
                        {
                            string dirName = Path.GetFileName(subDir);
                            fileName = $"{dirName}_{fileName}";
                            destFile = Path.Combine(allDirectory, fileName);

                            // 檢查檔案是否已存在 (在加上目錄前綴後)
                            if (File.Exists(destFile))
                            {
                                // 詢問使用者如何處理
                                var result = MessageBox.Show(
                                    $"檔案已存在：{Path.GetFileName(sourceFile)}\n\n" +
                                    $"是否要覆蓋？\n\n" +
                                    $"是(Y) = 覆蓋\n" +
                                    $"否(N) = 跳過此檔案\n" +
                                    $"取消 = 停止整個複製作業",
                                    "檔案已存在",
                                    MessageBoxButtons.YesNoCancel,
                                    MessageBoxIcon.Question);

                                if (result == DialogResult.Cancel)
                                {
                                    UpdateStatus("使用者取消操作", Color.Orange);
                                    return; // 停止整個作業
                                }
                                else if (result == DialogResult.No)
                                {
                                    continue; // 跳過此檔案
                                }
                                // DialogResult.Yes 則繼續覆蓋
                            }
                        }

                        File.Copy(sourceFile, destFile, true);
                        totalCopied++;
                    }

                    totalProcessed++;
                    UpdateProgress(totalProcessed, subDirectories.Length);
                }
            }

            UpdateStatus($"完成！共複製 {totalCopied} 個圖檔到 ALL 目錄", Color.FromArgb(46, 204, 113));
            UpdateProgress(subDirectories.Length, subDirectories.Length);
            
            // 自動重新整理目錄狀態 (更新 ALL 資料夾的數量)
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LoadDirectoryInfo()));
            }
            else
            {
                LoadDirectoryInfo();
            }
        }

        private void ProcessZipFiles()
        {
            // 建立 ALL 目錄
            string allDirectory = Path.Combine(selectedDirectory, "ALL");
            if (!Directory.Exists(allDirectory))
            {
                Directory.CreateDirectory(allDirectory);
            }

            // 掃描所有ZIP檔案
            var zipFiles = Directory.GetFiles(selectedDirectory, "*.zip")
                .Where(f => !Path.GetFileName(f).Equals("ALL.zip", StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => Path.GetFileName(f))
                .ToList();

            if (zipFiles.Count == 0)
            {
                UpdateStatus("找不到任何 ZIP 壓縮檔", Color.FromArgb(231, 76, 60));
                return;
            }

            int totalCopied = 0;
            int mainTabCounter = 0;

            UpdateProgress(0, zipFiles.Count);
            UpdateStatus($"找到 {zipFiles.Count} 個 ZIP 檔案，開始處理...", Color.FromArgb(52, 152, 219));

            // 收集所有文件
            var allNumberedFiles = new List<(string tempPath, string originalName)>();
            var allMainTabFiles = new List<(string tempPath, string destName)>();

            // 處理每個ZIP
            for (int i = 0; i < zipFiles.Count; i++)
            {
                ProcessSingleZipArchive(zipFiles[i], allNumberedFiles, allMainTabFiles, ref mainTabCounter);
                UpdateProgress(i + 1, zipFiles.Count);
            }

            // 複製main/tab檔案
            foreach (var (tempPath, destName) in allMainTabFiles)
            {
                try
                {
                    string destPath = Path.Combine(allDirectory, destName);
                    File.Copy(tempPath, destPath, true);
                    File.Delete(tempPath);
                    totalCopied++;
                }
                catch { }
            }

            // 複製數字檔案
            // 策略：ZIP 批次處理時，為了避免不同 ZIP 間的檔名衝突（覆蓋），
            // 統一採用連續編號。勾選框僅決定「是否跳過 12 的倍數」。
            
            int newNumber = 1;
            foreach (var (tempPath, originalName) in allNumberedFiles)
            {
                try
                {
                    string ext = Path.GetExtension(tempPath);
                    string destPath = Path.Combine(allDirectory, $"{newNumber:D2}{ext}");
                    
                    // 複製檔案
                    File.Copy(tempPath, destPath, true);
                    File.Delete(tempPath);
                    
                    totalCopied++;
                    newNumber++;
                }
                catch { }
            }

            UpdateStatus($"完成！共從 {zipFiles.Count} 個 ZIP 檔案複製 {totalCopied} 個圖檔", Color.FromArgb(46, 204, 113));
            
            // 在UI線程上重新載入目錄資訊
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LoadDirectoryInfo()));
            }
            else
            {
                LoadDirectoryInfo();
            }
        }

        private void ProcessSingleZipArchive(string zipPath, 
            List<(string tempPath, string originalName)> numberedFiles,
            List<(string tempPath, string destName)> mainTabFiles,
            ref int mainTabCounter)
        {
            string zipName = Path.GetFileNameWithoutExtension(zipPath);
            UpdateStatus($"處理 ZIP：{zipName}...", Color.FromArgb(52, 152, 219));

            bool foundMainTab = false;
            int processedCount = 0;

            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    // 🔥 修改：直接處理所有圖檔 entries（不分目錄）
                    var allImageEntries = archive.Entries
                        .Where(e => !string.IsNullOrEmpty(e.Name)) // 排除目錄本身
                        .Where(e => ImageExtensions.Contains(Path.GetExtension(e.Name).ToLower()))
                        .ToList();

                    UpdateStatus($"ZIP {zipName}: 找到 {allImageEntries.Count} 個圖檔", Color.FromArgb(52, 152, 219));

                    if (allImageEntries.Count == 0)
                    {
                        UpdateStatus($"ZIP {zipName}: 沒有圖檔", Color.FromArgb(231, 76, 60));
                        return;
                    }

                    // 按目錄分組處理
                    var groupedByDirectory = allImageEntries
                        .GroupBy(e => Path.GetDirectoryName(e.FullName) ?? "")
                        .ToList();

                    foreach (var group in groupedByDirectory)
                    {
                        // 對每個目錄內的檔案排序
                        var imageEntries = group
                            .OrderBy(e =>
                            {
                                string nameWithoutExt = Path.GetFileNameWithoutExtension(e.Name);
                                if (int.TryParse(nameWithoutExt, out int number))
                                    return number;
                                return int.MaxValue;
                            })
                            .ThenBy(e => e.Name)
                            .ToList();

                        // 處理每個圖檔
                        for (int i = 0; i < imageEntries.Count; i++)
                        {
                            var entry = imageEntries[i];
                            string fileName = Path.GetFileNameWithoutExtension(entry.Name);
                            bool isLast = (i == imageEntries.Count - 1);

                            // 檢查是否為 main 或 tab
                            bool isMainOrTab = fileName.Equals("main", StringComparison.OrdinalIgnoreCase) ||
                                               fileName.Equals("tab", StringComparison.OrdinalIgnoreCase);

                            if (isMainOrTab)
                            {
                                // 自動編號
                                string suffix = mainTabCounter == 0 ? "" : mainTabCounter.ToString();
                                string destName = fileName + suffix + Path.GetExtension(entry.Name);
                                
                                // 提取到臨時文件
                                using (Stream entryStream = entry.Open())
                                using (var ms = new MemoryStream())
                                {
                                    entryStream.CopyTo(ms);
                                    string tempPath = Path.Combine(Path.GetTempPath(), 
                                        Guid.NewGuid().ToString() + Path.GetExtension(entry.Name));
                                    File.WriteAllBytes(tempPath, ms.ToArray());
                                    mainTabFiles.Add((tempPath, destName));
                                    processedCount++;
                                }
                                foundMainTab = true;
                                continue;
                            }

                            // 跳過最後一個檔案（非 main/tab）
                            if (isLast) continue;

                            // 過濾12倍數
                            if (chkFilterMultipleOf12.Checked && IsMultipleOf12Filename(entry.Name))
                                continue;

                            // 提取到臨時文件
                            using (Stream entryStream = entry.Open())
                            using (var ms = new MemoryStream())
                            {
                                entryStream.CopyTo(ms);
                                string tempPath = Path.Combine(Path.GetTempPath(),
                                    Guid.NewGuid().ToString() + Path.GetExtension(entry.Name));
                                File.WriteAllBytes(tempPath, ms.ToArray());
                                numberedFiles.Add((tempPath, entry.Name));
                                processedCount++;
                            }
                        }
                    }
                }

                UpdateStatus($"ZIP {zipName}: 已提取 {processedCount} 個圖檔", Color.FromArgb(46, 204, 113));

                // 如果這個ZIP有main/tab，增加計數器
                if (foundMainTab)
                    mainTabCounter++;
            }
            catch (Exception ex)
            {
                UpdateStatus($"處理 {zipName} 時發生錯誤：{ex.Message}", Color.FromArgb(231, 76, 60));
            }
        }

        private bool IsMultipleOf12Filename(string filename)
        {
            // 取得不含副檔名的檔名
            string nameWithoutExt = Path.GetFileNameWithoutExtension(filename);

            // 嘗試轉換為數字
            if (int.TryParse(nameWithoutExt, out int number))
            {
                // 判斷是否為12的倍數（且大於0）
                return number % 12 == 0 && number > 0;
            }

            return false;
        }

        private void CleanAndRenumberFiles()
        {
            string allDir = Path.Combine(selectedDirectory, "ALL");

            if (!Directory.Exists(allDir))
            {
                MessageBox.Show("ALL 目錄不存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1. 掃描所有圖檔並排序
            var allFiles = Directory.GetFiles(allDir, "*.*")
                .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                .Where(f => int.TryParse(Path.GetFileNameWithoutExtension(f), out _))
                .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                .ToList();

            // 2. 找出12倍數的檔案
            var filesToDelete = allFiles
                .Where(f => IsMultipleOf12Filename(Path.GetFileName(f)))
                .ToList();

            if (filesToDelete.Count == 0)
            {
                MessageBox.Show("沒有找到12倍數的檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 3. 確認對話框
            var result = MessageBox.Show(
                $"找到 {filesToDelete.Count} 個12倍數檔案\n" +
                $"刪除後將重新編號所有檔案\n" +
                $"這個操作無法復原，確定要繼續嗎？",
                "確認刪除並重新編號",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                // 4. 刪除12倍數檔案
                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }

                // 5. 重新掃描剩餘檔案（已刪除12倍數）
                var remainingFiles = Directory.GetFiles(allDir, "*.*")
                    .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                    .Where(f => int.TryParse(Path.GetFileNameWithoutExtension(f), out _))
                    .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                    .ToList();

                // 6. 先重新命名為臨時名稱（避免衝突）
                var tempFiles = new List<string>();
                for (int i = 0; i < remainingFiles.Count; i++)
                {
                    string ext = Path.GetExtension(remainingFiles[i]);
                    string tempName = Path.Combine(allDir, $"temp_{i:D4}{ext}");
                    File.Move(remainingFiles[i], tempName);
                    tempFiles.Add(tempName);
                }

                // 7. 重新命名為正確編號（從1開始）
                for (int i = 0; i < tempFiles.Count; i++)
                {
                    string ext = Path.GetExtension(tempFiles[i]);
                    string newName = Path.Combine(allDir, $"{(i + 1):D2}{ext}");
                    File.Move(tempFiles[i], newName);
                }

                // 8. 更新UI
                LoadDirectoryInfo();
                MessageBox.Show(
                    $"已完成！\n刪除 {filesToDelete.Count} 個檔案\n重新編號 {remainingFiles.Count} 個檔案",
                    "完成",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message, Color color)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() =>
                {
                    lblStatus.Text = message;
                    lblStatus.ForeColor = color;
                }));
            }
            else
            {
                lblStatus.Text = message;
                lblStatus.ForeColor = color;
            }
        }

        private void chkProcessZip_CheckedChanged(object sender, EventArgs e)
        {
            // 當勾選/取消勾選 ZIP 模式時，重新載入目錄列表
            if (!string.IsNullOrEmpty(selectedDirectory))
            {
                LoadDirectoryInfo();
            }
        }

        private void UpdateProgress(int current, int total)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() =>
                {
                    progressBar.Maximum = total;
                    progressBar.Value = Math.Min(current, total);
                }));
            }
            else
            {
                progressBar.Maximum = total;
                progressBar.Value = Math.Min(current, total);
            }
        }
        private async void tvDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null) return;

            string subDirPath = e.Node.Tag as string;
            if (string.IsNullOrEmpty(subDirPath) || !Directory.Exists(subDirPath)) return;

            try
            {
                await LoadThumbnailsAsync(subDirPath);
            }
            catch (Exception ex)
            {
                UpdateStatus($"無法載入預覽圖: {ex.Message}", Color.Red);
            }
        }

        private void menuSetMainTabSource_Click(object sender, EventArgs e)
        {
            if (tvDirectories.SelectedNode == null || tvDirectories.SelectedNode.Tag == null) return;

            mainTabSourceDirectory = tvDirectories.SelectedNode.Tag as string;
            
            // 重新載入目錄資訊以更新視覺效果
            LoadDirectoryInfo();
            
            UpdateStatus($"已設定 Main/Tab 來源：{Path.GetFileName(mainTabSourceDirectory)}", Color.FromArgb(41, 128, 185));
        }

        private void menuClearMainTabSource_Click(object sender, EventArgs e)
        {
            mainTabSourceDirectory = "";
            
            // 重新載入目錄資訊以更新視覺效果
            LoadDirectoryInfo();
            
            UpdateStatus("已清除 Main/Tab 來源設定", Color.Gray);
        }

        private void contextMenuTreeView_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 根據選中的節點決定顯示哪些選單項目
            if (tvDirectories.SelectedNode == null || tvDirectories.SelectedNode.Tag == null)
            {
                e.Cancel = true;
                return;
            }

            string selectedPath = tvDirectories.SelectedNode.Tag as string;
            bool isAllDirectory = selectedPath != null && 
                                  Path.GetFileName(selectedPath).Equals("ALL", StringComparison.OrdinalIgnoreCase);

            // 如果是 ALL 目錄，只顯示清空選項
            menuSetMainTabSource.Visible = !isAllDirectory;
            menuClearMainTabSource.Visible = !isAllDirectory;
            menuSeparator.Visible = !isAllDirectory;
            menuClearAllDirectory.Visible = isAllDirectory;
            menuCleanMultipleOf12.Visible = isAllDirectory; // 新增：12倍數清理選項也只在 ALL 目錄顯示
            menuRenumberAllFiles.Visible = isAllDirectory; // 新增：重新編號選項也只在 ALL 目錄顯示
        }

        private void menuClearAllDirectory_Click(object sender, EventArgs e)
        {
            if (tvDirectories.SelectedNode == null || tvDirectories.SelectedNode.Tag == null) return;

            string allDirPath = tvDirectories.SelectedNode.Tag as string;
            if (string.IsNullOrEmpty(allDirPath) || !Directory.Exists(allDirPath)) return;

            // 確認是 ALL 目錄
            if (!Path.GetFileName(allDirPath).Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("此功能僅適用於 ALL 目錄", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 詢問使用者確認
            var files = Directory.GetFiles(allDirPath);
            if (files.Length == 0)
            {
                MessageBox.Show("ALL 目錄已經是空的", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"確定要刪除 ALL 目錄中的所有 {files.Length} 個檔案嗎？\n\n此操作無法復原！",
                "確認刪除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                int deletedCount = 0;
                foreach (var file in files)
                {
                    File.Delete(file);
                    deletedCount++;
                }

                UpdateStatus($"已刪除 {deletedCount} 個檔案", Color.FromArgb(39, 174, 96));
                
                // 重新載入目錄資訊以更新顯示
                LoadDirectoryInfo();
                
                MessageBox.Show($"成功刪除 {deletedCount} 個檔案", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刪除檔案時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuCleanMultipleOf12_Click(object sender, EventArgs e)
        {
            CleanAndRenumberFiles();
        }

        private void menuRenumberAllFiles_Click(object sender, EventArgs e)
        {
            RenumberAllFiles();
        }

        private void RenumberAllFiles()
        {
            string allDir = Path.Combine(selectedDirectory, "ALL");

            if (!Directory.Exists(allDir))
            {
                MessageBox.Show("ALL 目錄不存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1. 掉描所有的數字檔名
            var numberFiles = Directory.GetFiles(allDir, "*.*")
                .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                .Where(f => int.TryParse(Path.GetFileNameWithoutExtension(f), out _))
                .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                .ToList();

            if (numberFiles.Count == 0)
            {
                MessageBox.Show("沒有找到可重新編號的檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. 確認對話框
            var result = MessageBox.Show(
                $"找到 {numberFiles.Count} 個數字檔名\n" +
                $"將重新編號為 1, 2, 3...\n" +
                $"這個操作無法復原，確定要繼續嗎？",
                "確認重新編號",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                // 3. 先重新命名為臨時名稱（避免衝突）
                var tempFiles = new List<string>();
                for (int i = 0; i < numberFiles.Count; i++)
                {
                    string ext = Path.GetExtension(numberFiles[i]);
                    string tempName = Path.Combine(allDir, $"temp_{i:D4}{ext}");
                    File.Move(numberFiles[i], tempName);
                    tempFiles.Add(tempName);
                }

                // 4. 重新命名為正確編號（從1開始）
                for (int i = 0; i < tempFiles.Count; i++)
                {
                    string ext = Path.GetExtension(tempFiles[i]);
                    string newName = Path.Combine(allDir, $"{(i + 1):D2}{ext}");
                    File.Move(tempFiles[i], newName);
                }

                // 5. 更新UI
                LoadDirectoryInfo();
                MessageBox.Show(
                    $"已完成！\n重新編號 {numberFiles.Count} 個檔案",
                    "完成",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadThumbnailsAsync(string path)
        {
            lvImages.Items.Clear();
            imgListThumbnails.Images.Clear();
            
            var imageFiles = Directory.GetFiles(path)
                .Where(f => ImageExtensions.Contains(Path.GetExtension(f).ToLower()))
                .ToList();
            
            // 🔥 修正：按檔名數字排序（如果是數字檔名）
            imageFiles = imageFiles.OrderBy(f =>
            {
                string nameWithoutExt = Path.GetFileNameWithoutExtension(f);
                // 如果是純數字檔名，按數字排序
                if (int.TryParse(nameWithoutExt, out int number))
                {
                    return number;
                }
                // 否則按字串排序（main, tab 等）
                return int.MaxValue; // 讓 main/tab 排在最後
            }).ThenBy(f => Path.GetFileName(f)).ToList();

            if (imageFiles.Count == 0) return;

            // 限制預覽數量，避免過多消耗資源（例如前 50 張）
            int previewCount = Math.Min(imageFiles.Count, 50);

            await Task.Run(() =>
            {
                for (int i = 0; i < previewCount; i++)
                {
                    string file = imageFiles[i];
                    try
                    {
                        using (var img = Image.FromFile(file))
                        {
                            var thumb = CreateHighQualityThumbnail(img, 120, 120);
                            
                            this.Invoke((MethodInvoker)delegate
                            {
                                imgListThumbnails.Images.Add(Path.GetFileName(file), thumb);
                                var item = new ListViewItem(Path.GetFileName(file));
                                item.ImageKey = Path.GetFileName(file);
                                lvImages.Items.Add(item);
                            });
                        }
                    }
                    catch
                    {
                        // 略過損毀的圖檔
                    }
                }
            });
        }

        private Image CreateHighQualityThumbnail(Image image, int width, int height)
        {
            var res = new Bitmap(width, height);
            using (var g = Graphics.FromImage(res))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // 保持比例縮放
                double ratio = Math.Min((double)width / image.Width, (double)height / image.Height);
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                int posX = (width - newWidth) / 2;
                int posY = (height - newHeight) / 2;

                g.Clear(Color.White); // 背景填白
                g.DrawImage(image, posX, posY, newWidth, newHeight);
            }
            return res;
        }
    }
}
