using System;
using System.Windows.Forms;

namespace BrainMood.Harvester.GUI
{
    public partial class Break : Form
    {
        private int m_timeRemaining;

        public Break(int time)
        {
            m_timeRemaining = time;
            InitializeComponent();
        }

        private void Break_Shown(object sender, EventArgs e)
        {
            formTimer.Start();
        }

        private void formTimer_Tick(object sender, EventArgs e)
        {
            m_timeRemaining--;
            timeLeftLabel.Text = $"Time left: {m_timeRemaining}";

            if (m_timeRemaining <= 0)
            {
                Close();
            }
        }
    }
}
