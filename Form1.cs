using DMISharp;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using XmpCore.Impl.XPath;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace DMIExtractorV2
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// This represents the number of files that need extraction (only dmi files)
        /// </summary>
        private int NumberOfDmiFiles = 0;
        /// <summary>
        /// This represents the number of files that need to be moved (only png and gif files)
        /// </summary>
        private int NumberOfAssetFiles = 0;

        /// <summary>
        /// This represents the path that the extraction will pull from
        /// </summary>
        private string DataPathString = string.Empty;
        /// <summary>
        /// This represents the path that the extraction will put files into
        /// </summary>
        private string OutputPathString = string.Empty;

        private ExtractorState State = ExtractorState.UNINITIALIZED;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {

            CheckStateUpdate();
            base.OnLoad(e);
        }

        private static List<string>? GetDMIFileNames(string dmiFile)
        {
            if (!File.Exists(dmiFile))
            {
                return null;
            }
            using var dmi = new DMIFile(dmiFile);
            List<string> files = new();

            foreach (var state in dmi.States)
            {
                if (state.Dirs > 1)
                {
                    foreach (StateDirection direction in Enum.GetValues(typeof(StateDirection)))
                    {
                        if (state.IsAnimated())
                        {
                            if (string.IsNullOrEmpty(state.Name))
                            {
                                files.Add($"(DMI) {Path.GetFileName(dmiFile).Split('.')[0]} {Enum.GetName(typeof(StateDirection), direction)}.gif");
                            }
                            else
                            {
                                files.Add($"(DMI) {state.Name} {Enum.GetName(typeof(StateDirection), direction)}.gif");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(state.Name))
                            {
                                files.Add($"(DMI) {Path.GetFileName(dmiFile).Split('.')[0]} {Enum.GetName(typeof(StateDirection), direction)}.png");
                            }
                            else
                            {
                                files.Add($"(DMI) {state.Name} {Enum.GetName(typeof(StateDirection), direction)}.png");
                            }
                        }
                    }
                }
                else
                {
                    if (state.IsAnimated())
                    {
                        if (string.IsNullOrEmpty(state.Name))
                        {
                            files.Add($"(DMI) {Path.GetFileName(dmiFile).Split('.')[0]}.gif");
                        }
                        else
                        {
                            files.Add($"(DMI) {state.Name}.gif");
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(state.Name))
                        {
                            files.Add($"(DMI) {Path.GetFileName(dmiFile).Split('.')[0]}.png");
                        }
                        else
                        {
                            files.Add($"(DMI) {state.Name}.png");
                        }
                    }
                }
            }

            return files;
        }

        private void PopulateFolderViewWithFiles(string rootDirectory, TreeNode node)
        {
            string[] files = Directory.GetFiles(rootDirectory);

            foreach (string file in files)
            {
                TreeNode fileNode = new(Path.GetFileName(file));
                switch (Path.GetExtension(file))
                {
                    case ".dmi":
                        node.Nodes.Add(fileNode);
                        var dmi = GetDMIFileNames(file);
                        if (dmi == null) { continue; }
                        NumberOfDmiFiles++;

                        foreach (var dmiFile in dmi)
                        {
                            TreeNode dmiNode = new(dmiFile);
                            fileNode.Nodes.Add(dmiNode);
                        }
                        break;

                    case ".png":
                    case ".gif":
                        node.Nodes.Add(fileNode);
                        NumberOfAssetFiles++;
                        break;
                }
            }
        }

        /// <summary>
        /// Recursive function to populate the folder view with all the dmi, gif and png files from the data path.
        /// </summary>
        /// <param name="rootDirectory">The root to look into</param>
        /// <param name="parentNode">The parent node for the search</param>
        private void PopulateFolderViewWithFolders(string rootDirectory, TreeNode parentNode)
        {
            string[] subDirectories = Directory.GetDirectories(rootDirectory);

            foreach (string directory in subDirectories)
            {
                TreeNode childNode = new(directory.Split('\\').Last());
                parentNode.Nodes.Add(childNode);
                PopulateFolderViewWithFolders(directory, childNode);
                PopulateFolderViewWithFiles(directory, childNode);
            }
        }

        /// <summary>
        /// Starts the population of the folder view, and prepares all the data
        /// </summary>
        /// <param name="root"></param>
        private void StartPopulateFolderView(string root)
        {
            FolderStructureView.Nodes.Clear();


            TreeNode rootNode = new(root.Split('\\').Last());
            FolderStructureView.Nodes.Add(rootNode);
            PopulateFolderViewWithFolders(root, rootNode);
        }

        private void DataPathBtn_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "Select a folder:";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DataPathString = folderBrowserDialog.SelectedPath;
                DataPath.Text = DataPathString; // Display selected folder path
                CheckStateForPaths();
            }
        }

        private void OutputPathBtn_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "Select a folder:";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                OutputPathString = folderBrowserDialog.SelectedPath;
                OutputPath.Text = OutputPathString; // Display selected folder path
                CheckStateForPaths();
            }
        }

        private void DataPath_TextChanged(object sender, EventArgs e)
        {
            DataPathString = DataPath.Text;
            CheckStateForPaths();
        }

        private void OutputPath_TextChanged(object sender, EventArgs e)
        {
            OutputPathString = OutputPath.Text;
            CheckStateForPaths();
        }

        private void CheckStateForPaths()
        {
            if (string.IsNullOrWhiteSpace(DataPathString) || string.IsNullOrWhiteSpace(OutputPathString))
                State = ExtractorState.UNINITIALIZED;
            else
                State = ExtractorState.FILEPATHS_SET;
            CheckStateUpdate();
        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            NumberOfAssetFiles = 0;
            NumberOfDmiFiles = 0;
            State = ExtractorState.CHECKING;
            CheckStateUpdate();

            StartPopulateFolderView(DataPathString);

            if (NumberOfAssetFiles > 0 || NumberOfDmiFiles > 0)
            {
                State = ExtractorState.READY_TO_EXTRACT;
            }
            else
            {
                State = ExtractorState.FILEPATHS_SET;
            }
            CheckStateUpdate();
        }

        private void ExtractBtn_Click(object sender, EventArgs e)
        {
            State = ExtractorState.EXTRACTING;

            CheckStateUpdate();

            Task.Run(ExtractionTask);
        }

        private async Task ExtractionTask()
        {
            await SetupFolders(DataPathString);

            SetExtractionProgressView_DataFilesMax(NumberOfDmiFiles);

            string[] dmiFiles = Directory.GetFiles(DataPathString, "*.dmi", SearchOption.AllDirectories);

            Trace.WriteLine($"Found {dmiFiles.Length} dmis");

            foreach (string dmiFile in dmiFiles)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string dmi_to_dir = Path.Combine(Path.GetDirectoryName(dmiFile), Path.GetFileNameWithoutExtension(dmiFile));
#pragma warning restore CS8604 // Possible null reference argument.
                string relative = Path.GetRelativePath(DataPathString, dmi_to_dir);
                string target = Path.Join(OutputPathString, relative);
                Directory.CreateDirectory(target);
                await ExtractDmiFile(dmiFile, target);
                StepExtractionProgressView_DataFiles();
            }

            SetExtractionProgressView_OutputFilesMax(NumberOfAssetFiles);

            string[] pngFiles = Directory.GetFiles(DataPathString, "*.png", SearchOption.AllDirectories);

            Trace.WriteLine($"Found {pngFiles.Length} pngs");

            foreach (string pngFile in pngFiles)
            {
                string relative = Path.GetRelativePath(DataPathString, pngFile);
                string target = Path.Join(OutputPathString, relative);
                File.Copy(pngFile, target);
                StepExtractionProgressView_OutputFiles();
            }

            string[] gifFiles = Directory.GetFiles(DataPathString, "*.gif", SearchOption.AllDirectories);

            Trace.WriteLine($"Found {gifFiles.Length} gifs");

            foreach (string gifFile in gifFiles)
            {
                string relative = Path.GetRelativePath(DataPathString, gifFile);
                string target = Path.Join(OutputPathString, relative);
                File.Copy(gifFile, target);
                StepExtractionProgressView_OutputFiles();
            }

            State = ExtractorState.DONE;
            CheckStateUpdate();
        }

        private async Task SetupFolders(string root)
        {
            string[] subDirs = Directory.GetDirectories(root);

            foreach (string subDir in subDirs)
            {
                string RelativeFolder = Path.GetRelativePath(DataPathString, subDir);
                string TargetFolder = Path.Join(OutputPathString, RelativeFolder);
                Directory.CreateDirectory(TargetFolder);
                await SetupFolders(subDir);
            }
        }

        private Task ExtractDmiFile(string dmiFile, string targetDirectory)
        {
            using var dmi = new DMIFile(dmiFile);

            SetExtractionProgressView_OutputFilesMax(dmi.States.Count);

            foreach (var state in dmi.States)
            {
                try
                {
                    if (state.Dirs > 1)
                    {
                        foreach (StateDirection direction in Enum.GetValues(typeof(StateDirection)))
                        {
                            if (state.IsAnimated())
                            {
                                if (string.IsNullOrEmpty(state.Name))
                                {
                                    string file = $"{Path.GetFileName(dmiFile).Split('.')[0]} {Enum.GetName(typeof(StateDirection), direction)}.gif";
                                    state.GetAnimated(direction);
                                    using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                    state.SaveAnimatedGIF(stream, direction);
                                }
                                else
                                {
                                    string file = $"{state.Name} {Enum.GetName(typeof(StateDirection), direction)}.gif";
                                    state.GetAnimated(direction);
                                    using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                    state.SaveAnimatedGIF(stream, direction);
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(state.Name))
                                {
                                    string file = $"{Path.GetFileName(dmiFile).Split('.')[0]} {Enum.GetName(typeof(StateDirection), direction)}.png";
                                    var image = state.GetFrame(direction, 0);
                                    using var stream = File.OpenWrite(Path.Join(targetDirectory, file));


                                    image?.Save(stream, new PngEncoder());
                                }
                                else
                                {
                                    string file = $"{state.Name} {Enum.GetName(typeof(StateDirection), direction)}.png";
                                    var image = state.GetFrame(direction, 0);
                                    using var stream = File.OpenWrite(Path.Join(targetDirectory, file));


                                    image?.Save(stream, new PngEncoder());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (state.IsAnimated())
                        {
                            if (string.IsNullOrEmpty(state.Name))
                            {
                                string file = $"{Path.GetFileName(dmiFile).Split('.')[0]}.gif";
                                using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                state.SaveAnimatedGIF(stream, StateDirection.South);
                            }
                            else
                            {
                                string file = $"{state.Name}.gif";
                                using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                state.SaveAnimatedGIF(stream, StateDirection.South);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(state.Name))
                            {
                                string file = $"{Path.GetFileName(dmiFile).Split('.')[0]}.png";
                                using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                var image = state.GetFrame(0);

                                image?.Save(stream, new PngEncoder());
                            }
                            else
                            {
                                string file = $"{state.Name}.png";
                                using var stream = File.OpenWrite(Path.Join(targetDirectory, file));

                                var image = state.GetFrame(0);

                                image?.Save(stream, new PngEncoder());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }

                StepExtractionProgressView_OutputFiles();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Task safe method for checking the state and updating the controls
        /// </summary>
        private void CheckStateUpdate()
        {
            Invoke(() =>
            {
                switch (State)
                {
                    case ExtractorState.UNINITIALIZED:
                        DataPathBtn.Enabled = true;
                        OutputPathBtn.Enabled = true;
                        DataPath.Enabled = true;
                        OutputPath.Enabled = true;
                        CheckBtn.Enabled = false;
                        ExtractBtn.Enabled = false;
                        break;

                    case ExtractorState.FILEPATHS_SET:
                        DataPathBtn.Enabled = true;
                        OutputPathBtn.Enabled = true;
                        DataPath.Enabled = true;
                        OutputPath.Enabled = true;
                        CheckBtn.Enabled = true;
                        ExtractBtn.Enabled = false;
                        break;

                    case ExtractorState.CHECKING:
                        DataPathBtn.Enabled = false;
                        OutputPathBtn.Enabled = false;
                        DataPath.Enabled = false;
                        OutputPath.Enabled = false;
                        CheckBtn.Enabled = false;
                        ExtractBtn.Enabled = false;
                        break;

                    case ExtractorState.READY_TO_EXTRACT:
                        DataPathBtn.Enabled = true;
                        OutputPathBtn.Enabled = true;
                        DataPath.Enabled = true;
                        OutputPath.Enabled = true;
                        CheckBtn.Enabled = true;
                        ExtractBtn.Enabled = true;
                        break;

                    case ExtractorState.EXTRACTING:
                        DataPathBtn.Enabled = false;
                        OutputPathBtn.Enabled = false;
                        DataPath.Enabled = false;
                        OutputPath.Enabled = false;
                        CheckBtn.Enabled = false;
                        ExtractBtn.Enabled = false;
                        break;

                    case ExtractorState.DONE:
                        DataPathBtn.Enabled = true;
                        OutputPathBtn.Enabled = true;
                        DataPath.Enabled = true;
                        OutputPath.Enabled = true;
                        CheckBtn.Enabled = true;
                        ExtractBtn.Enabled = false;
                        break;
                }
            });
        }

        /// <summary>
        /// Task safe version of stepping the progress
        /// </summary>
        private void StepExtractionProgressView_OutputFiles() => Invoke(ExtractionProgressView_OutputFiles.PerformStep);

        /// <summary>
        /// Task safe version of stepping the progress
        /// </summary>
        private void StepExtractionProgressView_DataFiles() => Invoke(ExtractionProgressView_DataFiles.PerformStep);

        /// <summary>
        /// Task safe version of setting the progress values
        /// </summary>
        /// <param name="max">The max value for the progress</param>
        private void SetExtractionProgressView_DataFilesMax(int max)
        {
            Invoke(() =>
            {
                ExtractionProgressView_DataFiles.Value = 0;
                ExtractionProgressView_DataFiles.Maximum = max;
            });
        }

        /// <summary>
        /// Task safe version of setting the progress values
        /// </summary>
        /// <param name="max">The max value for the progress</param>
        private void SetExtractionProgressView_OutputFilesMax(int max)
        {
            Invoke(() =>
            {
                ExtractionProgressView_OutputFiles.Value = 0;
                ExtractionProgressView_OutputFiles.Maximum = max;
            });
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.