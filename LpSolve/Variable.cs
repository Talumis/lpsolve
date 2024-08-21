using System.Linq.Expressions;

namespace Talumis.LpSolver
{
  public class Variable : IEquatable<LinearCombination>
  {
    internal Variable( int column, string name )
      : this( column )
    {
      if( name.Any( c => !char.IsLetterOrDigit( c ) && ( c != '_' ) ) )
      {
        throw new ArgumentException( "Variable names can only contain letters, digits and underscores." );
      }

      this.Name = name;
    }

    public static LinearCombination operator -( Variable a, double r )
      => new LinearCombination( a ) - r;

    public static LinearCombination operator -( double r, Variable a )
      => r - new LinearCombination( a );

    public static LinearCombination operator -( Variable a, Variable b )
      => new LinearCombination( a ) - new LinearCombination( b );

    public static LinearCombination operator *( double r, Variable a )
      => new LinearCombination( a, r );

    public static LinearCombination operator *( Variable a, double r )
      => new LinearCombination( a, r );

    public static LinearCombination operator /( Variable variable, double r )
      => new LinearCombination( variable, 1.0 / r );

    public static LinearCombination operator +( Variable a, double r )
      => new LinearCombination( a ) + r;

    public static LinearCombination operator +( double r, Variable a )
      => new LinearCombination( a ) + r;

    public static LinearCombination operator +( Variable a, Variable b )
      => new LinearCombination( a ) + new LinearCombination( b );

    public static Constraint operator <( Variable variable, double value )
      => new LinearCombination( variable ) < value;

    public static Constraint operator <( double value, Variable variable )
      => value < new LinearCombination( variable );

    public static Constraint operator <( Variable a, Variable b )
      => new LinearCombination( a ) < new LinearCombination( b );

    public static Constraint operator <=( Variable expression, double value )
      => new LinearCombination( expression ) <= value;

    public static Constraint operator <=( double value, Variable expression )
      => value <= new LinearCombination( expression );

    public static Constraint operator <=( Variable a, Variable b )
      => new LinearCombination( a ) <= new LinearCombination( b );

    public static Constraint operator >( Variable expression, double value )
      => new LinearCombination( expression ) > value;

    public static Constraint operator >( double value, Variable expression )
      => value > new LinearCombination( expression );

    public static Constraint operator >( Variable a, Variable b )
      => new LinearCombination( a ) > new LinearCombination( b );

    public static Constraint operator >=( Variable expression, double value )
      => new LinearCombination( expression ) >= value;

    public static Constraint operator >=( double value, Variable expression )
      => value >= new LinearCombination( expression );

    public static Constraint operator >=( Variable a, Variable b )
     => new LinearCombination( a ) >= new LinearCombination( b );

    internal Variable( int column )
    {
      this.Column = column;
    }

    public string? Name { get; internal init; }

    public int Column { get; internal init; }

    public override string ToString() => Name ?? $"x[{Column}]";

    public bool Equals( LinearCombination? other )
      => ( other != null ) && other.Equals( this );
  }
}