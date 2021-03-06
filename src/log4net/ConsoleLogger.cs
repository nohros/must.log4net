using System;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A generic logger_ that uses the third party log4net logging library.
  /// </summary>
  /// <remarks>
  /// This is a generic logger_ that loads automatically and configures itself
  /// through the code. The messages are logged to the console window.
  /// <para>
  /// The pattern used to log message are:
  ///   . "[%date %-5level/%thread] %message%newline %exception" for the
  ///   non-debug messages.
  /// </para>
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public class ConsoleLogger : AbstractLogger
  {
    readonly string layout_pattern_;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogger"/> by
    /// using the default layout pattern.
    /// </summary>
    /// <see cref="AbstractLogger.kDefaultLogMessagePattern"/>
    public ConsoleLogger() : this(kDefaultLogMessagePattern) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogger"/> class by
    /// using the specified layout pattern.
    /// </summary>
    /// <param name="layout_pattern">
    /// A string that identifies the pattern to be used to format the output
    /// log message.
    /// </param>
    public ConsoleLogger(string layout_pattern) {
      layout_pattern_ = layout_pattern;
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ConsoleLogger"/> by using the default layout pattern and
    /// log level.
    /// </summary>
    /// <see cref="AbstractLogger.kDefaultLogMessagePattern"/>
    public static ConsoleLogger Create() {
      return Create(kDefaultLogMessagePattern, Level.Info);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ConsoleLogger"/> by using the given layout pattern and
    /// default log level.
    /// </summary>
    /// <see cref="AbstractLogger.kDefaultLogMessagePattern"/>
    public static ConsoleLogger Create(string layout_pattern) {
      return Create(layout_pattern, Level.Info);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ConsoleLogger"/> by using the default layout pattern and
    /// the given log level.
    /// </summary>
    /// <see cref="AbstractLogger.kDefaultLogMessagePattern"/>
    public static ConsoleLogger Create(Level level) {
      return Create(kDefaultLogMessagePattern, level);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ConsoleLogger"/> class by using the specified layout
    /// pattern and log <paramref name="level"/>.
    /// </summary>
    /// <param name="layout_pattern">
    /// A string that identifies the pattern to be used to format the output
    /// log message.
    /// </param>
    /// <param name="level">
    /// The desired log <see cref="Level"/>.
    /// </param>
    public static ConsoleLogger Create(string layout_pattern, Level level) {
      var logger = new ConsoleLogger();
      logger.Configure(level);
      return logger;
    }

    /// <summary>
    /// Configures the <see cref="FileLogger"/> logger_ adding the appenders
    /// to the root repository.
    /// </summary>
    public void Configure(Level level) {
      // create a new logger_ into the repository of the current assembly.
      ILoggerRepository repository = LogManager.GetRepository();

      var logger = (Logger) repository.GetLogger("NohrosConsoleLogger");

      // create the layout and appender for on error messages.
      var layout = new PatternLayout {ConversionPattern = layout_pattern_};
      layout.ActivateOptions();

      // create the appender
      var appender = new ConsoleAppender {
        Name = "NohrosCommonConsoleAppender",
        Layout = layout,
        Target = "Console.Out",
        Threshold = level
      };
      appender.ActivateOptions();

      logger.AddAppender(appender);

      repository.Configured = true;

      logger_ = LogManager.GetLogger("NohrosConsoleLogger");
    }
  }
}
