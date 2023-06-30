@echo off

GOTO :PROGRAM

:PROGRAM
echo.
echo Choice project:
echo.
echo     list                       List packages
echo.
echo     pack-testing               Pack `p1eXu5.AspNetCore.Testing`
echo     pack-logging               Pack `p1eXu5.AspNetCore.Testing.Logging`
echo.
echo.

set /p "id=Enter ID: "

IF        "%id%"=="list" (
    dotnet fsi .\pack.fsx "list-packages"
    GOTO :PROGRAM

) ELSE IF "%id%"=="pack-testing" (
    dotnet fsi .\pack.fsx "pack-p1eXu5.AspNetCore.Testing"
    GOTO :PROGRAM

) ELSE IF "%id%"=="pack-logging" (
    dotnet fsi .\pack.fsx "pack-p1eXu5.AspNetCore.Testing.Logging"
    GOTO :PROGRAM

) ELSE IF "%id%"=="e" (
    echo exit
    GOTO :EOF

) ELSE (
    echo unknown case
    GOTO :PROGRAM
)