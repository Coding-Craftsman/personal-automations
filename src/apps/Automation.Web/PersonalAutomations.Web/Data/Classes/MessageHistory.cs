namespace PersonalAutomations.Web.Data.Classes
{
    /// <summary>
    /// Message history is used to keep track of all the 
    ///  triggers to automations that have been performed by the
    ///  application.
    /// </summary>
    public class MessageHistory
    {
        public int ID { get; set; } = 0;

        public DateTime EventTime { get; set; } = DateTime.MinValue;

        public string Message { get; set; } = string.Empty;
    }
}
