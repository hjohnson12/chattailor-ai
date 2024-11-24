using System;
using ChatTailorAI.Shared.Enums;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IPageTypeResolver
    {
        Type GetPageType(NavigationPageKeys pageKey);
    }
}