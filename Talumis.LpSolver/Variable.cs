namespace Talumis.LpSolver
{
  public class Variable : IEquatable<LinearCombination>
  {
    public VariableType Type { get; set; }

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
      => new( a, r );

    public static LinearCombination operator *( Variable a, double r )
      => new( a, r );

    public static LinearCombination operator /( Variable variable, double r )
      => new( variable, 1.0 / r );

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

    public static Constraint operator ==( Variable a, Variable b )
      => a - b == 0.0;

    public static Constraint operator ==( Variable a, LinearCombination b )
      => b - a == 0.0;

    public static Constraint operator ==( Variable a, double b )
      => a - b == 0.0;

    public static LinearCombination operator -( Variable a )
      => -1.0 * a;

    public static Constraint operator !=( Variable a, Variable b )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    public static Constraint operator !=( Variable a, LinearCombination b )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    public static Constraint operator !=( Variable a, double b )
      => throw new InvalidOperationException( "Not equals constraints are not supported, use two inequalities instead." );

    internal Variable( int column )
    {
      this.Column = column;
    }

    public string? Name { get; internal init; }

    public int Column { get; internal init; }

    public override string ToString() => Name ?? $"x[{Column}]";

    public bool Equals( LinearCombination? other )
      => ( other is not null ) && other.Equals( this );

    public override bool Equals( object? obj )
      => ReferenceEquals( this, obj );

    public override int GetHashCode()
      => HashCode.Combine( this.Column, this.Type, this.Name );
  }
}