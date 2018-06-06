using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.OpenCoverToCoberturaConverter
{
    /// <summary>
    /// Runs the tool
    /// </summary>
    public class OpenCoverToCoberturaConverterRunner : Tool<OpenCoverToCoberturaConverterSettings> 
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCoverToCoberturaConverterRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OpenCoverToCoberturaConverterRunner( IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "OpenCoverToCoberturaConverter";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] {"OpenCoverToCoberturaConverter.exe"};
        }

        /// <summary>
        /// Converts the opencover report to cobertura using the specified settings.
        /// </summary>
        /// <param name="openCoverReport">The opencover coverage report.</param>
        /// <param name="coberturaReport">The output cobertura file.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath openCoverReport, FilePath coberturaReport, OpenCoverToCoberturaConverterSettings settings)
        {
            if(settings == null)
                throw new ArgumentException(nameof(settings));
            if(openCoverReport == null)
                throw new ArgumentException(nameof(openCoverReport));
            if(coberturaReport == null)
                throw new ArgumentException(nameof(coberturaReport));

            Run(settings, GetArgument(settings, openCoverReport, coberturaReport));
        }

        private ProcessArgumentBuilder GetArgument(OpenCoverToCoberturaConverterSettings settings, FilePath openCoverReport, FilePath coberturaReport) 
        {
            var builder = new ProcessArgumentBuilder();
            AppendQuoted(builder, "input", openCoverReport.MakeAbsolute(_environment).FullPath);
            AppendQuoted(builder, "output", coberturaReport.MakeAbsolute(_environment).FullPath);

            if( settings.SolutionBaseDirectory != null )
                AppendQuoted(builder, "sources", settings.SolutionBaseDirectory.MakeAbsolute(_environment).FullPath);
            if( settings.IncludeGettersSetters.HasValue )
                AppendQuoted(builder, "includeGettersSetters", settings.IncludeGettersSetters.ToString().ToLower());

            return builder;
        }
        private void AppendQuoted(ProcessArgumentBuilder builder, string key, string value)
        {
            builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "-{0}:{1}", key, value));
        }
    }
}
