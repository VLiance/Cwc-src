param([string]$Exe = (Join-Path $PSScriptRoot '..\..\_Cwc\cwc.exe'))
$ErrorActionPreference = 'Stop'
if([IntPtr]::Size -eq 8) {
    & "$env:WINDIR\SysWOW64\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Bypass -File $PSCommandPath -Exe $Exe
    exit $LASTEXITCODE
}
$testRoot = Join-Path $env:TEMP ('cwc-tests-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testRoot -Force | Out-Null
function Run([string]$name, [string]$arguments, [int]$expected) {
    $info = [Diagnostics.ProcessStartInfo]::new($Exe, $arguments)
    $info.UseShellExecute = $false
    $info.CreateNoWindow = $true
    $info.RedirectStandardOutput = $true
    $info.RedirectStandardError = $true
    $info.WorkingDirectory = $testRoot
    $proc = [Diagnostics.Process]::new()
    $proc.StartInfo = $info
    $null = $proc.Start()
    $out = $proc.StandardOutput.ReadToEndAsync()
    $err = $proc.StandardError.ReadToEndAsync()
    if (!$proc.WaitForExit(60000)) { $proc.Kill(); throw "$name timed out" }
    $stdout = $out.GetAwaiter().GetResult()
    $stderr = $err.GetAwaiter().GetResult()
    [IO.File]::WriteAllText((Join-Path $testRoot "$name.log"), $stdout + $stderr)
    if ($proc.ExitCode -ne $expected) { throw "$name exit=$($proc.ExitCode), expected=$expected`n$stdout`n$stderr" }
    Write-Output "$name PASS exit=$($proc.ExitCode)"
}
Set-Content -LiteralPath (Join-Path $testRoot 'seed.txt') -Value 'sentinel'
Set-Content -LiteralPath (Join-Path $testRoot 'silent-fail.cwMake') -Value @'
>
-{_wToolchain}=VLianceTool/LibRT
>:cmd /d /c exit 7
>:cmd /d /c copy /y seed.txt forbidden.txt
>
-#Run run.bat
>
'@
Set-Content -LiteralPath (Join-Path $testRoot 'success.cwMake') -Value @'
>
-{_wToolchain}=VLianceTool/LibRT
>:cmd /d /c copy /y seed.txt built.txt
>
-#Run child.cwc
>
-#Run run.bat
>
'@
Set-Content -LiteralPath (Join-Path $testRoot 'child.cwc') -Value @'
>:cmd /d /c copy /y seed.txt nested.txt
>
'@
Set-Content -LiteralPath (Join-Path $testRoot 'run.bat') -Value '@copy /y seed.txt ran.txt'
Run 'help' '--cli --help' 0
Run 'invalid-options' '--cli --run --no-run success.cwMake' 2
Run 'invalid-ui-options' '--cli --gui success.cwMake' 2
$assembly = [Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $Exe).Path)
$mode = $assembly.GetType('cwc.Program').GetMethod('fConsoleMode', [Reflection.BindingFlags]'Static,NonPublic')
foreach($case in @(
    @{Name='debugger'; Redirected=$true; Parent='pwsh'; Debugging=$true; Cli=$false},
    @{Name='vscode'; Redirected=$true; Parent='Code'; Debugging=$false; Cli=$false},
    @{Name='visual-studio'; Redirected=$true; Parent='devenv'; Debugging=$false; Cli=$false},
    @{Name='explorer'; Redirected=$false; Parent='explorer'; Debugging=$false; Cli=$false},
    @{Name='automation'; Redirected=$true; Parent='agent'; Debugging=$false; Cli=$true},
    @{Name='powershell'; Redirected=$false; Parent='pwsh'; Debugging=$false; Cli=$true}
)) {
    $actual = $mode.Invoke($null, @($case.Redirected, $case.Parent, $case.Debugging))
    if($actual -ne $case.Cli) { throw "Wrong mode for $($case.Name)" }
    Write-Output "mode-$($case.Name) PASS"
}
Run 'silent-fail' '--cli --run silent-fail.cwMake' 1
if (Test-Path (Join-Path $testRoot 'forbidden.txt')) { throw 'Build continued after failure' }
if (Test-Path (Join-Path $testRoot 'ran.txt')) { throw 'Run executed after failure' }
Run 'success-no-run' '--cli success.cwMake' 0
if (!(Test-Path (Join-Path $testRoot 'built.txt'))) { throw 'Build did not execute' }
if (!(Test-Path (Join-Path $testRoot 'nested.txt'))) { throw 'Nested build did not execute' }
if (Test-Path (Join-Path $testRoot 'ran.txt')) { throw 'CLI ran by default' }
Run 'success-auto-cli' 'success.cwMake' 0
if (Test-Path (Join-Path $testRoot 'ran.txt')) { throw 'Redirected caller ran by default' }
Run 'success-explicit-no-run' '--no-run success.cwMake' 0
if (Test-Path (Join-Path $testRoot 'ran.txt')) { throw '--no-run executed a run directive' }
Run 'success-run' '--cli --run success.cwMake' 0
if (!(Test-Path (Join-Path $testRoot 'ran.txt'))) { throw 'Explicit run did not execute' }
Run 'missing-project' '--cli missing.cwMake' 1
Set-Content -LiteralPath (Join-Path $testRoot 'bad.c') -Value 'int main(void) { return missing_symbol; }'
Set-Content -LiteralPath (Join-Path $testRoot 'good.c') -Value 'int main(void) { return 0; }'
foreach ($backend in @('Clang', 'Default')) {
    $failure = "compiler-fail-$backend.cwMake"
    $success = "compiler-ok-$backend.cwMake"
    Set-Content -LiteralPath (Join-Path $testRoot "run-$backend.bat") -Value "@copy /y seed.txt ran-$backend.txt"
    Set-Content -LiteralPath (Join-Path $testRoot $failure) -Value @"
>
-{_wToolchain}=VLianceTool/LibRT[$backend]
>
-c bad.c -o bad-$backend.o
>:cmd /d /c copy /y seed.txt after-$backend.txt
>
-#Run run-$backend.bat
>
"@
    Run "compiler-fail-$backend" "--cli --run $failure" 1
    if (Test-Path (Join-Path $testRoot "after-$backend.txt")) { throw "$backend continued after compiler failure" }
    if (Test-Path (Join-Path $testRoot "ran-$backend.txt")) { throw "$backend ran after compiler failure" }
    Set-Content -LiteralPath (Join-Path $testRoot $success) -Value @"
>
-{_wToolchain}=VLianceTool/LibRT[$backend]
>
-c good.c -o good-$backend.o
>
"@
    Run "compiler-ok-$backend" "--cli $success" 0
    if (!(Test-Path (Join-Path $testRoot "good-$backend.o"))) { throw "$backend produced no object" }
}
Set-Content -LiteralPath (Join-Path $testRoot 'launch-fail.cwMake') -Value @'
>
-{_wToolchain}=VLianceTool/LibRT
>:cwc_missing_build_tool unused
>:cmd /d /c copy /y seed.txt after-launch.txt
>
'@
Run 'launch-fail' '--cli --run launch-fail.cwMake' 1
if (Test-Path (Join-Path $testRoot 'after-launch.txt')) { throw 'Build continued after process startup failure' }
Write-Output "Logs: $testRoot"
