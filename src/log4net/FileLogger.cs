using System;
using System.Reflection;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A generic logger that uses the third party log4net logging library.
  /// </summary>
  /// <remarks>
  /// This is a generic logger that loads automatically and configures itself
  /// through the code. The messages are logged to a file that resides on the
  /// same folder of the caller application base directory.The name of the
  /// file is nohros-logger.log.
  /// <para>
  /// The pattern used to log message are:
  ///     . "[%date %-5level/%thread] %message%newline %exception".
  /// </para>
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the nohros
  /// configuration file.
  /// </para>
  /// </remarks>
  public class FileLogger : AbstractLogger
  {
    readonly string layout_pattern_;
    readonly string log_file_path_;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileLogger"/> class by
    /// using the default layout pattern and log's file name.
    /// </summary>
    /// <see cref="AbstractLogger.kDefaultLogMessagePattern"/>
    /// <see cref="AbstractLogger.kDefaultLogFileName"/>
    public FileLogger() : this(kDefaultLogMessagePattern, kDefaultLogFileName) {
    }

    /// <summary>
    /// Initializes a new instance of the Logger class by using the specified
    /// string as the path to the log file.
    /// </summary>
    /// <remarks>
    /// The logger_ is not functional at this point, you need to call the
    /// <see cref="Configure"/> method to in order to make the logger_ usable.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="layout_pattern"/> or <paramref name="log_file_path"/>
    /// are null.
    /// </exception>
    public FileLogger(string layout_pattern, string log_file_path) {
      if (log_file_path == null || layout_pattern == null) {
        throw new ArgumentNullException(log_file_path == null
          ? "log_file_path"
          : "layout_pattern");
      }

      if (log_file_path.Length == 0) {
        throw new ArgumentException("log_file_path");
      }

      log_file_path_ = log_file_path;
      layout_pattern_ = layout_pattern;
    }

    /// <summary>
    /// Creates a configured <see cref="FileLogger"/> that uses the default
    /// file name, layout pattern and log level.
    /// </summary>
    /// <returns></returns>
    public static FileLogger Create() {
      return Create(Level.Info);
    }

    /// <summary>
    /// Creates a configured <see cref="FileLogger"/> that uses the default
    /// file name and layout pattern and the given log level.
    /// </summary>
    public static FileLogger Create(Level level) {
      return Create(kDefaultLogMessagePattern, level, kDefaultLogFileName);
    }

    /// <summary>
    /// Creates a configured <see cref="FileLogger"/> that uses the default
    /// file name and layout pattern.
    /// </summary>
    public static FileLogger Create(string layout_pattern, Level level) {
      return Create(layout_pattern, level, kDefaultLogFileName);
    }

    /// <summary>
    /// Creates a configured <see cref="FileLogger"/> that uses the default
    /// file name and layout pattern.
    /// </summary>
    public static FileLogger Create(Level level, string file_name) {
      return Create(kDefaultLogMessagePattern, level, kDefaultLogFileName);
    }

    /// <summary>
    /// Creates a configured <see cref="FileLogger"/> that uses the default
    /// file name and layout pattern.
    /// </summary>
    /// <remarks>
    /// The logger_ is not functional at this point, you need to call the
    /// <see cref="Configure"/> method to in order to make the logger_ usable.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="layout_pattern"/> or <paramref name="log_file_path"/>
    /// are null.
    /// </exception>
    public static FileLogger Create(string layout_pattern, Level level,
      string log_file_path) {
      var logger = new FileLogger(layout_pattern, log_file_path);
      logger.Configure(level);
      return logger;
    }

    /// <summary>
    /// Configures the <see cref="FileLogger"/> logger adding the appenders to
    /// the root repository.
    /// </summary>
    /// <remarks></remarks>
    public void Configure(Level level) {
      // create a new logger_ into the repository of the current assembly.
      ILoggerRepository repository =
        LogManager
          .GetRepository(Assembly.GetExecutingAssembly());

      var logger = (Logger) repository.GetLogger("NohrosFileLogger");

      // create the layout and appender for log messages
      var layout = new PatternLayout {
        ConversionPattern = layout_pattern_
      };
      layout.ActivateOptions();

      var appender = new FileAppender {
        Name = "NohrosCommonFileAppender",
        File = log_file_path_,
        AppendToFile = true,
        Layout = layout,
        Threshold = level
      };
      appender.ActivateOptions();

      // add the appender to the root repository
      logger.AddAppender(appender);

      repository.Configured = true;

      logger_ = LogManager.GetLogger("NohrosFileLogger");
    }
  }
}
