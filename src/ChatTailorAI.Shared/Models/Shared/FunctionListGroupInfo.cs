using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTailorAI.Shared.Models.Shared
{
    public class FunctionListGroupInfo
    {
        public string Header { get; set; }
        public List<FunctionListItem> Items { get; set; }
    }
}