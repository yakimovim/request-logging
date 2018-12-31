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

    internal class LogLevelRulesCompiler
    {
        public IReadOnlyList<LogLevelRule> Compile(IReadOnlyList<LogLevelRuleDescription> levelRuleDescriptions)
        {
            var result = new List<LogLevelRule>();

            foreach (var levelRuleDescription in levelRuleDescriptions ?? new LogLevelRuleDescription[0])
            {
                var script = CSharpScript.Create<bool>(levelRuleDescription.RuleCode, globalsType: typeof(Globals));
                ScriptRunner<bool> runner = script.CreateDelegate();

                result.Add(new LogLevelRule(levelRuleDescription.LogLevel, runner));
            }

            return result;
        }
    }

    internal sealed class LogLevelRule
    {
        public string LogLevel { get; }

        public ScriptRunner<bool> Rule { get; }

        public LogLevelRule(string logLevel, ScriptRunner<bool> rule)
        {
            LogLevel = logLevel ?? throw new ArgumentNullException(nameof(logLevel));
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }
    }
}