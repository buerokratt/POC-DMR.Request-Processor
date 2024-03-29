﻿using Buerokratt.Common.Encoder;
using System;
using Xunit;

namespace Buerokratt.Common.UnitTests.Encoder
{
    public class EncodingServiceTest
    {
        private readonly EncodingService sut;
        private const string plainText = "bürokratt";
        private const string base64Text = "YsO8cm9rcmF0dA==";
        private const string invalidBase64String = "ee8f6ab2-be5b-4712-a684-937a87684c52";

        public EncodingServiceTest()
        {
            sut = new EncodingService();
        }

        [Fact]
        public void DecodeBase64ReturnsPlain()
        {
            // Act
            var result = sut.DecodeBase64(base64Text);

            // Assert
            Assert.Equal(plainText, result);
        }

        [Fact]
        public void DecodeBase64ReturnsArgumentNullException()
        {
            // Act & Assert
            _ = Assert.Throws<ArgumentNullException>(() => sut.DecodeBase64(null));
        }

        [Fact]
        public void DecodeBase64ReturnsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sut.DecodeBase64(invalidBase64String));
            Assert.Equal("Argument is not valid Base64", ex.Message);
        }

        [Fact]
        public void EncodeBase64ReturnsBase64()
        {
            // Act
            var result = sut.EncodeBase64(plainText);

            // Assert
            Assert.Equal(base64Text, result);
        }

        [Fact]
        public void EncodeBase64ReturnsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => sut.EncodeBase64(null));
            Assert.Equal("Value cannot be null. (Parameter 'content')", ex.Message);
        }
    }
}
