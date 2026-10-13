# Cwc-src
Source code of the Cwc compiler

## Command-line builds

```bat
cwc --cli project.cwMake
cwc --cli --run project.cwMake
cwc --no-run project.cwMake
cwc --gui --no-run project.cwMake
```

CLI mode is selected by `--cli`, command shells (cmd/PowerShell), or redirected
output (automation), except debugger and direct VS Code / Visual Studio launches,
which retain the GUI default. `--gui` explicitly selects the interface, even with
redirected output; `--cli` explicitly selects the console, even under a debugger.
It builds without running the project by default in CLI mode. `--run`
allows the project's Run directives; `--no-run` suppresses them in either mode.
Nested `.cwc` / `.cwMake` build scripts and build commands still execute.
Place these options before the project path. GUI launches retain their Run behavior.
The process exits with code 0 on success, 1 on build failure, or 2 for invalid options.
Compiler and build-command exit codes are checked independently of diagnostic text.

Cwc remains a Windows GUI executable for Explorer launches. In PowerShell, pipe
its output to wait for completion and display its diagnostics:

```powershell
& 'D:\App\_Cwc\cwc.exe' --cli 'path\project.cwMake' | Out-Host
$LASTEXITCODE
```

Regression checks (using locally installed LibRT Clang and MinGW toolchains):

```powershell
.\Tests\cli.ps1
```