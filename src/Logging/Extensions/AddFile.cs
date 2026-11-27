// ****************************************************************************
// Project:  HSMS.Logging
// File:     AddFile.cs
// Author:   Latency McLaughlin
// Date:     06/30/2021
// ****************************************************************************

#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP
using Logging.RollingFile;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Logging.Extensions;

/// <summary>
///     Extensions for adding the <see cref="FileLoggerProvider" /> to the <see cref="ILoggingBuilder" />
/// </summary>
// ReSharper disable once UnusedMember.Global
public static class Logging
{
    /// <param name="builder">The <see cref="ILoggingBuilder" /> to use.</param>
    extension(ILoggingBuilder builder)
    {
        /// <summary>
        ///     Adds a file logger named 'File' to the factory.
        /// </summary>
        public ILoggingBuilder AddFile()
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }

        /// <summary>
        ///     Adds a file logger named 'File' to the factory.
        /// </summary>
        /// <param name="filename">Sets the filename prefix to use for log files</param>
        public ILoggingBuilder AddFile(string filename)
        {
            builder.AddFile(options => options.FileName = filename);
            return builder;
        }

        /// <summary>
        ///     Adds a file logger named 'File' to the factory.
        /// </summary>
        /// <param name="configure">Configure an instance of the <see cref="FileLoggerOptions" /> to set logging options</param>
        public ILoggingBuilder AddFile(Action<FileLoggerOptions> configure)
        {
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            builder.AddFile();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
#endif