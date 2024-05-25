using ThoughtGuide.Common.Interfaces.Entities;
using ThoughtGuide.Common.Interfaces.Providers;
using ThoughtGuide.Common.Utilities;

namespace ThoughtGuide.Common.Providers;

/// <inheritdoc cref="IElapsedTimeMeterProvider"/>
public class ElapsedTimeMeterProvider : IElapsedTimeMeterProvider
{
    /// <inheritdoc cref="IElapsedTimeMeterProvider.GetElapsedTimeMeter"/>
    public IElapsedTimeMeter GetElapsedTimeMeter(bool enableAutoStartup = false)
    {
        return new ElapsedTimeMeter(enableAutoStartup);
    }
}