namespace WPFDynamicGUI.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    [DebuggerStepThrough()]
    public class AddNameArgs : IPayload
    {
        public object Sender { get; set; }
        public string Salutation { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}
