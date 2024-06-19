using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IAppNotificationService
    {
        void Display(string message, int duration = 3000);
    }
}
