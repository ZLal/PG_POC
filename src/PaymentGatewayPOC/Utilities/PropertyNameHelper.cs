using System.Linq.Expressions;

namespace PaymentGatewayPOC.Utilities;

public static class PropertyNameHelper
{
    /// <summary>
    /// Gets the property name from a lambda expression like: x => x.Property
    /// </summary>
    /// <typeparam name="T">Type containing the property</typeparam>
    /// <typeparam name="TProp">Type of the property</typeparam>
    /// <param name="expression">Lambda expression selecting the property</param>
    /// <returns>The property name as a string</returns>
    /// <exception cref="ArgumentNullException">If expression is null</exception>
    /// <exception cref="ArgumentException">If expression does not refer to a property</exception>
    public static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> expression)
    {
        if (expression == null)
            throw new ArgumentNullException(nameof(expression));

        // Handle conversions (e.g., object casting)
        MemberExpression? memberExp = expression.Body as MemberExpression;
        if (memberExp == null && expression.Body is UnaryExpression unaryExp)
        {
            memberExp = unaryExp.Operand as MemberExpression;
        }

        if (memberExp == null)
            throw new ArgumentException("Expression does not refer to a property.", nameof(expression));

        return memberExp.Member.Name;
    }
}
