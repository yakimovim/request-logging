using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RequestLogging.Logging
{
    public class RequestCheckerFileReader
    {
        public IReadOnlyList<RequestChecker> ReadFile(string filePath)
        {
            return JsonConvert.DeserializeObject<RequestChecker[]>(File.ReadAllText(filePath));
        }
    }
}