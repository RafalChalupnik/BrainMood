using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BrainMood.Abstractions.Data;
using BrainMood.Abstractions.MindwaveClient.Events;
using Newtonsoft.Json;

namespace BrainMood.Abstractions.MindwaveClient
{
    public class HeadsetClient : TcpClient
    {
        private const string c_ipAddress = "127.0.0.1";
        private const int c_port = 13854;

        private static readonly Encoding s_encoding = Encoding.UTF8;

        private static readonly ConfigurationPacket s_configurationPacket
            = new ConfigurationPacket(enableRawOutput: false, format: "Json");

        private readonly NetworkStream m_networkStream;

        public event EventHandler<DataReceivedEventArgs>? DataReceived;

        public event EventHandler<ErrorEventArgs>? ErrorOccured;

        public HeadsetClient() : base(c_ipAddress, c_port)
        {
            m_networkStream = GetStream();

            if (!m_networkStream.CanRead)
            {
                throw new InvalidOperationException("Cannot read from network stream.");
            }
            if (!m_networkStream.CanWrite)
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
                    ReadPacketsBatch()
                        .ForEach(HandlePacket);
                }
            }, cancellationToken);
        }

        private async Task SendConfigurationPacket()
        {
            var configurationPacketJson = JsonConvert.SerializeObject(s_configurationPacket);
            var configurationPacketBytes = s_encoding.GetBytes(configurationPacketJson);

            await m_networkStream.WriteAsync(configurationPacketBytes, 0, configurationPacketBytes.Length);
        }

        private List<string> ReadPacketsBatch()
        {
            const int c_maxLength = 4096;
            const char c_separator = '\r';

            var buffer = new byte[c_maxLength];
            var bytesRead = m_networkStream.Read(buffer, 0, c_maxLength);

            return s_encoding.GetString(buffer, 0, bytesRead)
                .Split(c_separator)
                .ToList();
        }

        private void HandlePacket(string packetJson)
        {
            if (!packetJson.Contains("{") || packetJson.Contains("blink") || packetJson.Contains("mentalEffort") || packetJson.Contains("familiarity"))
            {
                //ReportError($"Unrecognized JSON: {packetJson}");
                return;
            }

            var clearedPacketJson = packetJson.Substring(packetJson.IndexOf('{'));
            var packet = JsonConvert.DeserializeObject<IDictionary>(clearedPacketJson);

            if (IsDeviceOff(packet))
            {
                ReportError("Device is off");
                return;
            }
            if (IsDeviceNotFitted(packet))
            {
                ReportError("Device is not fitted correctly.");
                return;
            }

            var deserializedPacket = JsonConvert.DeserializeObject<DataWithoutEmotion>(packetJson);
            ReportData(deserializedPacket);
        }

        private static bool IsDeviceOff(IDictionary packet)
        {
            return packet.Contains("status");
        }

        private static bool IsDeviceNotFitted(IDictionary packet)
        {
            return packet["eSense"]!.ToString()
                == "{\"attention\":0,\"meditation\":0}";
        }

        private void ReportError(string error)
        {
            ErrorOccured?.Invoke(this, new ErrorEventArgs(error));
        }

        private void ReportData(DataWithoutEmotion data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }
    }
}