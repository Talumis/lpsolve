
using LpSolveDotNet;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Talumis.LpSolver
{
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