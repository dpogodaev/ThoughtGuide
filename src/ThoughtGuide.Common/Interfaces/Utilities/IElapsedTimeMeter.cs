using System;

namespace ThoughtGuide.Common.Interfaces.Entities;

/// <summary>
/// The elapsed time meter for measuring the time interval with readings in milliseconds.
/// </summary>
public interface IElapsedTimeMeter
{
    /// <summary>
    /// Indicates if the time meter is active.
    /// </summary>
    /// <remarks>
    /// When the time meter is not active, the values of the <see cref="ElapsedTimeInMs"/> and <see cref="TotalElapsedTimeInMs"/> are <c>0</c>.
    /// </remarks>
    bool IsActive { get; }

    /// <summary>
    /// Indicates if the time meter is paused.
    /// </summary>
    /// <remarks>When the time meter is paused, the value of the <see cref="ElapsedTimeInMs"/> and <see cref="TotalElapsedTimeInMs"/> does not change.</remarks>
    bool IsPaused { get; }

    /// <summary>
    /// Elapsed time in milliseconds.
    /// </summary>
    long ElapsedTimeInMs { get; }

    /// <summary>
    /// Total elapsed time in milliseconds (taking into account the restart).
    /// </summary>
    long TotalElapsedTimeInMs { get; }

    /// <summary>
    /// Starts the time meter.
    /// </summary>
    /// <remarks>After the time meter is started, the <see cref="IsActive"/> is set to <c>true</c>.</remarks>
    /// <exception cref="InvalidOperationException">The timer meter has already been started.</exception>
    void Start();

    /// <summary>
    /// Stops the timer meter.
    /// </summary>
    /// <remarks>
    /// After the time meter is stopped, the <see cref="IsActive"/> is set to <c>false</c>,
    /// <see cref="ElapsedTimeInMs"/> and <see cref="TotalElapsedTimeInMs"/> are set to <c>0</c>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">The timer meter meter was not started.</exception>
    void Stop();

    /// <summary>
    /// Pauses the timer meter when it is active.
    /// </summary>
    /// <remarks>To resume the time meter, call the <see cref="Start"/>.</remarks>
    /// <exception cref="InvalidOperationException">The timer meter was not started or it is already on pause.</exception>
    void Pause();

    /// <summary>
    /// Restarts the timer meter when it is active.
    /// </summary>
    /// <remarks>
    /// The <see cref="ElapsedTimeInMs"/> is set to <c>0</c>.<br/>
    /// The <see cref="TotalElapsedTimeInMs"/> is not reset.
    /// </remarks>
    /// <exception cref="InvalidOperationException">The timer meter is not active.</exception>
    void Restart();
}