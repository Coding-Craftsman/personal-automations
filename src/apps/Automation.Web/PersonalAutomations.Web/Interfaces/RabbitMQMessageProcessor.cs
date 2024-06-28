using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;
using RabbitMQ.Client;

namespace PersonalAutomations.Web.Interfaces
{
    public class RabbitMQMessageProcessor : IMessageProcessor
    {
        string username = string.Empty;
        string password = string.Empty;
        string queueName = string.Empty;
        string vhost = string.Empty;
        string hostname = string.Empty;
        string exchangeName = string.Empty;

        IConnection connection;
        IModel channel;

        ApplicationDbContext _context;

        public RabbitMQMessageProcessor(
            string HostName, 
            string VHostName, 
            string Queue, 
            string UserName, 
            string Password,
            ApplicationDbContext context)
        {
            hostname = HostName;
            vhost = VHostName;
            queueName = Queue;
            username = UserName;
            password = Password;
            _context = context;

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = username;
            factory.Password = password;
            factory.VirtualHost = vhost;
            factory.HostName = hostname;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            exchangeName = $"{queueName}-exchange";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, "", null);
        }

        ~RabbitMQMessageProcessor()
        {
            connection.Close();
            channel.Close();
        }

        public void PublishMessage(string message, string Originator)
        {
            var history = new MessageHistory()
            {
                EventTime = DateTime.Now,
                Message = message,
                User = Originator
            };

            _context.MessageHistory.Add(history);
            _context.SaveChanges();

            byte[] body = System.Text.Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchangeName, "", null, body);
        }
    }
}
