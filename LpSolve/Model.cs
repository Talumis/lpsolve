using LpSolveDotNet;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Talumis.LpSolver
{
  public enum Objective
  {
    Minimize, Maximize
  };

  public class Model
  {
    private readonly List<Constraint> constraints = [];
    private readonly Dictionary<string, Variable> namedVariables = [];
    private readonly List<Variable> variables = [];

    public IReadOnlyList<Constraint> Constraints => this.constraints;
    public LinearCombination? Goal { get; set; }
    public int NumberOfConstraints => this.constraints.Count;
    public int NumberOfVariables => this.variables.Count;
    public Objective Objective { get; set; }
    public IReadOnlyList<Variable> Variables => this.variables;

    public Variable this[ string name ]
      => this.namedVariables[ name ];

    public Variable this[ int index ]
      => this.variables[ index ];

    public Variable AddVariable( string name, VariableType? type = null )
    {
      var variable = new Variable( variables.Count, name );
      if( type != null )
      {
        variable.Type = type.Value;
      }

      variables.Add( variable );
      namedVariables.Add( name, variable );
      return variable;
    }

    public Variable AddVariable( VariableType? type = null )
    {
      var variable = new Variable( variables.Count );
      if( type != null )
      {
        variable.Type = type.Value;
      }

      variables.Add( variable );
      return variable;
    }

    public Variable[] AddVariables( int numberOfVariables, VariableType? type = null )
      => Enumerable.Range( 0, numberOfVariables ).Select( _ => AddVariable( type ) ).ToArray();

    public Variable[] AddVariables( params string[] names )
      => names.Select( name => AddVariable( name ) ).ToArray();

    public Model Maximize( LinearCombination goal )
    {
      this.Goal = goal;
      this.Objective = Objective.Maximize;
      return this;
    }

    public Model Minimize( LinearCombination goal )
    {
      this.Goal = goal;
      this.Objective = Objective.Maximize;
      return this;
    }

    public Model SuchThat( Constraint constraint )
    {
      constraints.Add( constraint );
      return this;
    }

    public Model SuchThat( params Constraint[] constraint )
    {
      constraints.AddRange( constraint );
      return this;
    }
  }
}