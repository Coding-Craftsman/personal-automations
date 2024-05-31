namespace PersonalAutomations.Web.Data.Classes
{
    /// <summary>
    /// AutomationMessagePayload is sent to the message queue for the automation jobs to execute
    ///  it contains the action information as well as all the required parameters.
    /// </summary>
    public class AutomationMessagePayload
    {
        public AutomationAction Action { get; set; } = new AutomationAction();

        public List<ActionParameter> Parameters { get; set; } = new List<ActionParameter>();
    }
}
