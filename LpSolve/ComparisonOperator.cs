namespace Talumis.LpSolver
{
  public enum ComparisonOperator
  {
    Equal,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual
  }

  public static class ComparisonOperatorExtensions
  {
    public static string Symbol( this ComparisonOperator comparison )
    {
      return comparison switch
      {
        ComparisonOperator.Equal => "=",
        ComparisonOperator.LessThan => "<",
        ComparisonOperator.LessThanOrEqual => "<=",
        ComparisonOperator.GreaterThan => ">",
        ComparisonOperator.GreaterThanOrEqual => ">=",
        _ => throw new ArgumentOutOfRangeException( nameof( comparison ) )
      };
    }
  }
}