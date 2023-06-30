---
title: "README"
author: "p1eXu5"
date: "11/2022"
output: 
  html_document:
     css: css/splendor.css
     self_contained: no
---

<link href="css/splendor.css" rel="stylesheet"></link>

TECHNO XAKE
===========

_useful links_:

- *[Xake Documentation](https://github.com/FakeBuild/Xake/wiki)*
- *[Xake Operators](https://github.com/FakeBuild/Xake/blob/dev/src/core/ScriptFuncs.fs#L123)*
- *[Xake Implementation notes](https://github.com/FakeBuild/Xake/blob/dev/docs/implnotes.md#implementation-notes)*


_content:_

- [Run targets](#run-targets)
- [Testing](#testing)

> <h3><i> All rules must be lowercase!!!</i></h3>

## Installation

1. Download [FSharp.DependencyManager.Paket.dll](https://github.com/fsprojects/Paket/releases)
2. Copy it to the this folders:

  - C:\Program Files\dotnet\sdk\<version number>\FSharp
  - C:\Program Files\Microsoft Visual Studio\<version number>\Community\Common7\IDE\CommonExtensions\Microsoft\FSharp
  - C:\Program Files\Microsoft Visual Studio\<version number>\Community\Common7\IDE\CommonExtensions\Microsoft\FSharp\Tools

  [see F# Interactive Integration](https://fsprojects.github.io/Paket/fsi-integration.html)

3. Run restore.deps.cmd file

4. Run pack.cmd file

<br/>

### Run manualy

`dotnet fake run <script> -- "<rule>"`

<br/>

## Found Issuers

- "Could not find a suitable .NET 6 runtime version matching SDK version: 6.0.402"

  Set sdk version according runtime sdk version in `global.json` or try run again.

- ["Found .NET SDK, but did not find dotnet.dll"](https://stackoverflow.com/a/69178032/11228653)

  1. Check outdated folders in `C:\Program Files\dotnet\sdk`
  2. Check `global.json`

- "Fake System.Exception: Cache is invalid as 'Microsoft.FSharp"
    
  - Clear Nuget Cache (Open Visual Studio, go to Tools -> NuGet Package Manager -> Package Manager Settings menu.)
  - Delete `.fake` folder and `.xake` file

- "Paket: to use with no errors and with `dotnet fsi`"

  1. Download [FSharp.DependencyManager.Paket.dll](https://github.com/fsprojects/Paket/releases)
  2. Copy it to the this folders:

    - C:\Program Files\dotnet\sdk\<version number>\FSharp
    - C:\Program Files\Microsoft Visual Studio\<version number>\Community\Common7\IDE\CommonExtensions\Microsoft\FSharp
    - C:\Program Files\Microsoft Visual Studio\<version number>\Community\Common7\IDE\CommonExtensions\Microsoft\FSharp\Tools

  [see F# Interactive Integration](https://fsprojects.github.io/Paket/fsi-integration.html)