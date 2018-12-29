using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;

namespace RequestLogging.Logging
{
    public static class LogSupport
    {
        internal static IReadOnlyList<LogLevelSetter> LogLevelSetters = new LogLevelSetter[0];

        public static readonly AsyncLocal<string> LogNamePrefix = new AsyncLocal<string>();

        public static ILog GetLogger(string name)
        {
            return GetLoggerWithPrefixedName(name);
        }

        public static ILog GetLogger(Type type)
        {
            return GetLoggerWithPrefixedName(type.FullName);
        }

        private static ILog GetLoggerWithPrefixedName(string name)
        {
            if (!string.IsNullOrWhiteSpace(LogNamePrefix.Value))
            { name = $"{LogNamePrefix.Value}.{name}"; }

            return LogManager.GetLogger(typeof(LogSupport).Assembly, name);
        }

        public static async Task<string> GetLogPrefix(HttpContext context)
        {
            var globals = new Globals
            {
                context = context
            };

            string result = null;

            foreach (var logLevelSetter in LogLevelSetters)
            {
                if (await logLevelSetter.Checker(globals))
                {
                    result = $"EdlinSoftware.Log.{logLevelSetter.LogLevel}";
                    break;
                }
            }

            return result;
        }
    }
}