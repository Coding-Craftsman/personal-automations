namespace PersonalAutomations.Web.Data.Classes
{
    public class AutomationEmail
    {
        public int ID { get; set; }

        public string Email { get; set; } = string.Empty;
        
        public string MessageBody { get; set; } = string.Empty;
        
        public string MessageSubject { get; set; } = string.Empty;
    }
}
