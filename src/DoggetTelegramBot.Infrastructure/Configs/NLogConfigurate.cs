using System.Reflection;
using NLog;
using NLog.Targets;
using DoggetTelegramBot.Domain.Common.Enums;
using NLog.Config;
using LoggerConstants = DoggetTelegramBot.Domain.Common.Constants.Logger.Constants.Logger;

namespace DoggetTelegramBot.Infrastructure.Configs
{
    public static class NLogConfigurate
    {
        private static readonly string BaseDir =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
            throw new InvalidOperationException("Unable to determine base directory.");

        public static void Configurate()
        {
            LoggingConfiguration configuration = new();

            var logsType = Enum.GetValues(typeof(TelegramEvents));

            foreach (var logType in logsType)
            {
                var type = logType.ToString();

                FileTarget logfile = new(type)
                {
                    FileName = BaseDir + "/logs/" + type + "/log-${date:format=\\dd.\\MM.\\yyyy}.txt"
                };

                configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile, type);
            }

            FileTarget logError = new(LoggerConstants.ErrorName)
            {
                FileName = BaseDir + "/logs/" + LoggerConstants.ErrorName + "/log-${date:format=\\dd.\\MM.\\yyyy}.txt"
            };

            configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logError, LoggerConstants.ErrorName);

            LogManager.Configuration = configuration;
        }
    }
}
