# Cake.OpenCoverToCoberturaConverter

Small Cake addin to run [OpenCoverToCoberturaConverter](https://github.com/danielpalme/OpenCoverToCoberturaConverter)

## Usage

```
  #addin "nuget:?package=Cake.OpenCoverToCoberturaConverter&version=0.1.1.2"
  #tool "nuget:?package=OpenCoverToCoberturaConverter&version=0.3.2"

  OpenCoverToCoberturaConverter("OpenCover.xml", "Cobertura.xml");

```
