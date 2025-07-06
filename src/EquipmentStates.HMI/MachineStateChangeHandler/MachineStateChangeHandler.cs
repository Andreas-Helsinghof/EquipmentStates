using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace EquipmentStates.HMI.MachineStateChangeHandler
{
    public class MachineStateChangeHandler : IHostedService, IMachineEvents
    {
        public event Action<string>? onMachineEmergencyStop;
        private Session? _session;
        private Subscription? _subscription;
        private MonitoredItem? _temperatureItem;
        private readonly string _opcuaEndpoint = Environment.GetEnvironmentVariable("OPCUA_ENDPOINT") ?? "opc.tcp://localhost:4840/freeopcua/server/"; // Use service name from docker-compose
        private readonly string _temperatureNodeId = "ns=2;s=freeopcua.Tags.temperature"; // Adjust to match your Python OPC UA server

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => await ConnectAndSubscribe(), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _session?.Close();
            return Task.CompletedTask;
        }

        private async Task ConnectAndSubscribe()
        {

            var config = new ApplicationConfiguration()
            {
                ApplicationName = "HMIClient",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier(),
                    AutoAcceptUntrustedCertificates = true,
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = "Directory", StorePath = "./certs/issuers" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = "Directory", StorePath = "./certs/peers" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = "Directory", StorePath = "./certs/rejected" }
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
            };
            await config.Validate(ApplicationType.Client);            

            var app = new ApplicationInstance { ApplicationName = "HMIClient", ApplicationType = ApplicationType.Client, ApplicationConfiguration = config };

            var endpoint = CoreClientUtils.SelectEndpoint(config, _opcuaEndpoint, false);
            var endpointConfig = EndpointConfiguration.Create(config);
            var endpointDesc = new ConfiguredEndpoint(null, endpoint, endpointConfig);

            _session = await Session.Create(config, endpointDesc, false, "HMIClient", 6000, null, null);

            _subscription = new Subscription(_session.DefaultSubscription) { PublishingInterval = 1000 };
            _temperatureItem = new MonitoredItem(_subscription.DefaultItem)
            {
                StartNodeId = new NodeId(_temperatureNodeId),
                AttributeId = Attributes.Value,
                DisplayName = "Temperature"
            };
            _temperatureItem.Notification += OnTemperatureChanged;
            _subscription.AddItem(_temperatureItem);
            _session.AddSubscription(_subscription);

            _subscription.Create();
            Console.WriteLine($"Connected to OPC UA server at {_opcuaEndpoint} and subscribed to temperature changes.");
        }

        private void OnTemperatureChanged(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            foreach (var value in item.DequeueValues())
            {
                if (value.Value is double temp && temp > 5)
                {
                    onMachineEmergencyStop?.Invoke($"Emergency stop: temperature exceeded threshold! Value: {temp}");
                }
            }
        }
    }
}
