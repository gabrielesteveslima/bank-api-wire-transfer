using Confluent.Kafka;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.PublishOnTopic
{
    /// <summary>
    ///     Represent class configuration for kafka
    /// </summary>
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string TopicName { get; set; }
        public SecurityProtocol SecurityProtocol { get; set; }
        public bool EnableSslCertificateVerification { get; set; }
        public int MessageTimeoutMs { get; set; }
    }
}