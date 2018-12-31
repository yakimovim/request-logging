using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace RequestLogging.Logging
{
    public class LogLevelRulesFileReader
    {
        public IReadOnlyList<LogLevelRuleDescription> ReadFile(string filePath)
        {
            // Wait while an application modifying the file release lock.
            Thread.Sleep(1000);

            return JsonConvert.DeserializeObject<LogLevelRuleDescription[]>(File.ReadAllText(filePath));
        }
    }
}