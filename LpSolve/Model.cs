
using LpSolveDotNet;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Talumis.LpSolver
{
  public class Constraint
  {
  }

  public class LinearCombination
  {
    protected Dictionary<Variable, double> terms;

    protected LinearCombination()
    {
      this.terms = new();
    }

    internal LinearCombination( LinearCombination other )
    {
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
      foreach( var (variable, coefficient) in a.terms )
      {
        result.Add( variable, r * coefficient );
      }

      return result;
    }


    public static LinearCombination operator *( LinearCombination a, double r )
      => r * a;

    public static LinearCombination operator /( LinearCombination variable, double r )
      => ( 1.0 / r ) * variable;

    public static LinearCombination operator +( LinearCombination a, LinearCombination b )
    {
      var result = new LinearCombination();
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


    public override string ToString()
    {
      if( terms.All( kvp => kvp.Value == 0.0 ) )
      {
        return "0";
      }

      StringBuilder sb = new();
      foreach( var (variable, coefficient) in terms )
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

      return sb.ToString();
    }
  }

  public class Variable : LinearCombination
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

    internal Variable( int column )
    {
      base.terms.Add( this, 1.0 );
      this.Column = column;
    }

    public string? Name { get; init; }

    internal int Column { get; init; }

    public override string ToString() => Name ?? $"x[{Column}]";

  }

  public class Model
  {
    private Dictionary<string, Variable> namedVariables = new();
    private List<Variable> variables = new();
    private List<Constraint> constraints = new();

    public Variable AddVariable( string name )
    {
      var variable = new Variable( variables.Count + 1, name );
      variables.Add( variable );
      namedVariables.Add( name, variable );
      return variable;
    }

    public Variable AddVariable()
    {
      var variable = new Variable( variables.Count + 1 );
      variables.Add( variable );
      return variable;
    }

    public Variable[] AddVariables( int numberOfVariables )
      => Enumerable.Range( 0, numberOfVariables ).Select( _ => AddVariable() ).ToArray();

    public Variable[] AddVariables( params string[] names )
      => names.Select( AddVariable ).ToArray();

    public Variable this[ string name ]
      => this.namedVariables[ name ];

    public Variable this[ int index ]
      => this.variables[ index ];

  }
}