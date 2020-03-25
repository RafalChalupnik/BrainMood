using System;
using System.Windows.Forms;

namespace BrainMood.Harvester.GUI
{
    public partial class Assessment : Form
    {
        public AssessmentResult Result { get; private set; }

        public Assessment()
        {
            InitializeComponent();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            var happinessChecked = GetHappiness(out var happiness);

            if (!happinessChecked)
            {
                MessageBox.Show("Select happiness");
                return;
            }

            var excitementChecked = GetExcitement(out var excitement);

            if (!excitementChecked)
            {
                MessageBox.Show("Select happiness");
                return;
            }

            var controlChecked = GetControl(out var control);

            if (!controlChecked)
            {
                MessageBox.Show("Select control");
                return;
            }

            Result = new AssessmentResult(happiness, excitement, control);
            Close();
        }

        private bool GetHappiness(out int happiness)
        {
            if (happiness5.Checked)
            {
                happiness = 5;
                return true;
            }
            if (happiness4.Checked)
            {
                happiness = 4;
                return true;
            }
            if (happiness3.Checked)
            {
                happiness = 3;
                return true;
            }
            if (happiness2.Checked)
            {
                happiness = 2;
                return true;
            }
            if (happiness1.Checked)
            {
                happiness = 1;
                return true;
            }

            happiness = -1;
            return false;
        }

        private bool GetExcitement(out int excitement)
        {
            if (excitement5.Checked)
            {
                excitement = 5;
                return true;
            }
            if (excitement4.Checked)
            {
                excitement = 4;
                return true;
            }
            if (excitement3.Checked)
            {
                excitement = 3;
                return true;
            }
            if (excitement2.Checked)
            {
                excitement = 2;
                return true;
            }
            if (excitement1.Checked)
            {
                excitement = 1;
                return true;
            }

            excitement = -1;
            return false;
        }

        private bool GetControl(out int control)
        {
            if (control5.Checked)
            {
                control = 5;
                return true;
            }
            if (control4.Checked)
            {
                control = 4;
                return true;
            }
            if (control3.Checked)
            {
                control = 3;
                return true;
            }
            if (control2.Checked)
            {
                control = 2;
                return true;
            }
            if (control1.Checked)
            {
                control = 1;
                return true;
            }

            control = -1;
            return false;
        }
    }
}
