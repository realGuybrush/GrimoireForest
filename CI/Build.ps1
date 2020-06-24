Function Info($msg) {
    Write-Host -ForegroundColor DarkGreen "INFO: $msg"
}

Function ResolvePath($path) {
    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($path)
}

$deployDir = ResolvePath "$PSScriptRoot/../Build/Deploy"
$projectDir = ResolvePath "$PSScriptRoot/../Unity"
$artifact = "$deployDir/StandaloneWindows64"
$unityVersion = "2019.4.1f1"

Info "deployDir = $deployDir"
Info "projectDir = $projectDir"
Info "unityVersion = $unityVersion"

Info "Start unity build"
& "C:/Program Files/Unity/Hub/Editor/$unityVersion/Editor/unity.exe" -quit -batchmode -projectPath "$projectDir" -executeMethod "Builder.Build" -logFile - | Out-Host
if(-Not $?) {
    Write-Error "Unity build failed"
    exit 1
}

Info "Create zip archive: ${artifact}.zip"
Compress-Archive -Force -Path $artifact/* -DestinationPath $artifact

Info "=== Finished successfully ==="
