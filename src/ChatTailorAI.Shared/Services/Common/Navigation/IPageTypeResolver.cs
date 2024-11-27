using System;
using ChatTailorAI.Shared.Enums;

namespace ChatTailorAI.Shared.Services.Common.Navigation
{
    public interface IPageTypeResolver
    {
        Type GetPageType(NavigationPageKeys pageKey);
    }
}