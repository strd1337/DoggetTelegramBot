using System.Reflection;
using NLog;
using NLog.Targets;
using DoggetTelegramBot.Domain.Common.Enums;
using NLog.Config;
using DoggetTelegramBot.Domain.Common.Constants;

namespace DoggetTelegramBot.Infrastructure.Configs
{
    public static class NLogConfigurate
    {
        private readonly static string BaseDir =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
            throw new InvalidOperationException("Unable to determine base directory.");

        public static void Configurate()
        {
            var configuration = new LoggingConfiguration();

            var logsType = Enum.GetValues(typeof(TelegramEvents));

            foreach (var logType in logsType)
            {
                var type = logType.ToString();

                var logfile = new FileTarget(type) 
                { 
                    FileName = BaseDir + "/logs/" + type + "/log-${date:format=\\dd.\\MM.\\yyyy}.txt" 
                };

                configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile, type);
            }

            var logError = new FileTarget(Constants.Logger.ErrorName) 
            { 
                FileName = BaseDir + "/logs/" + Constants.Logger.ErrorName + "/log-${date:format=\\dd.\\MM.\\yyyy}.txt" 
            };
            
            configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logError, Constants.Logger.ErrorName);

            LogManager.Configuration = configuration;
        }
    }
}
