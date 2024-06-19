using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Events.EventArgs
{
    public class ApiKeyChangedEventArgs : System.EventArgs
    {
        public ApiKeyType KeyType { get; set;  }

        public string ApiKey { get; set; }
    }
}
