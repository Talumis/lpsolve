using System.Reflection.Metadata;

namespace Talumis.LpSolver
{
  public class Constraint : IEquatable<Constraint>
  {
    internal Constraint( LinearCombination expression, double value, ComparisonOperator comparison )
    {
      double constant = expression.Constant;
      this.Expression = expression - constant;
      this.Value = value - constant;
      this.Operator = comparison;
    }

    public LinearCombination Expression { get; init; }

    public double Value { get; init; }

    public ComparisonOperator Operator { get; init; }

    public bool Equals( Constraint? other )
    {
      // We are not null, so if other is null we are not the same.
      if( ReferenceEquals( null, other ) )
      {
        return false;
      }

      // If we are literally the same object, we are the same.
      if( ReferenceEquals( this, other ) )
      {
        return true;
      }

      // We are the same constraint if all our fields match
      return this.Value.Equals( other.Value )
        && this.Operator == other.Operator
        && this.Expression.Equals( other.Expression );
    }

    public override string ToString()
      => $"{Expression} {Operator.Symbol()} {Value}";
  }
}