namespace PersonalAutomations.Web.Data.Classes
{
    /// <summary>
    /// AutomationAction: This is the name of the aciton that will be executed (sent to the message queue).
    /// Each automation will be looking for payloads with this ActionKeyword in the payload.
    /// </summary>
    public class AutomationAction
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// Friendly name of the automation.  Useful to find on the web page.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The action keyword is the string that the automations look for.
        /// Example: RUN_JOB_1, UPDATE_TODO_BOARD
        /// </summary>
        public string ActionKeyword { get; set; } = string.Empty;

        /// <summary>
        /// Inactive automations will not show up on the web page.
        /// </summary>
        public bool IsActive { get; set; } = false;

        public ICollection<ActionParameter> Parameters { get; set; } = new List<ActionParameter>();
    }
}
