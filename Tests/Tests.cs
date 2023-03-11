// ****************************************************************************
// Project:  Tests
// File:     Tests.cs
// Author:   Latency McLaughlin
// Date:     09/16/2022
// ****************************************************************************

using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests
{
    public partial class Tests
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected readonly ITestOutputHelper Console;


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="console"></param>
        public Tests(ITestOutputHelper console)
        {
            Console = console;
            
            using var file    = File.OpenText("Properties\\launchSettings.json");
            var       reader  = new JsonTextReader(file);
            var       jObject = JObject.Load(reader);

            var variables = jObject
                            .GetValue("profiles")
                            //select a proper profile here
                            .SelectMany(profiles => profiles.Children())
                            .SelectMany(profile => profile.Children<JProperty>())
                            .Where(prop => prop.Name == "environmentVariables")
                            .SelectMany(prop => prop.Value.Children<JProperty>())
                            .ToList();

            foreach (var variable in variables)
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
        }
    }
}