using Dbank.Digisoft.PrediBet.Ussd.SDK.Session.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Dbank.Digisoft.PrediBet.Ussd.SDK.Abstractions;

/// <summary>
/// 
/// </summary>
public static class HandlerOperator
{
    public static HandlerAttribute? GetAttribute(MethodInfo method)
    {
        var attribs = ((HandlerAttribute[])method.GetCustomAttributes(typeof(HandlerAttribute), false));
        return (attribs.Length > 0) ? attribs[0] : null;
    }

    public static bool MethodHasAttribute(MethodInfo method)
    {
        var attrib = GetAttribute(method);
        return attrib != null;
    }

    public static string? GetHandlerName(MethodInfo method)
    {
        if (MethodHasAttribute(method))
        {
            var attrib = GetAttribute(method);
            return string.IsNullOrEmpty(attrib?.HandlerName) ? method.Name : attrib.HandlerName;
        }
        return null;
    }

    public static Func<UssdMenuItem, SessionInfo, Task<MenuCollection>?>? GetHandler(object context, MethodInfo method)
    {
        if (MethodHasAttribute(method))
        {
            var del = Delegate.CreateDelegate(typeof(Func<UssdMenuItem, SessionInfo, Task<MenuCollection>>), context, method.Name);
            Func<UssdMenuItem, SessionInfo, Task<MenuCollection>?> func = (param1, param2) =>
                (Task<MenuCollection>)del.DynamicInvoke(param1, param2)!;
            return func;
        }
        return null;
    }
}