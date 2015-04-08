using System;
using log4net;
using log4net.Repository;
using log4net.Core;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A basic implementation of the <see cref="ILogger"/> that uses the
  /// log4net library as the underlying logging library.
  /// </summary>
  public abstract class AbstractLogger : ILogger
  {
    /// <summary>
    /// The default message log pattern.
    /// </summary>
    protected internal const string kDefaultLogMessagePattern =
      "[%-5level %date] %message%newline%exception";

    protected internal const string kDefaultLogFileName = "must.log";

    protected ILog logger_;

    /// <inherit />
    public void Debug(string message) {
      logger_.Debug(message);
    }

    /// <inherit />
    public void Debug(string message, Exception exception) {
      logger_.Debug(message, exception);
    }

    /// <inherit />
    public void Error(string message) {
      logger_.Error(message);
    }

    /// <inherit />
    public void Error(string message, Exception exception) {
      logger_.Error(message, exception);
    }

    /// <inherit />
    public void Fatal(string message) {
      logger_.Fatal(message);
    }

    /// <inherit />
    public void Fatal(string message, Exception exception) {
      logger_.Fatal(message, exception);
    }

    /// <inherit />
    public void Info(string message) {
      logger_.Info(message);
    }

    /// <inherit />
    public void Info(string message, Exception exception) {
      logger_.Info(message, exception);
    }

    /// <inherit />
    public void Warn(string message) {
      logger_.Warn(message);
    }

    /// <inherit />
    public void Warn(string message, Exception exception) {
      logger_.Warn(message, exception);
    }

    /// <inherit />
    public bool IsDebugEnabled {
      get { return logger_.IsDebugEnabled; }
    }

    /// <inherit />
    public bool IsErrorEnabled {
      get { return logger_.IsErrorEnabled; }
    }

    /// <inherit />
    public bool IsFatalEnabled {
      get { return logger_.IsFatalEnabled; }
    }

    /// <inherit />
    public bool IsInfoEnabled {
      get { return logger_.IsInfoEnabled; }
    }

    /// <inherit />
    public bool IsWarnEnabled {
      get { return logger_.IsWarnEnabled; }
    }

    /// <inherit />
    public bool IsTraceEnabled {
      get { return false; }
    }

    /// <summary>
    /// Gets or sets the threshold level of the logger_ repository.
    /// </summary>
    internal LogLevel LogLevel {
      get {
        Level level = logger_.Logger.Repository.Threshold;
        if (level == Level.All) {
          return LogLevel.All;
        }
        if (level == Level.Debug) {
          return LogLevel.Debug;
        }
        if (level == Level.Error) {
          return LogLevel.Error;
        }
        if (level == Level.Fatal) {
          return LogLevel.Fatal;
        }
        if (level == Level.Info) {
          return LogLevel.Info;
        }
        if (level == Level.Off) {
          return LogLevel.Off;
        }
        if (level == Level.Warn) {
          return LogLevel.Warn;
        }
        return LogLevel.Off;
      }

      set {
        ILoggerRepository repository = logger_.Logger.Repository;
        switch (value) {
          case LogLevel.All:
            repository.Threshold = Level.All;
            break;

          case LogLevel.Debug:
            repository.Threshold = Level.Debug;
            break;

          case LogLevel.Error:
            repository.Threshold = Level.Error;
            break;

          case LogLevel.Fatal:
            repository.Threshold = Level.Fatal;
            break;

          case LogLevel.Info:
            repository.Threshold = Level.Info;
            break;

          case LogLevel.Off:
            repository.Threshold = Level.Off;
            break;

          case LogLevel.Trace:
            repository.Threshold = Level.Trace;
            break;

          case LogLevel.Warn:
            repository.Threshold = Level.Warn;
            break;
        }
      }
    }
  }
}
