using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.OpenCoverToCoberturaConverter.Test
{
    class OpenCoverToCoberturaConverterFixture : ToolFixture<OpenCoverToCoberturaConverterSettings>
    {
        public FilePath OpenCoverReport { get; set; }
        public FilePath CoberturaReport { get; set; }

        public OpenCoverToCoberturaConverterFixture() : base("OpenCoverToCoberturaConverter.exe")
        {
            OpenCoverReport = (FilePath)"OpenCover.xml";
            CoberturaReport = (FilePath)"Cobertura.xml";
        }

        protected override void RunTool()
        {
            var tool = new OpenCoverToCoberturaConverterRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(OpenCoverReport, CoberturaReport, Settings);
        }
    }
}
