using System.Text;

namespace Talumis.LpSolver
{
  public class LinearCombination : IEquatable<LinearCombination>
  {
    protected double constant;
    protected Dictionary<Variable, double> terms;

    protected LinearCombination()
    {
      this.terms = new();
    }

    internal LinearCombination( LinearCombination other )
    {
      this.constant = other.constant;
      this.terms = new( other.terms );
    }

    public LinearCombination( Variable variable, double factor )
    {
      this.terms = new() { [ variable ] = factor };
    }

    internal LinearCombination Add( Variable variable, double coefficient )
    {
      this.terms.TryAdd( variable, 0.0 );
      this.terms[ variable ] += coefficient;
      return this;
    }

    public static LinearCombination operator *( double r, LinearCombination a )
    {
      if( r == 1.0 )
      {
        return a;
      }

      var result = new LinearCombination();
      result.constant = r * a.constant;
      foreach( var (variable, coefficient) in a.terms )
      {
        result.Add( variable, r * coefficient );
      }

      return result;
    }


    public static LinearCombination operator *( LinearCombination a, double r )
      => r * a;

    public static LinearCombination operator +( LinearCombination a, double r )
    {
      if( r == 0 )
      {
        return a;
      }

      var result = new LinearCombination( a );
      result.constant += r;
      return result;
    }

    public static LinearCombination operator +( double r, LinearCombination a )
      => a + r;

    public static LinearCombination operator -( LinearCombination a, double r )
      => a + ( -r );

    public static LinearCombination operator -( double r, LinearCombination a )
      => ( -1.0 * a ) + r;

    public static LinearCombination operator /( LinearCombination variable, double r )
      => ( 1.0 / r ) * variable;

    public static LinearCombination operator +( LinearCombination a, LinearCombination b )
    {

      var result = new LinearCombination();
      result.constant = a.constant + b.constant;
      foreach( var (variable, coefficient) in a.terms )
      {
        result.Add( variable, coefficient );
      }

      foreach( var (variable, coefficient) in b.terms )
      {
        result.Add( variable, coefficient );
      }

      return result;
    }

    public static LinearCombination operator -( LinearCombination a, LinearCombination b )
      => a + ( -1.0 * b );

    public static Constraint operator <=( LinearCombination expression, double value )
      => new Constraint( expression, value, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator <( LinearCombination expression, double value )
      => new Constraint( expression, value, ComparisonOperator.LessThan );

    public static Constraint operator >=( LinearCombination expression, double value )
      => new Constraint( expression, value, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator >( LinearCombination expression, double value )
      => new Constraint( expression, value, ComparisonOperator.GreaterThan );

    public static Constraint operator <=( double value, LinearCombination expression )
      => new Constraint( expression, value, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator <( double value, LinearCombination expression )
      => new Constraint( expression, value, ComparisonOperator.GreaterThan );

    public static Constraint operator >=( double value, LinearCombination expression )
      => new Constraint( expression, value, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator >( double value, LinearCombination expression )
      => new Constraint( expression, value, ComparisonOperator.LessThan );

    public static Constraint operator <=( LinearCombination a, LinearCombination b )
      => new Constraint( a - b, 0.0, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator <( LinearCombination a, LinearCombination b )
      => new Constraint( a - b, 0.0, ComparisonOperator.LessThan );

    public static Constraint operator >=( LinearCombination a, LinearCombination b )
      => new Constraint( a - b, 0.0, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator >( LinearCombination a, LinearCombination b )
      => new Constraint( a - b, 0.0, ComparisonOperator.GreaterThan );

    public bool IsConstant => this.terms.All( kvp => kvp.Value == 0.0 );

    public bool IsZero => this.terms.All( kvp => kvp.Value == 0.0 ) && ( this.constant == 0.0 );

    public bool HasSingleVariable => this.terms.Count == 1;

    public double Constant => this.constant;

    public override string ToString()
    {
      if( IsZero )
      {
        return "0";
      }

      if( IsConstant )
      {
        return this.constant.ToString();
      }

      StringBuilder sb = new();
      foreach( var (variable, coefficient) in terms.OrderBy( t => t.Key.Column ) )
      {
        if( coefficient == 0.0 )
        {
          continue;
        }

        if( sb.Length == 0 )
        {
          if( coefficient < 0 )
          {
            sb.Append( "-" );
          }
        }
        else
        {
          if( coefficient > 0 )
          {
            sb.Append( " + " );
          }
          else
          {
            sb.Append( " - " );
          }
        }

        if( ( coefficient != 1 ) && ( coefficient != -1 ) )
        {
          sb.Append( Math.Abs( coefficient ) );
          sb.Append( " * " );
        }

        sb.Append( variable );
      }

      if( this.constant != 0.0 )
      {
        // Note: if we get here, we have at least one term
        if( this.constant < 0 )
        {
          sb.Append( " - " );
        }
        else
        {
          sb.Append( " + " );
        }

        sb.Append( Math.Abs( this.constant ) );
      }

      return sb.ToString();
    }

    public bool Equals( LinearCombination? other )
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

      // We cannot be the same if we have different constants or not the same number of terms.
      if( ( this.constant != other.constant ) || ( this.terms.Count != other.terms.Count ) )
      {
        return false;
      }

      // We checked that our constants match. If we are both constant, we must be the same.
      if( this.IsConstant && other.IsConstant )
      {
        return true;
      }

      // Do the expensive comparison: are all my terms equal to theirs and vice versa?
      return this.terms.All( t => other.terms.GetValueOrDefault( t.Key ) == t.Value )
        && other.terms.All( t => this.terms.GetValueOrDefault( t.Key ) == t.Value );
    }
  }
}