using System;
using System.Windows.Forms;

using SpawnConverter.Logs;
using SpawnConverter.FStream;
using SpawnConverter.Converters;
using SpawnConverter.Levels;

namespace SpawnConverter
{
    public partial class Main : Form
    {
        private int save_selected = 0;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Visible = false;
            Enabled = true;

            InitializeSetting();
        }

        private void InitializeSetting()
        {
            Logger.CreateLog();
            Logger.OnError += OnError;

            Settings.Load();

            if (ValidatePath())
            {
                FillListComboBox();
            }
        }
        private bool ValidatePath()
        {
            var result = FilePath.Validation();

            if(result != FilePath.RESULT.ALL_VALID)
            {
                string message = FilePath.GetErrorsMessage(result);
                Logger.SendError(null, new LogEventArgs(CODES.NOT_DIRECTORY, message, "ValidatePath", 47, "Main.cs"));
                return false;
            }

            return true;
        }
        private void FillListComboBox()
        {
            using LevelCollection level = new();

            foreach (var name in level.Levels)
            {
                LevelComboBox.Items.Add(name);
            }

            LevelComboBox.SelectedIndex = 0;
            LevelComboBox.Enabled = true;
            LevelAllCheckBox.Enabled = true;
            GoButton.Enabled = true;
        }
        private async void StartConvert()
        {
            string name = LevelAllCheckBox.Checked ? string.Empty : LevelComboBox.SelectedItem.ToString();
            bool result = await Converter.Run(name);

            string message = result ? "Complete" : "Failed";
            _ = MessageBox.Show(message);

            ControlsToggle();
        }
        private void ControlsToggle()
        {
            GoButton.Enabled = !GoButton.Enabled;
            LevelAllCheckBox.Enabled = !LevelAllCheckBox.Enabled;
            LevelComboBox.Enabled = !LevelAllCheckBox.Checked && !LevelComboBox.Enabled;
        }

        #region Events
        private void GoButton_Click(object sender, EventArgs e)
        {
            ControlsToggle();
            StartConvert();
        }
        private void OnError(object sender, LogEventArgs e)
        {
            string message = Logger.FormatErrorMessage(sender, e, true);
            _ = MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Close();
        }
        private void LevelAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            save_selected = LevelAllCheckBox.Checked ? LevelComboBox.SelectedIndex : save_selected;

            LevelComboBox.Enabled = !LevelAllCheckBox.Checked;
            LevelComboBox.SelectedIndex = LevelAllCheckBox.Checked ? -1 : save_selected;
        }
        #endregion
    }
}
