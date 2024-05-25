using System;
using System.Threading;
using ThoughtGuide.Common.Utilities;
using Xunit;

namespace ThoughtGuide.Common.Tests.UtilityTests;

/// <summary>
/// Tests for <see cref="ElapsedTimeMeter"/> class.
/// </summary>
public class ElapsedTimeMeterTests
{
    #region Constructor

    /// <summary>
    /// Test for the constructor of class <see cref="ElapsedTimeMeter"/>.
    /// </summary>
    [Fact]
    public void Constructor_AutoStartupIsEnabled_TimerIsActive()
    {
        // Act
        var timer = new ElapsedTimeMeter(true);

        // Assert
        Assert.True(timer.IsActive);
    }

    /// <summary>
    /// Test for the constructor of class <see cref="ElapsedTimeMeter"/>.
    /// </summary>
    [Fact]
    public void Constructor_AutoStartupIsDisabled_TimerIsNotActive()
    {
        // Act
        var timer = new ElapsedTimeMeter();

        // Assert
        Assert.False(timer.IsActive);
    }

    #endregion

    #region Start

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Start"/> method.
    /// </summary>
    [Fact]
    public void Start_TimerIsNotActive_StartsTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter();

        // Act
        timer.Start();
        DoSomethingInMs(1);

        // Assert
        Assert.True(timer.IsActive);
        Assert.False(timer.IsPaused);
        Assert.NotEqual(0, timer.ElapsedTimeInMs);
        Assert.NotEqual(0, timer.TotalElapsedTimeInMs);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Start"/> method.
    /// </summary>
    [Fact]
    public void Start_TimerIsPaused_ResumesTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        DoSomethingInSeconds(1);
        timer.Pause();
        DoSomethingInSeconds(1); // We do not take this interval into account.

        // Act
        timer.Start();
        DoSomethingInSeconds(1);

        // Assert
        Assert.Equal(2, timer.ElapsedTimeInMs / 1000);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Start"/> method.
    /// </summary>
    [Fact]
    public void Start_TimerIsActive_ThrowsException()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        // Act
        void StartTimer() => timer.Start();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(StartTimer);
        Assert.Equal("The time meter has already been started", exception.Message);
        Assert.True(timer.IsActive);
    }

    #endregion

    #region Stop

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Stop"/> method.
    /// </summary>
    [Fact]
    public void Stop_TimerIsActive_StopsTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        DoSomethingInMs(1);

        var elapsedTimeInMs = timer.ElapsedTimeInMs;
        var totalElapsedTimeInMs = timer.TotalElapsedTimeInMs;

        // Act
        timer.Stop();

        // Assert
        Assert.False(timer.IsActive);
        Assert.False(timer.IsPaused);
        Assert.Equal(0, timer.ElapsedTimeInMs);
        Assert.Equal(0, timer.TotalElapsedTimeInMs);
        Assert.NotEqual(0, elapsedTimeInMs);
        Assert.NotEqual(0, totalElapsedTimeInMs);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Stop"/> method.
    /// </summary>
    [Fact]
    public void Stop_TimerIsNotActive_ThrowsException()
    {
        //Arrange
        var timer = new ElapsedTimeMeter();

        // Act
        void StopTimer() => timer.Stop();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(StopTimer);
        Assert.Equal("The time meter was not started", exception.Message);
        Assert.False(timer.IsActive);
    }

    #endregion

    #region Pause

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Pause"/> method.
    /// </summary>
    [Fact]
    public void Pause_TimerIsActive_PausesTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        DoSomethingInMs(1);

        // Act
        timer.Pause();

        var elapsedTime1 = timer.ElapsedTimeInMs;
        var totalElapsedTime1 = timer.TotalElapsedTimeInMs;

        DoSomethingInMs(1);

        var elapsedTime2 = timer.ElapsedTimeInMs;
        var totalElapsedTime2 = timer.TotalElapsedTimeInMs;

        // Assert
        Assert.True(timer.IsActive);
        Assert.True(timer.IsPaused);
        Assert.Equal(elapsedTime1, elapsedTime2);
        Assert.Equal(totalElapsedTime1, totalElapsedTime2);
        Assert.NotEqual(0, elapsedTime1);
        Assert.NotEqual(0, totalElapsedTime1);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Pause"/> method.
    /// </summary>
    [Fact]
    public void Pause_TimerIsNotActive_ThrowsException()
    {
        //Arrange
        var timer = new ElapsedTimeMeter();

        // Act
        void PauseTimer() => timer.Pause();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(PauseTimer);
        Assert.Equal("The time meter was not started", exception.Message);
        Assert.False(timer.IsActive);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Pause"/> method.
    /// </summary>
    [Fact]
    public void Pause_TimerIsAlreadyOnPause_ThrowsException()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        timer.Pause();

        // Act
        void PauseTimer() => timer.Pause();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(PauseTimer);
        Assert.Equal("The time meter is already paused", exception.Message);
        Assert.True(timer.IsPaused);
    }

    #endregion

    #region Restart

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Restart"/> method.
    /// </summary>
    [Fact]
    public void Restart_TimerIsActive_RestartsTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        DoSomethingInSeconds(1);

        var elapsedTimeInSecondsBeforeReset = timer.ElapsedTimeInMs / 1000;
        var totalElapsedTimeInSecondsBeforeReset = timer.TotalElapsedTimeInMs / 1000;

        // Act
        timer.Restart();

        var statusAfterReset = timer.IsActive;
        var elapsedTimeInSecondsAfterReset = timer.ElapsedTimeInMs / 1000;
        var totalElapsedTimeInSecondsAfterReset = timer.TotalElapsedTimeInMs / 1000;

        DoSomethingInSeconds(1);

        var elapsedTimeInSecondsBeforeStop = timer.ElapsedTimeInMs / 1000;
        var totalElapsedTimeInSecondsBeforeStop = timer.TotalElapsedTimeInMs / 1000;

        timer.Stop();

        // Assert
        Assert.Equal(1, elapsedTimeInSecondsBeforeReset);
        Assert.Equal(1, totalElapsedTimeInSecondsBeforeReset);

        Assert.True(statusAfterReset);
        Assert.Equal(0, elapsedTimeInSecondsAfterReset);
        Assert.Equal(1, totalElapsedTimeInSecondsAfterReset);

        Assert.Equal(1, elapsedTimeInSecondsBeforeStop);
        Assert.Equal(2, totalElapsedTimeInSecondsBeforeStop);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Restart"/> method.
    /// </summary>
    [Fact]
    public void Restart_TimerIsNotActive_ThrowsException()
    {
        //Arrange
        var timer = new ElapsedTimeMeter();

        // Act
        void PauseTimer() => timer.Restart();

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(PauseTimer);
        Assert.Equal("The time meter is not active", exception.Message);
        Assert.False(timer.IsActive);
    }

    /// <summary>
    /// Test for <see cref="ElapsedTimeMeter.Pause"/> method.
    /// </summary>
    [Fact]
    public void Pause_TimerIsPaused_RestartsTimer()
    {
        //Arrange
        var timer = new ElapsedTimeMeter(true);

        DoSomethingInSeconds(1);

        var elapsedTimeInSecondsBeforeReset = timer.ElapsedTimeInMs / 1000;
        var totalElapsedTimeInSecondsBeforeReset = timer.TotalElapsedTimeInMs / 1000;

        timer.Pause();

        // Act
        timer.Restart();

        // Assert
        Assert.Equal(1, elapsedTimeInSecondsBeforeReset);
        Assert.Equal(1, totalElapsedTimeInSecondsBeforeReset);

        Assert.True(timer.IsActive);
        Assert.Equal(0, timer.ElapsedTimeInMs / 1000);
        Assert.Equal(1, timer.TotalElapsedTimeInMs / 1000);
    }

    #endregion

    #region Private logic

    private static void DoSomethingInMs(int millisecondsNumber)
    {
        Thread.Sleep(millisecondsNumber);
    }

    private static void DoSomethingInSeconds(int secondsNumber)
    {
        Thread.Sleep(secondsNumber * 1000);
    }

    #endregion
}