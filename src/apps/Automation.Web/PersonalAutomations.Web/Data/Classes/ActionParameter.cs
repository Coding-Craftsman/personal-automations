using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAutomations.Web.Data.Classes
{
    /// <summary>
    /// ActionParameter: ActionParameters are used to pass key-value pairs that can be used for an automation.
    /// This class is only used to help build up the web page to show a list of the expected parameters and
    ///    allow you to trigger the events via the web page.
    /// </summary>
    public class ActionParameter
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// The ID of the Automation Action 
        /// </summary>
        public int ActionID { get; set; } = 0;

        /// <summary>
        /// The key of the parameter to be used in the automation
        /// </summary>
        public string Name { get; set; } = string.Empty;        

        /// <summary>
        /// The value of the paramter used in the automation
        /// </summary>
        [NotMapped]
        public string Value { get; set; } = string.Empty;
    }
}
