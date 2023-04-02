using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using System;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;

public interface IMenuHandler
{
    Type MenuType { get;  }
    bool Override { get;  }
    Task<MenuCollection> HandlerFunc(UssdMenuItem input, SessionInfo sessionInfoData = default!);
}