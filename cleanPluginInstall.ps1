. ".\settings.ps1"

Remove-Item ".\src\dotnet\ReSharperPlugin.SpecflowRiderPlugin\ReSharperPlugin.SpecflowRiderPlugin.csproj.user"
Remove-Item ".\output\ReSharperPlugin.SpecflowRiderPlugin.*.nupkg"
Remove-Item "$env:LOCALAPPDATA\JetBrains\plugins\ReSharperPlugin.SpecflowRiderPlugin.*" -Recurse