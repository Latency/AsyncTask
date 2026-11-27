// ****************************************************************************
// Project:  HSMS.Logging
// File:     ToSentence.cs
// Author:   Latency McLaughlin
// Date:     06/24/2021
// ****************************************************************************

using System.Diagnostics;

namespace Logging.Extensions;

public static partial class Format
{
    [DebuggerStepThrough]
    public static string ToSentence(this string str) => new(str.SelectMany((c, i) => i > 0 && char.IsUpper(c) ? [' ', c] : new[] { c }).ToArray());
}