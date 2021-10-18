using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.OpenCoverToCoberturaConverter
{
    /// <summary>
    /// Extension methods for converting open cover reports to cobertura
    /// </summary>
    [CakeAliasCategory("OpenCover")]
    [CakeAliasCategory("Cobertura")]
    public static class OpenCoverToCoberturaConverterCakeAliases
    {
        /// <summary>
        /// Converts the opencover report to cobertura
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="openCoverReport">The opencover coverage report.</param>
        /// <param name="coberturaReport">The output cobertura file.</param>
        /// <example>
        /// <code>
        /// OpenCoverToCoberturaConverter("c:/temp/coverage/opencover.xml", "c:/temp/coverage/cobertura.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OpenCoverToCoberturaConverter(this ICakeContext context, FilePath openCoverReport, FilePath coberturaReport)
        {
            OpenCoverToCoberturaConverter(context, openCoverReport, coberturaReport, new OpenCoverToCoberturaConverterSettings());
        }
        
        /// <summary>
        /// Converts the opencover report to cobertura using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="openCoverReport">The opencover coverage report.</param>
        /// <param name="coberturaReport">The output cobertura file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// OpenCoverToCoberturaConverter("c:/temp/coverage/opencover.xml", "c:/temp/coverage/cobertura.xml", new OpenCoverToCoberturaConverterSettings(){
        ///     SolutionBaseDirectory = ...
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OpenCoverToCoberturaConverter(this ICakeContext context, FilePath openCoverReport, FilePath coberturaReport, OpenCoverToCoberturaConverterSettings settings)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var runner = new OpenCoverToCoberturaConverterRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(openCoverReport, coberturaReport, settings);
        }
    }
}
