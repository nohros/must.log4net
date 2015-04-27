using System;
using System.Xml;
using log4net;
using log4net.Config;

namespace Nohros.Logging.log4net
{
  public class LegacyLogger : AbstractLogger
  {
    readonly XmlElement element_;
    readonly string logger_name_;

    /// <summary>
    /// Initializes a new instance of the <see cref="LegacyLogger"/> class
    /// using the specified xml elelement.
    /// </summary>
    /// <param name="element">
    /// A <see cref="XmlElement"/> that represents a log4net xml configuration
    /// element.
    /// </param>
    /// <param name="logger_name">
    /// The name of the logger_ related with the given <paramref name="element"/>.
    /// </param>
    /// <remarks>
    /// The <paramref name="element"/> will be used to configure the log4net
    /// library and the <paramref name="logger_name"/> will be used to get a
    /// named logger_ throught the log4net <see cref="LogManager"/> class.
    /// </remarks>
    public LegacyLogger(XmlElement element, string logger_name) {
      element_ = element;
      logger_name_ = logger_name;
    }

    /// <summary>
    /// Creates and configures a new instance of the <see cref="LegacyLogger"/>
    /// </summary>
    /// <param name="element">
    /// The <see cref="XmlElement"/> containing the log4net configuration data.
    /// </param>
    /// <param name="logger_name">
    /// The name of the logger to be created.
    /// </param>
    public static LegacyLogger Create(XmlElement element, string logger_name) {
      var logger = new LegacyLogger(element, logger_name);
      logger.Configure();
      return logger;
    }

    /// <summary>
    /// Configures the <see cref="LegacyLogger"/> logger_.
    /// </summary>
    public void Configure() {
      XmlConfigurator.Configure(element_);
      logger_ = LogManager.GetLogger(logger_name_);
    }
  }
}
