using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrainMood.Harvester.GUI
{
    public partial class Startup : Form
    {
        public Startup()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            var outputDirectory = outputDirectoryTextBox.Text;
            var dataDirectory = dataDirectoryTextBox.Text;
            var samplesPerEmotion = int.Parse(samplesPerEmotionTextBox.Text);
            var timePerSample = int.Parse(timePerSampleTextBox.Text);
            var breakTime = int.Parse(breakTimeTextBox.Text);

            var sessionConfig = new SessionConfig(
                outputDirectory,
                dataDirectory,
                samplesPerEmotion,
                timePerSample,
                breakTime);

            var session = new Session(sessionConfig);

            Hide();
            Task.Run(() => session.Run());
            Show();
        }
    }
}
