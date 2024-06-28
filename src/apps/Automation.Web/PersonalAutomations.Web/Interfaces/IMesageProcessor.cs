namespace PersonalAutomations.Web.Interfaces
{
    public interface IMessageProcessor
    {
        public void PublishMessage(string message, string Originator);
    }
}
