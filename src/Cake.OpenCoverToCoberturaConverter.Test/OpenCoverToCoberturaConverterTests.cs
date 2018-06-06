using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using System;
using Xunit;

namespace Cake.OpenCoverToCoberturaConverter.Test
{
    public sealed class OpenCoverToCoberturaConverterTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_OpenCover_Is_Null()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.OpenCoverReport = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
            }

            [Fact]
            public void Should_Throw_If_Cobertura_Is_Null()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.CoberturaReport = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
            }

            [Fact]
            public void Should_Find_Report_Generator()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/OpenCoverToCoberturaConverter.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Tool_Path_If_Present()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.Settings.ToolPath = "/some/where/else/OpenCoverToCoberturaConverter.exe";
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/some/where/else/OpenCoverToCoberturaConverter.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("OpenCoverToCoberturaConverter: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("OpenCoverToCoberturaConverter: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_OpenCover_And_Cobertura_Directory()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-input:/Working/OpenCover.xml\" \"-output:/Working/Cobertura.xml\"", result.Args);
            }


            [Fact]
            public void Should_Set_Solution_Directory_Directory()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.Settings.SolutionBaseDirectory = "/some/where";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-input:/Working/OpenCover.xml\" \"-output:/Working/Cobertura.xml\" \"-sources:/some/where\"", result.Args);
            }


            [Fact]
            public void Should_Set_Include_Getters_Setters_Directory()
            {
                // Given
                var fixture = new OpenCoverToCoberturaConverterFixture();
                fixture.Settings.IncludeGettersSetters = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-input:/Working/OpenCover.xml\" \"-output:/Working/Cobertura.xml\" \"-includeGettersSetters:true\"", result.Args);
            }
        }
    }
}