using System;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rassoodock.SqlServer.Windows.Mappings;

namespace Rassoodock.Tests.Utilities
{
    public static class LaunchSettings
    {
        private static bool _environmentVariablesSetup;
        
        public static void SetLaunchSettingsEnvironmentVariables()
        {
            if (_environmentVariablesSetup)
            {
                return;
            }
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }

            SetupAutomapper();
            _environmentVariablesSetup = true;
        }

        private static void SetupAutomapper()
        {
            var startup = new MappingStartup();
            Mapper.Initialize(cfg =>
            {
                startup.Configure(cfg);
            });
        }
    }
}
