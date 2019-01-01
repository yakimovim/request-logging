using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RequestLogging.Logging
{
    public class LogLevelRulesFileReader
    {
        public IReadOnlyList<LogLevelRuleDescription> ReadFile(string filePath)
        {
            return JsonConvert.DeserializeObject<LogLevelRuleDescription[]>(File.ReadAllText(filePath));
        }
    }
}