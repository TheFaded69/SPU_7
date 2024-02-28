using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SPU_7.Common.Stand;

namespace SPU_7.Models.Services.Logger
{
    public class Logger : ILogger
    {
        public Logger()
        {
            var logDirectory = "./AppLogs";
            var path = $"{Directory.GetCurrentDirectory()}/{logDirectory}/AppWork {DateTime.Now:d}.log";
            //var path = $"C:/Test/AppWork {DateTime.Now:d}.log";
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            _logCollections = new List<ObservableCollection<LogMessage>>();

            try
            {
                _streamWriter = new StreamWriter(path, true);
            }
            catch 
            {
                Logging(new LogMessage("Логгирование в файл не доступно", LogLevel.Fatal));
                // ignored
            }
            Logging(new LogMessage("Старт приложения", LogLevel.Info));
        }

        private readonly StreamWriter _streamWriter;
        private readonly List<ObservableCollection<LogMessage>> _logCollections;


        public bool Logging(LogMessage logMessage)
        {
            
            logMessage.Message = (DateTime.Now + " LogLevel: " + logMessage.LogLevel + " " + logMessage.Message);
            _streamWriter?.WriteLine(logMessage.Message);
            _streamWriter?.Flush();

            foreach (var collection in _logCollections)
            {
                collection.Insert(0, logMessage);
            }
            
            return true;
        }
        
        public void AddCollectionForLogging(ObservableCollection<LogMessage> collection)
        {
            _logCollections.Add(collection);
        }

        public void ClearLog()
        {
            foreach (var collection in _logCollections)
            {
                collection.Clear();
            }
        }
    }
}
