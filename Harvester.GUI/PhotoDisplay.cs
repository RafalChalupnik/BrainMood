using System;
using System.Windows.Forms;

namespace BrainMood.Harvester.GUI
{
    public partial class PhotoDisplay : Form
    {
        private int m_timeToShow;

        public PhotoDisplay(string photoFileName, int timeToShow)
        {
            m_timeToShow = timeToShow;

            InitializeComponent();
            pictureBox.ImageLocation = photoFileName;
            formTimer.Interval = 1000;
        }

        private void PhotoDisplay_Shown(object sender, EventArgs e)
        {
            formTimer.Start();
        }

        private void formTimer_Tick(object sender, EventArgs e)
        {
            m_timeToShow -= 1;
            timeLeftLabel.Text = $"Time left: {m_timeToShow}";

            if (m_timeToShow <= 0)
            {
                Close();
            }
        }
    }
}
