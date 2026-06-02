using PaymentGatewayPOC.Utilities.Interfaces;
using PaymentGatewayPOC.Utilities;

namespace PaymentGatewayPOC.UnitTests.Utilities;

public class RandomServiceTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(70)]
    public void GenerateRandomString_ShouldReturnStringOfSpecifiedLength(int length)
    {
        // Arrange
        IRandomService randomService = new RandomService();

        // Act
        string result = randomService.GenerateRandomString(length);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(length, result.Length);
    }
}
