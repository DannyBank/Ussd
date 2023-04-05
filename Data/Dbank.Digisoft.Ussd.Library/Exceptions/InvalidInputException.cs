using Dbank.Digisoft.Ussd.SDK.Session.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dbank.Digisoft.Ussd.SDK.Exceptions;

public sealed class InvalidInputException:Exception
{
    public InvalidInputException():base()
    {        }

    public InvalidInputException(SessionStage stage, ICollection<string> allowedInput, string input):base()
    {
        Data?.Add("AllowedInput", allowedInput); 
        Data?.Add("Input",input); 
        Data?.Add("Stage", stage);
        Message = $"Invalid Input {input} entered at {stage} allowed inputs are {string.Join(",", allowedInput)}";

    }

    public override IDictionary Data { get; }= new Hashtable();
    public override string Message { get; }
}