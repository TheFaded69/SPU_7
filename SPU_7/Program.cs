using Avalonia;
using System.Diagnostics.CodeAnalysis;

namespace SPU_7;

public class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args) =>
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() => AppBuilder
        .Configure<App>()
        .UsePlatformDetect();

}