# Logging
### Logging library for C#.

On .NET Framework 4.5–4.5.2, construct `FileLoggerProvider` with `FileLoggerOptions` and call `CreateLogger` directly. Microsoft.Extensions.Logging dependency injection and scopes are unavailable on these targets; setting `IncludeScopes` to true throws `NotSupportedException`. Later targets support `ILoggingBuilder.AddFile`.

### AAEC-compatible files

Set `UseAaecFileLayout` to use daily `prefixyyyyMMdd.N.txt` files and retain whole date groups using `RetainedFileCountLimit`. `FilesPerDayLimit` optionally caps each day. Set `CloseFileAfterWrite` to release the file after each record, and `ThrowOnWriteFailure` to surface file errors (including exhaustion of the file limit) as `IOException`. `CoordinateProcesses` additionally serializes file selection, maintenance, and the complete write using a named, machine-wide mutex. It requires `CloseFileAfterWrite` and a local directory (network shares are rejected on Windows). All writers must enable coordination, use the same normalized path and prefix, and have permission to access the mutex; aliases such as junctions or mapped paths do not share an identity. A 30-second lock timeout or access failure follows the configured write-error behavior. These options are disabled by default; existing HSMS file layout and error handling are unchanged.

---


<table>
<tr>
<th></th>
<th>Description</th>
</tr>
<tr>
<td>CREATED BY:</td>
<td>[Latency McLaughlin]</td>
</tr>
<tr>
<td>UPDATED:</td>
<td>6/28/2024</td>
</tr>
<tr>
<td>FRAMEWORK:</td>
<td>netstandard2.0, netstandard2.1, net9.0</td>
</tr>
<tr>
<td>LANGUAGE:</td>
<td>[C#] preview</td>
</tr>
<tr>
<td>OUTPUT TYPE:</td>
<td>Library [API]</td>
</tr>
<tr>
<td>SUPPORTS:</td>
<td>[Visual Studio]</td>
</tr>
<tr>
<td>GFX SUBSYS:</td>
<td>[None]</td>
</tr>
<tr>
<td>TAGS:</td>
<td>[C#], [.NET], [API], [Visual Studio]</td>
</tr>
<tr>
<td>STATUS:</td>
<td><a  href="https://github.com/Latency/Jasper/actions/workflows/dotnet.yml"><img src="https://github.com/Latency/Jasper/actions/workflows/dotnet.yml/badge.svg"></a></td>
</tr>
<tr>
<td>LICENSE:</td>
<td><a href="https://github.com/Latency/Jasper/blob/master/MIT-LICENSE.txt"><img src="https://img.shields.io/github/license/Latency/Jasper?style=plastic&logo=GitHub&logoColor=black&label=License&color=yellowgreen"></a></td>
</tr>
<tr>
<td>VERSION:</td>
<td><a href="https://github.com/Latency/Jasper/releases"><img src="https://img.shields.io/github/v/release/Latency/Jasper?include_prereleases&style=plastic&logo=GitHub&logoColor=black&label=Version&color=blue"></a></td>
</tr>
<!-- VERSION: 1.0.0 -->
</table>


