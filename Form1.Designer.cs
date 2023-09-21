namespace DMIExtractorV2
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
            ExtractionProgressView_OutputFiles = new ProgressBar();
            FolderStructureView = new TreeView();
            ExtractBtn = new Button();
            DataPath = new TextBox();
            label1 = new Label();
            DataPathBtn = new Button();
            label2 = new Label();
            OutputPath = new TextBox();
            OutputPathBtn = new Button();
            CheckBtn = new Button();
            ExtractionProgressView_DataFiles = new ProgressBar();
            SuspendLayout();
            // 
            // ExtractionProgressView_OutputFiles
            // 
            ExtractionProgressView_OutputFiles.Location = new Point(12, 415);
            ExtractionProgressView_OutputFiles.Name = "ExtractionProgressView_OutputFiles";
            ExtractionProgressView_OutputFiles.Size = new Size(776, 23);
            ExtractionProgressView_OutputFiles.Step = 1;
            ExtractionProgressView_OutputFiles.TabIndex = 0;
            // 
            // FolderStructureView
            // 
            FolderStructureView.Location = new Point(12, 102);
            FolderStructureView.Name = "FolderStructureView";
            FolderStructureView.Size = new Size(776, 278);
            FolderStructureView.TabIndex = 1;
            // 
            // ExtractBtn
            // 
            ExtractBtn.Location = new Point(683, 27);
            ExtractBtn.Name = "ExtractBtn";
            ExtractBtn.Size = new Size(105, 69);
            ExtractBtn.TabIndex = 2;
            ExtractBtn.Text = "Extract all";
            ExtractBtn.UseVisualStyleBackColor = true;
            ExtractBtn.Click += ExtractBtn_Click;
            // 
            // DataPath
            // 
            DataPath.Location = new Point(12, 27);
            DataPath.Name = "DataPath";
            DataPath.Size = new Size(527, 23);
            DataPath.TabIndex = 3;
            DataPath.TextChanged += DataPath_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 4;
            label1.Text = "Original data path:";
            // 
            // DataPathBtn
            // 
            DataPathBtn.Location = new Point(545, 27);
            DataPathBtn.Name = "DataPathBtn";
            DataPathBtn.Size = new Size(30, 23);
            DataPathBtn.TabIndex = 5;
            DataPathBtn.Text = "...";
            DataPathBtn.UseVisualStyleBackColor = true;
            DataPathBtn.Click += DataPathBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 6;
            label2.Text = "Output path:";
            // 
            // OutputPath
            // 
            OutputPath.Location = new Point(12, 73);
            OutputPath.Name = "OutputPath";
            OutputPath.Size = new Size(527, 23);
            OutputPath.TabIndex = 7;
            OutputPath.TextChanged += OutputPath_TextChanged;
            // 
            // OutputPathBtn
            // 
            OutputPathBtn.Location = new Point(545, 73);
            OutputPathBtn.Name = "OutputPathBtn";
            OutputPathBtn.Size = new Size(30, 23);
            OutputPathBtn.TabIndex = 8;
            OutputPathBtn.Text = "...";
            OutputPathBtn.UseVisualStyleBackColor = true;
            OutputPathBtn.Click += OutputPathBtn_Click;
            // 
            // CheckBtn
            // 
            CheckBtn.Location = new Point(581, 27);
            CheckBtn.Name = "CheckBtn";
            CheckBtn.Size = new Size(96, 69);
            CheckBtn.TabIndex = 9;
            CheckBtn.Text = "Check";
            CheckBtn.UseVisualStyleBackColor = true;
            CheckBtn.Click += CheckBtn_Click;
            // 
            // ExtractionProgressView_DataFiles
            // 
            ExtractionProgressView_DataFiles.Location = new Point(12, 386);
            ExtractionProgressView_DataFiles.Name = "ExtractionProgressView_DataFiles";
            ExtractionProgressView_DataFiles.Size = new Size(776, 23);
            ExtractionProgressView_DataFiles.Step = 1;
            ExtractionProgressView_DataFiles.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ExtractionProgressView_DataFiles);
            Controls.Add(CheckBtn);
            Controls.Add(OutputPathBtn);
            Controls.Add(OutputPath);
            Controls.Add(label2);
            Controls.Add(DataPathBtn);
            Controls.Add(label1);
            Controls.Add(DataPath);
            Controls.Add(ExtractBtn);
            Controls.Add(FolderStructureView);
            Controls.Add(ExtractionProgressView_OutputFiles);
            MaximizeBox = false;
            MaximumSize = new Size(816, 489);
            MinimizeBox = false;
            MinimumSize = new Size(816, 489);
            Name = "Form1";
            Text = "DMI Extractor V2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar ExtractionProgressView_OutputFiles;
        private TreeView FolderStructureView;
        private Button ExtractBtn;
        private TextBox DataPath;
        private Label label1;
        private Button DataPathBtn;
        private Label label2;
        private TextBox OutputPath;
        private Button OutputPathBtn;
        private Button CheckBtn;
        private ProgressBar ExtractionProgressView_DataFiles;
    }
}