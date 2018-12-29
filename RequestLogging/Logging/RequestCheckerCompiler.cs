using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace RequestLogging.Logging
{
    public class Globals
    {
        // ReSharper disable once InconsistentNaming
        public HttpContext context;
    }

    internal class RequestCheckerCompiler
    {
        public IReadOnlyList<LogLevelSetter> Compile(IReadOnlyList<RequestChecker> levelCheckers)
        {
            var result = new List<LogLevelSetter>();

            foreach (var levelChecker in levelCheckers ?? new RequestChecker[0])
            {
                var script = CSharpScript.Create<bool>(levelChecker.CheckerCode, globalsType: typeof(Globals));
                ScriptRunner<bool> runner = script.CreateDelegate();

                result.Add(new LogLevelSetter(levelChecker.LogLevel, runner));
            }

            return result;
        }
    }

    internal sealed class LogLevelSetter
    {
        public string LogLevel { get; }

        public ScriptRunner<bool> Checker { get; }

        public LogLevelSetter(string logLevel, ScriptRunner<bool> checker)
        {
            LogLevel = logLevel ?? throw new ArgumentNullException(nameof(logLevel));
            Checker = checker ?? throw new ArgumentNullException(nameof(checker));
        }
    }
}