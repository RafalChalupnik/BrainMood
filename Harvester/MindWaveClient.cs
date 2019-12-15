using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrainMood.Harvester
{
    // Connect to the device
    // Send configuration packet
    // Check device status (ON/OFF)
    // Check fitting
    // Check readiness
    // Read data
    // Write to file

    internal class ConfigurationPacket
    {
        [JsonProperty("enableRawOutput")]
        public bool EnableRawOutput { get; set; }

        [JsonProperty("format")]
        public string? Format { get; set; }
    }

    internal class MindWaveEventArgs : EventArgs
    {
        public string Message { get; }

        public object Data { get; }

        public MindWaveEventArgs(string message)
        {
            Message = message;
            Data = new object();
        }

        public MindWaveEventArgs(object data)
        {
            Message = "Data received.";
            Data = data;
        }
    }

    internal class MindWaveClient
    {
        private readonly TcpClient m_tcpClient;
        private readonly NetworkStream m_stream;

        private byte[] m_readBuffer;

        public event EventHandler<MindWaveEventArgs>? DataReceived;

        public MindWaveClient(TcpClient tcpClient)
        {
            m_tcpClient = tcpClient;
            m_stream = m_tcpClient.GetStream();
            m_readBuffer = new byte[4096];

            if (!m_stream.CanWrite)
            {
                throw new InvalidOperationException("Cannot write to network stream.");
            }
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            await SendConfigurationPacket();

            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    DownloadData();
                }
            }, cancellationToken);
        }

        private async Task SendConfigurationPacket()
        {
            var configurationPacket = new ConfigurationPacket
            {
                EnableRawOutput = false,
                Format = "Json"
            };

            await m_stream.WriteAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(configurationPacket)));
        }

        private void DownloadData()
        {
            var packets = DownloadPackets();
        }

        private List<object> DownloadPackets()
        {
            var bytesRead = m_stream.Read(m_readBuffer, 0, m_readBuffer.Length);
            var packets = Encoding.UTF8.GetString(m_readBuffer, 0, bytesRead).Split('\r');

            return packets
                .Select(packet => JsonConvert.DeserializeObject<object>(packet))
                .ToList();
        }
    }
}
