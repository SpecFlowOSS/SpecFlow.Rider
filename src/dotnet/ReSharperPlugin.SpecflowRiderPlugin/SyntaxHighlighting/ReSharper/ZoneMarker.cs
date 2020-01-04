using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Platform.VisualStudio.SinceVs10.Shell.Zones;

namespace ReSharperPlugin.SpecflowRiderPlugin.SyntaxHighlighting.ReSharper
{
    [ZoneMarker]
    public class ZoneMarker : IRequire<ISinceVs10EnvZone>
    {
    }
}