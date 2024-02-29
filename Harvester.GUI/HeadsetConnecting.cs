using BrainMood.Abstractions.MindwaveClient;
using BrainMood.Abstractions.MindwaveClient.Events;
using System;
using System.Windows.Forms;

namespace BrainMood.Harvester.GUI
{
    public partial class HeadsetConnecting : Form
    {
        private readonly int c_dataReceivedTreshold = 5;

        private readonly HeadsetClient m_headsetClient;

        private int m_dataReceivedCount = 0;

        public HeadsetConnecting(HeadsetClient headsetClient)
        {
            m_headsetClient = headsetClient;
            InitializeComponent();
        }

        private void HeadsetConnecting_Shown(object sender, EventArgs e)
        {
            m_headsetClient.DataReceived += DataReceived;
            m_headsetClient.ErrorOccured += ErrorOccured;
        }

        private void DataReceived(object sender, DataReceivedEventArgs args)
        {
            m_dataReceivedCount++;

            Invoke((MethodInvoker)delegate
            {
                outputTextBox.AppendText($"Data received: {m_dataReceivedCount}\n");
            });

            if (m_dataReceivedCount > c_dataReceivedTreshold)
            {
                m_headsetClient.DataReceived -= DataReceived;
                Invoke((MethodInvoker)Close);
            }
        }

        private void ErrorOccured(object sender, ErrorEventArgs args)
        {
            m_dataReceivedCount = 0;

            Invoke((MethodInvoker)delegate
            {
                outputTextBox.AppendText($"Connecting: {args}\n");
            });
        }
    }
}
