// ****************************************************************************
// Project:  HSMS.Logging
// File:     Serialize.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Logging.Extensions;

public static partial class Format
{
    /// <summary>
    ///     This method is used to serialize the object passed into the function and return it as a string.
    ///     The method is synchronized due to the the xmlserializer cache object types and reusing them.
    ///     If two threads call them method at the same time with the same object type.  The thread can
    ///     and do deadlock.
    /// </summary>
    /// <param name="o">object instance being serialized.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static string Serialize<T>(T o)
    {
        var tResult = o switch
        {
            object[] t => ((Func<object>)(() => t.Length == 0 ? string.Empty : t[0]))(),
            not null   => o,
            _          => throw new NullReferenceException()
        };

        var settings = new XmlWriterSettings
        {
            Indent             = true,
            IndentChars        = "  ",
            OmitXmlDeclaration = true,
            NewLineChars       = Environment.NewLine,
            NewLineHandling    = NewLineHandling.Replace
        };

        using var ms     = new MemoryStream();
        using var writer = XmlWriter.Create(ms, settings);

        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);

        if (tResult is Type type1)
        {
            var s = new XmlSerializer(type1);
            var a = Activator.CreateInstance(type1);
            s.Serialize(writer, a, ns);
        }
        else
        {
            var s = new XmlSerializer(tResult.GetType());
            s.Serialize(writer, tResult, ns);
        }

        return Encoding.UTF8.GetString(ms.ToArray()).Substring(1);
    }
}