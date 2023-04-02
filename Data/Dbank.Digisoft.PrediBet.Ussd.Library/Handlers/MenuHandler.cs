using Dbank.Digisoft.PrediBet.Ussd.Data.Models;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;
using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Handlers;

public class MenuHandler
{
    private readonly ILogger _logger;
    private readonly UssdMessages _messages;

    public MenuHandler(
        ILogger<MenuHandler> logger, IOptionsSnapshot<UssdMessages> messages, 
        IEnumerable<IMenuHandler> menuHandlers)
    {
        _logger = logger;
        _messages = messages.Value;
        foreach (var menuHandler in menuHandlers)
        {
            AddHandlers(menuHandler.MenuType, menuHandler.HandlerFunc, menuHandler.Override);
        }
    }

    private readonly Dictionary<Type, Func<UssdMenuItem, SessionInfo, Task<MenuCollection>>> _typeHandlers = new();

    public void AddHandlers(Type handlerFor, Func<UssdMenuItem, SessionInfo, Task<MenuCollection>> handlerFunction, bool ovveride)
    {
        var exists = _typeHandlers.TryGetValue(handlerFor, out var handleFunc);
        if (exists && ovveride)
        {
            _typeHandlers.Remove(handlerFor);
        }

        _typeHandlers.TryAdd(handlerFor, handlerFunction);
    }

    public async Task<MenuCollection> ProcessMenu(SessionInfo sessionInfo, UssdMenuItem input)
    {
        try
        {
            //Store Selections
            if(input.DataType is null || typeof(Exception).IsAssignableFrom(input.DataType))
            {
                return new(input.Text ?? string.Empty);
            }
            var selectedHandler = _typeHandlers.GetValueOrDefault(input.DataType);
            if (selectedHandler != null)
            {
                input.ProcessData();
                var output = await selectedHandler.Invoke(input, sessionInfo);
                _logger.LogTrace("Trace Output for {MethodName} :\nSession {@Session}\nMenus: {@MenuItems}", nameof(ProcessMenu), sessionInfo, output);
                return output;
            }

            throw new NotImplementedException($"Menu Handler for Type {input.DataType} not Implemented");
        }
        catch (Exception e)
        {
            //Monitoring.Monitoring.CaptureTransactionException(e, nameof(MenuHandler));
            return new(_messages.GenericError);
        }
    }
}