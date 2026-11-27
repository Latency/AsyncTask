// ****************************************************************************
// Project:  HSMS.Logging
// File:     StackInfo.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

using System.Diagnostics;
using System.Reflection;
using System.Text;
using Logging.Extensions;
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using ParameterContainer = System.Collections.Generic.Dictionary<string, (object? Argument, System.Reflection.ParameterInfo ParameterInfo)>;
#else
using ParameterContainer = System.Collections.Generic.Dictionary<string, System.Tuple<object?, System.Reflection.ParameterInfo>>;
#endif

namespace Logging.Models;

internal sealed class StackInfo
{
    private const sbyte  Padding   = -6;
    private const string Delimiter = ": ";

    public string?            TargetClass  => StackFrame?.GetMethod()?.DeclaringType?.FullName;
    public string?            TargetMethod => StackFrame?.GetMethod()?.ToString();
    public StackFrame?        StackFrame   { get; }
    public object?            ReturnValue  { get; set; } = default;
    public ParameterContainer ArgumentList { get; } = [];


    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="argumentList"></param>3
    public StackInfo(bool fullName, params object?[]? argumentList)
    {
        var stackTrace = new StackTrace(true);
        var frame      = stackTrace.FrameCount - 7;
        StackFrame = stackTrace.GetFrame(frame);
        if (StackFrame is null)
            return;

        var method = StackFrame.GetMethod();
        if (method is null)
            return;

        //var tok = method.ToString().Split(' ');
        var idx = 0;

        if (argumentList is null || argumentList.Length <= 0)
            return;

        foreach (var arg in argumentList.ToList())
        {
            var param = method.GetParameters()[idx++];
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            var tuple = (Argument: arg, ParameterInfo: param);
#else
            var tuple = Tuple.Create(arg, param);
#endif
            if (param.Name is not null)
                ArgumentList.Add(param.Name, tuple);
        }

        //return $"{(fullName ? $"{tok[0]} {method.DeclaringType!.FullName}." : string.Empty)}{tok[1]}";
    }


    private static string IsNullOrEmpty(string? argument) => argument switch
    {
        null => "<Null>",
        ""   => "<Empty>",
        _    => argument
    };


    private static string FormatArguments(ParameterContainer container)
    {
        var sb     = new StringBuilder();
        var cnt    = 0;
        var format = new Action<ParameterInfo, string, object>((parameterInfo, name, arg) => sb.Append($"{Environment.NewLine}\t{$"Arg {++cnt:00}",Padding}{Delimiter}{parameterInfo.ParameterType.Name} {name} = {IsNullOrEmpty(arg?.ToString())}"));
        foreach (var c in container)
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            format.Invoke(c.Value.ParameterInfo, c.Key, c.Value.Argument!);
#else
            format.Invoke(c.Value.Item2, c.Key, c.Value.Item1!);
#endif
        return sb.ToString();
    }


    private string FormatRetval()
    {
        var type = ReturnValue == null ? typeof(void) : ReturnValue.GetType();
        return $"{Environment.NewLine}\t{"Return",Padding}{Delimiter}{type}{(type.Attributes.ToString().Contains("Serializable") ? $"{Environment.NewLine}{Format.Serialize(ReturnValue)}" : string.Empty)}";
    }


    public override string ToString() => $"{Environment.NewLine}\t{"Line",Padding}{Delimiter}{StackFrame?.GetFileLineNumber()}{Environment.NewLine}\t{"Class",Padding}{Delimiter}{TargetClass}{Environment.NewLine}\t{"Method",Padding}{Delimiter}{TargetMethod}{(ArgumentList.Any() ? FormatArguments(ArgumentList) : string.Empty)}{FormatRetval()}";
}