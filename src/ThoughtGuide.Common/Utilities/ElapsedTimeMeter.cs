﻿using System;
using System.Diagnostics;
using ThoughtGuide.Common.Interfaces.Entities;

namespace ThoughtGuide.Common.Utilities;

/// <inheritdoc cref="IElapsedTimeMeter"/>
public class ElapsedTimeMeter : IElapsedTimeMeter
{
    private readonly Stopwatch _stopwatch = new();

    private long _totalElapsedTimeInMs;

    /// <summary>
    /// Initializes a new instance of <see cref="ElapsedTimeMeter"/> class.
    /// </summary>
    /// <param name="autoStartupEnabled">Indicates if the time meter should be started automatically. Optional.</param>
    public ElapsedTimeMeter(bool autoStartupEnabled = false)
    {
        if (autoStartupEnabled)
        {
            Start();
        }
    }

    #region IElapsedTimeMeter

    /// <inheritdoc cref="IElapsedTimeMeter.IsActive"/>
    public bool IsActive { get; private set; }

    /// <inheritdoc cref="IElapsedTimeMeter.IsPaused"/>
    public bool IsPaused { get; private set; }

    /// <inheritdoc cref="IElapsedTimeMeter.ElapsedTimeInMs"/>
    public long ElapsedTimeInMs => GetElapsedTimeInMs();

    /// <inheritdoc cref="IElapsedTimeMeter.TotalElapsedTimeInMs"/>
    public long TotalElapsedTimeInMs => _totalElapsedTimeInMs + GetElapsedTimeInMs();

    /// <inheritdoc cref="IElapsedTimeMeter.Start"/>
    public void Start()
    {
        if (IsActive & !IsPaused)
        {
            throw new InvalidOperationException("The time meter has already been started");
        }

        IsActive = true;
        IsPaused = false;

        _stopwatch.Start();
    }

    /// <inheritdoc cref="IElapsedTimeMeter.Stop"/>
    public void Stop()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("The time meter was not started");
        }

        _stopwatch.Reset(); // Resets the value of the elapsed time.
        _totalElapsedTimeInMs = 0;

        IsActive = false;
        IsPaused = false;
    }

    /// <inheritdoc cref="IElapsedTimeMeter.Restart"/>
    public void Restart()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("The time meter is not active");
        }

        _totalElapsedTimeInMs += GetElapsedTimeInMs();

        _stopwatch.Reset(); // Resets the value of the elapsed time.
        _stopwatch.Start();

        IsPaused = false;
    }

    /// <inheritdoc cref="IElapsedTimeMeter.Pause"/>
    public void Pause()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("The time meter was not started");
        }

        if (IsPaused)
        {
            throw new InvalidOperationException("The time meter is already paused");
        }

        IsPaused = true;

        _stopwatch.Stop(); // It works like a pause.
    }

    #endregion

    #region Private logic

    private long GetElapsedTimeInMs()
    {
        return _stopwatch.ElapsedMilliseconds;
    }

    #endregion
}