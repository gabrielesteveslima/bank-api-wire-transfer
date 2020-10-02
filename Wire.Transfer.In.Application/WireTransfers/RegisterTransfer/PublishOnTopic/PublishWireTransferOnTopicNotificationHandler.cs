using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using JsonApiSerializer;
using MediatR;
using Newtonsoft.Json;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer.PublishOnTopic
{
    public class
        PublishWireTransferOnTopicNotificationHandler : INotificationHandler<PublishWireTransferOnTopicNotification>
    {
        private readonly ILogging _logging;
        private readonly KafkaConfiguration _publishTopicSettings;

        public PublishWireTransferOnTopicNotificationHandler(KafkaConfiguration publishTopicSettings, ILogging logging)
        {
            _publishTopicSettings = publishTopicSettings;
            _logging = logging;
        }


        public async Task Handle(PublishWireTransferOnTopicNotification notification,
            CancellationToken cancellationToken)
        {
            Parallel.Invoke(() => PublishWireTransferOnTopic(notification, cancellationToken));
        }

        private async void PublishWireTransferOnTopic(PublishWireTransferOnTopicNotification notification,
            CancellationToken cancellationToken)
        {
            using var producer = new ProducerBuilder<string, string>(GetProducerConfig()).Build();

            var messageId = Guid.NewGuid().ToString();
            await producer.ProduceAsync(
                _publishTopicSettings.TopicName,
                new Message<string, string>
                {
                    Key = messageId,
                    Value = JsonConvert.SerializeObject(notification.WireTransfer,
                        new JsonApiSerializerSettings())
                }, cancellationToken).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    _logging.Error(task.Exception);

                if (task.IsCompletedSuccessfully)
                    _logging.Information(task);
            }, cancellationToken);
        }

        private ProducerConfig GetProducerConfig()
        {
            var basePath = AppContext.BaseDirectory;
            var certsPath = Path.Combine(basePath, "certs");

            var config = new ProducerConfig
            {
                BootstrapServers = _publishTopicSettings.BootstrapServers,
                SslCaLocation = _publishTopicSettings.EnableSslCertificateVerification
                    ? Path.Combine(certsPath, "ca-example.cert")
                    : null,
                SecurityProtocol = _publishTopicSettings.SecurityProtocol,
                EnableSslCertificateVerification = _publishTopicSettings.EnableSslCertificateVerification,
                SaslUsername = _publishTopicSettings.SaslUsername,
                SaslPassword = _publishTopicSettings.SaslPassword,
                MessageTimeoutMs = _publishTopicSettings.MessageTimeoutMs,
                SslEndpointIdentificationAlgorithm = SslEndpointIdentificationAlgorithm.None,
                SaslMechanism = SaslMechanism.Plain
            };
            return config;
        }
    }
}