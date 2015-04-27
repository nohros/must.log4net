using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A generic logger_ that uses the third party log4net logging library and
  /// logs messages to the system console.
  /// </summary>
  /// <remarks>
  /// <see cref="ColoredConsoleLogger"/> appends log events to the
  /// standard output stream ot the error output stream using layout defined
  /// by the user. It also allows the color of a specific type of message to
  /// be set.
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public class ColoredConsoleLogger : AbstractLogger
  {
    readonly string layout_pattern_;
    ColoredConsoleAppender.LevelColors[] level_colors_;

    /// <summary>
    /// Initializes a new instance of the ConsoleLogger class.
    /// </summary>
    public ColoredConsoleLogger(string layout_pattern) {
      layout_pattern_ = layout_pattern;
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ColoredConsoleLogger"/> that uses the default layout
    /// pattern and log level.
    /// </summary>
    /// <seealso cref="ColoredConsoleLogger"/>
    public static ColoredConsoleLogger Create() {
      return Create(kDefaultLogMessagePattern, Level.Info);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ColoredConsoleLogger"/> that uses the default layout
    /// pattern and the given log <paramref name="level"/>.
    /// </summary>
    /// <seealso cref="ColoredConsoleLogger"/>
    public static ColoredConsoleLogger Create(Level level) {
      return Create(kDefaultLogMessagePattern, level);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ColoredConsoleLogger"/> that uses the default layout
    /// pattern and log level.
    /// </summary>
    /// <seealso cref="ColoredConsoleLogger"/>
    public static ColoredConsoleLogger Create(string layout_pattern) {
      return Create(layout_pattern, Level.Info);
    }

    /// <summary>
    /// Creates and configures a new instance of the
    /// <see cref="ColoredConsoleLogger"/> that uses the given
    /// <paramref name="layout_pattern"/> pattern and log
    /// <paramref name="level"/>.
    /// </summary>
    /// <seealso cref="ColoredConsoleLogger"/>
    public static ColoredConsoleLogger Create(string layout_pattern, Level level) {
      var logger = new ColoredConsoleLogger(kDefaultLogMessagePattern);
      logger.Configure(level);
      return logger;
    }

    /// <summary>
    /// Configures the <see cref="FileLogger"/> logger_ adding the appenders
    /// to the root repository.
    /// </summary>
    public void Configure(Level level) {
      // create a new logger_ into the repository of the current assembly.
      ILoggerRepository repository =
        LogManager
          .GetRepository(Assembly.GetExecutingAssembly());

      var logger = (Logger) repository.GetLogger("NohrosColoredConsoleLogger");

      // create the layout and appender for on error messages.
      var layout = new PatternLayout {
        ConversionPattern = layout_pattern_
      };
      layout.ActivateOptions();

      // create the appender
      var appender = new ColoredConsoleAppender {
        Name = "NohrosCommonColoredConsoleAppender",
        Layout = layout,
        Target = "Console.Out",
        Threshold = level
      };

      if (level_colors_ == null) {
        level_colors_ = GetDefaultLevelsColors();
      }

      for (int i = 0, j = level_colors_.Length; i < j; i++) {
        // activate the level colors options and add it to the appender.
        ColoredConsoleAppender.LevelColors level_colors = level_colors_[i];
        if (level_colors != null) {
          level_colors.ActivateOptions();
          appender.AddMapping(level_colors);
        }
      }

      appender.ActivateOptions();

      logger.AddAppender(appender);

      repository.Configured = true;

      logger_ = LogManager.GetLogger("NohrosColoredConsoleLogger");
    }

    ColoredConsoleAppender.LevelColors[] GetDefaultLevelsColors() {
      // Define the default color mapping for the levels that have lower
      // priority than the the ERROR log. The level color mapping that will
      // be used to log a message will be the nearest mapping value for the
      // level that is equal to os less than the level of the message. So, in
      // order to log all messages to a specific color scheme we only need to
      // set the level of the lowest level in logger_ level hierarchy, that is
      // the lower that the level that is explicit specified, that is the
      // level of [ERROR].
      var levels = new[] {
        Level.Warn, Level.Notice, Level.Info, Level.Debug, Level.Fine,
        Level.Trace, Level.Finer, Level.Verbose, Level.Finest, Level.All
      };

      int levels_length = levels.Length;
      var levels_colors =
        new ColoredConsoleAppender.LevelColors[levels_length + 1];

      for (int i = 0; i < levels_length; i++) {
        var level_colors = new ColoredConsoleAppender.LevelColors();

        level_colors.ForeColor = ColoredConsoleAppender.Colors.Green;
        level_colors.Level = levels[i];
        levels_colors[i] = level_colors;
      }

      // define the default color mapping for the ERROR level.
      levels_colors[levels_length] = new ColoredConsoleAppender.LevelColors {
        Level = Level.Error,
        ForeColor = ColoredConsoleAppender.Colors.Red
      };

      return levels_colors;
    }

    /// <summary>
    /// Gets the mappings between the level that a logging call is made at
    /// and the color it should be displayed as.
    /// </summary>
    /// <remarks>
    /// The default level color used the default console color for all
    /// levels except for ERROR that uses the red color as foreground color.
    /// </remarks>
    public ColoredConsoleAppender.LevelColors[] LevelColors {
      get { return level_colors_; }
      set {
        if (value == null)
          throw new ArgumentNullException("value");
      }
    }
  }
}
