# TrueLayerChallenge

This repository contains my solution to the TrueLayer developer challenge. I have completed the solution using C#.

The solution is comprised of a Web API with a supplementary Swagger endpoint. There is also a [Dockerfile](.\TrueLayerChallenge\TrueLayerChallenge.WebApi\Dockerfile) included for running the app within a docker container.  

## Prerequisites
- Git installed, see https://git-scm.com/downloads
- .NET 6 installed, see https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- Docker installed, see https://docs.docker.com/desktop/windows/install/

## Get a copy of the repo

To get a local copy of the repo see the [Git documentation](https://git-scm.com/book/en/v2/Git-Basics-Getting-a-Git-Repository).

## Build & Run

For full `dotnet build` options see the [dotnet build documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build).

Build using the cli, Visual Studio or other IDE supporting the building of .NET applications.

Example cli use:
```
dotnet build --configuration Release solutionPath
```

Where `solutionPath` is the path to the solution (.sln) file or project (.csproj) file you wish to build.

To run the Web API, first publish the solution using the [`dotnet publish` command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish), for example:

```
dotnet publish webApiProjectPath -c Release -o publishPath

```
where `webApiProjectPath` is the path to the TrueLayerChallenge.WebApi.csproj file and `publishPath` is the path to publish the Web API to. Then use the [`dotnet` command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet) as follows:

```
dotnet publishPath\TrueLayerChallenge.WebApi.dll
```

One can also use the [`dotnet` run command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet) as follows:

```
dotnet run webApiProjectPath
```

The app will now be running on the configured ports in the appsettings.json (e.g. 5000 and 5001). The log is output to the command line. 

## Test

For full `dotnet test` options see the [dotnet build documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test).

Example cli use:
```
dotnet test --configuration Release solutionPath
```

Where `solutionPath` is the path to the solution (.sln) file or project (.csproj) file you wish to run tests of. Note the above command will also build the project or solution, if you wish to run tests without building add the `--no-build` argument.

## Future Improvements

If I had more time I would consider implementing the following:
- Adding extra logging
- Implement caching of responses using IMemoryCache.
- Looking into where the hot code paths are and checking for performance issues. Regarding performance it does take a few seconds to respond but I think this is due to the responsiveness of the two external services rather than my implementation but I could be wrong!
- Adding performance tests and integration level tests from the service layer to the respective external services. Each would be in individual projects.
- Adding a full CI pipeline 
- Completing XML docs and checking for inconsitencies in implementation and the docs I have provided.
- One could extend the FunTranslationsService to handle other translations. Rather than adding many duplicated methods for this I would use an enum to represent the FunTranslationType and use a switch statement to determine the URL in the outgoing request. 
- The FunTranslationsService stuff could then be moved out to a new assembly to make it reusable.
- Utilise build props to reduce the amount of duplicated content in csproj files.

