using ChatTailorAI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IPageTypeResolver
    {
        Type GetPageType(NavigationPageKeys pageKey);
    }
}