﻿using Buerokratt.Common.AsyncProcessor.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Buerokratt.Common.UnitTests.AsyncProcessor
{
    public class AsyncProcessorLoggerTests
    {
        [Fact]
        public void AsyncProcessorFailedLoggerTest()
        {
            var loggerMock = new Mock<ILogger>();
            _ = loggerMock.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

            loggerMock.Object.AsyncProcessorFailed(new ArgumentNullException("Test"));

            loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                new EventId(10, "AsyncProcessorFailed"),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<ArgumentNullException>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [Fact]
        public void AsyncProcessorStateChangeLoggerTest()
        {
            var loggerMock = new Mock<ILogger>();
            _ = loggerMock.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

            loggerMock.Object.AsyncProcessorStateChange("started");

            loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                new EventId(11, "AsyncProcessorStateChange"),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<ArgumentNullException>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [Fact]
        public void AsyncProcessorTelemetryLoggerTest()
        {
            var loggerMock = new Mock<ILogger>();
            _ = loggerMock.Setup(l => l.IsEnabled(It.IsAny<LogLevel>())).Returns(true);

            loggerMock.Object.AsyncProcessorTelemetry(1, 1000L);

            loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                new EventId(12, "AsyncProcessorTelemetry"),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<ArgumentNullException>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
