using System.Data.SqlClient;
using System.Threading.Tasks;
using NbusRabbitMQWebApi.Commands;
using NServiceBus;
using NServiceBus.Transport.RabbitMQ;

namespace NbusRabbitMQWebApi.Configs
{
    public class NServiceBusConfig
    {
        public static async Task<IEndpointInstance> StartEndpointAsync()
        {


            // Define the endpoint configuration
            var endpointConfiguration = new EndpointConfiguration("NbusRabbitMQWebApi");

            // Use external DI container
           // endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            // Configure serialization (JSON in this case)
            endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
            // Configure RabbitMQ as the transport
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost;username=guest;password=guest;virtualHost=/");
            //transport.UseConventionalRoutingTopology();
            // Specify the QueueType here
            transport.UseConventionalRoutingTopology(QueueType.Quorum); // or QueueType.Classic

            // Define routing for the command
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrderCommand), "NbusRabbitMQWebApi"); // Replace with the actual endpoint name that handles PlaceOrderCommand


            //transport.Transport.TransportTransactionMode = TransportTransactionMode.ReceiveOnly; // Configure transactions if needed

            // Enable queue creation
            endpointConfiguration.EnableInstallers();


            // Configure SQL Server as the persistence
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(() => new SqlConnection("Server=localhost;Database=products_db;User Id=sa;Password=smicr@123;TrustServerCertificate=True;"));

            // Set up conventions (optional, to customize message identification)
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "NbusRabbitMQWebApi.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "NbusRabbitMQWebApi.Events");

            // Enable install for SQL Server schema
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.DisableCache();

            // Start the endpoint instance
            return await NServiceBus.Endpoint.Start(endpointConfiguration);
        }
    }
}
