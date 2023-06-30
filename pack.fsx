#r "nuget: Xake, 1.1.4.427-beta"
#r "nuget: Xake.Dotnet, 1.1.4.7-beta"

open System
open Xake
open Xake.Tasks
open Xake.Dotnet

let [<Literal>] defaultVersion = "1.0.0-preview"

let nextVersion packageMask =
    recipe {
        return
            Fileset.listByMask "../_nugets/" (Path.PathMask [Path.Part.FileMask packageMask])
            |> Seq.sortByDescending id
            |> Seq.tryHead
            |> Option.bind (fun file ->
                let versionStr = String(file |> Seq.rev |> Seq.skip ("-preview.nupkg".Length) |> Seq.takeWhile ((<>) '.') |> Seq.rev |> Seq.toArray)
                match Int32.TryParse(versionStr) with
                | true, v -> $"1.0.{v + 1}-preview" |> Some
                | false, _ -> None
            )
            |> Option.defaultValue defaultVersion
    }

let wantedRule =
    if fsi.CommandLineArgs.Length > 1 && not <| String.IsNullOrWhiteSpace(fsi.CommandLineArgs[1])
    then
        fsi.CommandLineArgs[1]
    else
        "main"


do xakeScript {
    consolelog Verbosity.Normal
    want [wantedRule]
    rules [
        "main" => recipe {
            do! trace Message "ping"
        }

        "list-packages" ..> recipe {
            do! alwaysRerun ()
            do! trace Error "%A" (
                Fileset.listByMask 
                    "../_nugets/"
                    (Path.PathMask [Path.Part.FileMask "*.nupkg"])
                |> Seq.toList
                |> fun xs -> sprintf "packages: %s" (String.replicate 50 "-") :: xs)
        }

        "pack-(projectName:*)" ..> recipe {
            do! alwaysRerun ()

            let! projectName = getRuleMatch "projectName"

            if String.IsNullOrWhiteSpace(projectName) then
                failwith "Project name is not set"

            let packageMask = $"{projectName}*.nupkg"
            let! nextVersion = nextVersion packageMask

            let! _ = shell {
                cmd "dotnet pack"
                workdir "./"
                args [
                    $"src/{projectName}/{projectName}.csproj"
                    "-c Debug"
                    $"-p:PackageVersion={nextVersion}"
                    "--force"
                    "-o ../_nugets"
                    "--version-suffix test"
                ]
                failonerror
            }
            return ()
        }
    ]
}