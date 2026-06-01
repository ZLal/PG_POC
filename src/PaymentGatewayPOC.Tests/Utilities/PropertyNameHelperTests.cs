using System.Linq.Expressions;
using PaymentGatewayPOC.Utilities;

namespace PaymentGatewayPOC.Tests.Utilities;

public class PropertyNameHelperTests
{
    [Fact]
    public void GetPropertyName_ReturnsCorrectName()
    {
        // Arrange & Act
        string nameProp = PropertyNameHelper.GetPropertyName<Person, string>(p => p.Name);
        string ageProp = PropertyNameHelper.GetPropertyName<Person, int>(p => p.Age);

        // Assert
        Assert.Equal("Name", nameProp);
        Assert.Equal("Age", ageProp);
    }
}

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
