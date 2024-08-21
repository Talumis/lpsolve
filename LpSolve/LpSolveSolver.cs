using LpSolveDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver
{
  public class LpSolveSolver : Solver, IDisposable
  {
    #region Disposable pattern

    private bool addRowMode = false;
    private bool disposedValue;
    private LpSolve? solver;

    public LpSolveSolver( Model model )
      : base( model )
    {
      LpSolve.Init();
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose( disposing: true );
      GC.SuppressFinalize( this );
    }

    public void EnsureAddRowMode( bool addRowMode )
    {
      if( this.addRowMode != addRowMode )
      {
        this.solver!.set_add_rowmode( addRowMode );
        this.addRowMode = addRowMode;
      }
    }

    protected virtual void Dispose( bool disposing )
    {
      if( !this.disposedValue )
      {
        if( disposing )
        {
          if( this.solver != null )
          {
            this.solver.Dispose();
            this.solver = null;
          }
        }

        this.disposedValue = true;
      }
    }

    private class SparseData
    {
      public SparseData( LinearCombination linearCombination )
      {
        this.Count = linearCombination.Coefficients.Count;
        this.Indices = new int[ this.Count ];
        this.Values = new double[ this.Count ];

        int index = 0;
        foreach( var kvp in linearCombination.Coefficients )
        {
          this.Indices[ index ] = kvp.Key.Column + 1;
          this.Values[ index ] = kvp.Value;
          index++;
        }
      }

      public int Count { get; }
      public int[] Indices { get; }
      public double[] Values { get; }
    }

    #endregion Disposable pattern

    public override void AddConstraint( Constraint constraint )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( true );

      var data = new SparseData( constraint.Expression );
      var comparisonOperator = constraint.Operator switch
      {
        ComparisonOperator.LessThan => lpsolve_constr_types.LE,
        ComparisonOperator.LessThanOrEqual => lpsolve_constr_types.LE,
        ComparisonOperator.GreaterThan => lpsolve_constr_types.GE,
        ComparisonOperator.GreaterThanOrEqual => lpsolve_constr_types.GE,
        ComparisonOperator.Equal => lpsolve_constr_types.EQ,
        _ => throw new InvalidOperationException( "Unknown comparison operator." )
      };

      solver.add_constraintex( data.Count, data.Values, data.Indices, comparisonOperator, constraint.Value );
    }

    public override void AddVariable( Variable variable )
    {
      if( variable.Name != null )
      {
        var solver = CheckSolverInitialized();
        EnsureAddRowMode( false );
        solver.set_col_name( variable.Column + 1, variable.Name );
      }
    }

    public override void SetLowerBound( Variable variable, double value )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_lowbo( variable.Column + 1, value );
      }
    }

    public override void SetObjective( LinearCombination linearCombination, Objective objective = Objective.Minimize )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( true );

      var data = new SparseData( linearCombination );
      solver.set_obj_fnex( data.Count, data.Values, data.Indices );

      switch( objective )
      {
        case Objective.Minimize:
          solver.set_minim();
          break;

        case Objective.Maximize:
          solver.set_maxim();
          break;

        default:
          throw new InvalidOperationException( "Unknown objective." );
      }
    }

    public override void SetUpperBound( Variable variable, double value )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_upbo( variable.Column + 1, value );
      }
    }

    public override void SetVariableBounds( Variable variable, double lowerBound, double upperBound )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_bounds( variable.Column + 1, lowerBound, upperBound );
      }
    }

    public override void VariableIsBoolean( Variable variable )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_binary( variable.Column + 1, true );
      }
    }

    public override void VariableIsInteger( Variable variable )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_int( variable.Column + 1, true );
      }
    }

    public override void VariableIsReal( Variable variable )
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );
      if( variable.Name != null )
      {
        solver.set_binary( variable.Column + 1, false );
        solver.set_int( variable.Column + 1, false );
      }
    }

    protected override bool BuildModel()
    {
      this.solver = LpSolve.make_lp( 0, Model.NumberOfVariables );
      return base.BuildModel();
    }

    protected override bool SolveModel()
    {
      var solver = CheckSolverInitialized();
      EnsureAddRowMode( false );

      if( solver.solve() != lpsolve_return.OPTIMAL )
      {
        return false;
      }

      this.hasSolution = true;
      this.objectiveValue = solver.get_objective();

      var values = new double[ this.Model.NumberOfVariables ];
      solver.get_variables( values );

      this.solution = new();
      foreach( var variable in this.Model.Variables )
      {
        this.solution.Add( variable, values[ variable.Column ] );
      }

      return true;
    }

    private LpSolve CheckSolverInitialized()
          => this.solver
        ?? throw new InvalidOperationException( "The solver has not been initialized." );
  }
}