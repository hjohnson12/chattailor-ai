using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Events
{
    public class AuthenticationRequestedEvent
    {
        public string AuthenticationUrl { get; set; }
    }
}
