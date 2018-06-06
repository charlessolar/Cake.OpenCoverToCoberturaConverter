using System;
using System.Collections;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Linq;
using System.Reflection;

namespace Cake.OpenCoverToCoberturaConverter
{
    /// <summary>
    /// Contains settings used by <see cref="OpenCoverToCoberturaConverterRunner"/>.
    /// </summary>
    public class OpenCoverToCoberturaConverterSettings : ToolSettings
    {
        /// <summary>
        /// The solutions base directory to get sources
        /// </summary>
        public DirectoryPath SolutionBaseDirectory { get; set; }
        /// <summary>
        /// Include property getters and setters in Cobertura report (default false)
        /// </summary>
        public bool? IncludeGettersSetters { get; set; }
    }
}
