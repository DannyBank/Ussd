using System;

namespace Dbank.Digisoft.Ussd.SDK.Abstractions;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class HandlerAttribute : Attribute
{

    public string? HandlerName { get; }

    public HandlerAttribute(){}

    public HandlerAttribute(string handlerName)
    {
        HandlerName = handlerName;
    }

}