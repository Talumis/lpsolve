using System.Text;

namespace Talumis.LpSolver
{
  public class LinearCombination : IEquatable<LinearCombination>, IEquatable<Variable>
  {
    protected Dictionary<Variable, double> coefficients;
    protected double constant;

    public static implicit operator LinearCombination( double constant )
      => new() { coefficients = [], constant = constant };

    public LinearCombination( Variable variable, double factor = 1.0 )
    {
      this.coefficients = new() { [ variable ] = factor };
    }

    internal LinearCombination( LinearCombination other )
    {
      this.constant = other.constant;
      this.coefficients = new( other.coefficients );
    }

    protected LinearCombination()
    {
      this.coefficients = [];
    }

    public IReadOnlyDictionary<Variable, double> Coefficients => this.coefficients;

    public double Constant => this.constant;

    public bool HasSingleVariable => this.coefficients.Count == 1;

    public bool IsConstant => this.coefficients.All( kvp => kvp.Value == 0.0 );

    public bool IsZero => this.coefficients.All( kvp => kvp.Value == 0.0 ) && ( this.constant == 0.0 );

    public static LinearCombination operator -( LinearCombination a )
      => -1.0 * a;

    public static LinearCombination operator -( LinearCombination a, double r )
      => a + ( -r );

    public static LinearCombination operator -( double r, LinearCombination a )
      => ( -1.0 * a ) + r;

    public static LinearCombination operator -( LinearCombination a, Variable b )
     => a - new LinearCombination( b );

    public static LinearCombination operator -( Variable b, LinearCombination a )
      => new LinearCombination( b ) - a;

    public static LinearCombination operator -( LinearCombination a, LinearCombination b )
      => a + ( -1.0 * b );

    public static LinearCombination operator *( double r, LinearCombination a )
    {
      if( r == 1.0 )
      {
        return a;
      }

      var result = new LinearCombination { constant = r * a.constant };

      foreach( var (variable, coefficient) in a.coefficients )
      {
        result.Add( variable, r * coefficient );
      }

      return result;
    }

    public static LinearCombination operator *( LinearCombination a, double r )
      => r * a;

    public static LinearCombination operator /( LinearCombination variable, double r )
      => 1.0 / r * variable;

    public static LinearCombination operator +( LinearCombination a, Variable b )
      => a + new LinearCombination( b );

    public static LinearCombination operator +( Variable b, LinearCombination a )
      => new LinearCombination( b ) + a;

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

    public static LinearCombination operator +( LinearCombination a, LinearCombination b )
    {
      var result = new LinearCombination
      {
        constant = a.constant + b.constant
      };
      foreach( var (variable, coefficient) in a.coefficients )
      {
        result.Add( variable, coefficient );
      }

      foreach( var (variable, coefficient) in b.coefficients )
      {
        result.Add( variable, coefficient );
      }

      return result;
    }

    public static Constraint operator <( LinearCombination expression, double value )
      => new( expression, value, ComparisonOperator.LessThan );

    public static Constraint operator <( double value, LinearCombination expression )
      => new( expression, value, ComparisonOperator.GreaterThan );

    public static Constraint operator <( LinearCombination a, LinearCombination b )
      => new( a - b, 0.0, ComparisonOperator.LessThan );

    public static Constraint operator <=( LinearCombination expression, double value )
      => new( expression, value, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator <=( double value, LinearCombination expression )
      => new( expression, value, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator <=( LinearCombination a, LinearCombination b )
      => new( a - b, 0.0, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator >( LinearCombination expression, double value )
      => new( expression, value, ComparisonOperator.GreaterThan );

    public static Constraint operator >( double value, LinearCombination expression )
      => new( expression, value, ComparisonOperator.LessThan );

    public static Constraint operator >( LinearCombination a, LinearCombination b )
      => new( a - b, 0.0, ComparisonOperator.GreaterThan );

    public static Constraint operator >=( LinearCombination expression, double value )
      => new( expression, value, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator >=( double value, LinearCombination expression )
      => new( expression, value, ComparisonOperator.LessThanOrEqual );

    public static Constraint operator >=( LinearCombination a, LinearCombination b )
      => new( a - b, 0.0, ComparisonOperator.GreaterThanOrEqual );

    public static Constraint operator ==( LinearCombination a, double value )
      => new( a - value, 0.0, ComparisonOperator.Equal );

    public static Constraint operator ==( LinearCombination a, Variable value )
      => a - new LinearCombination( value ) == 0.0;

    public static Constraint operator ==( LinearCombination a, LinearCombination b )
      => a - b == 0.0;

    public static Constraint operator !=( LinearCombination a, double value )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    public static Constraint operator !=( LinearCombination a, Variable value )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    public static Constraint operator !=( LinearCombination a, LinearCombination value )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    public bool Equals( LinearCombination? other )
    {
      // We are not null, so if other is null we are not the same.
      if( other is null )
      {
        return false;
      }

      // If we are literally the same object, we are the same.
      if( ReferenceEquals( this, other ) )
      {
        return true;
      }

      // We cannot be the same if we have different constants or not the same number of terms.
      if( ( this.constant != other.constant ) || ( this.coefficients.Count != other.coefficients.Count ) )
      {
        return false;
      }

      // We checked that our constants match. If we are both constant, we must be the same.
      if( this.IsConstant && other.IsConstant )
      {
        return true;
      }

      // Do the expensive comparison: are all my terms equal to theirs and vice versa?
      return this.coefficients.All( t => other.coefficients.GetValueOrDefault( t.Key ) == t.Value )
        && other.coefficients.All( t => this.coefficients.GetValueOrDefault( t.Key ) == t.Value );
    }

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
      foreach( var (variable, coefficient) in coefficients.OrderBy( t => t.Key.Column ) )
      {
        if( coefficient == 0.0 )
        {
          continue;
        }

        if( sb.Length == 0 )
        {
          if( coefficient < 0 )
          {
            sb.Append( '-' );
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

    internal LinearCombination Add( Variable variable, double coefficient )
    {
      this.coefficients.TryAdd( variable, 0.0 );
      this.coefficients[ variable ] += coefficient;
      return this;
    }

    public bool Equals( Variable? variable )
      => ( variable is not null )
        && ( this.constant == 0.0 )
        && this.HasSingleVariable
        && this.coefficients.GetValueOrDefault( variable ) == 1.0;

    public override bool Equals( object? obj )
      => Equals( obj as LinearCombination );

    public override int GetHashCode()
      => HashCode.Combine( this.coefficients, this.constant );
  }
}