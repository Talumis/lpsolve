using LpSolveDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver
{
  public class LpSolveSolver : Solver, IDisposable
  {
    #region Disposable pattern

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

    #endregion Disposable pattern

    protected override bool BuildModel()
    {
      this.solver = LpSolve.make_lp( Model.NumberOfConstraints + 1, Model.NumberOfVariables + 1 );
      return base.BuildModel();
    }

    public override void AddConstraint( Constraint constraint ) => throw new NotImplementedException();

    public override void AddVariable( Variable variable )
    {
      if( variable.Name != null )
      {
        this.solver.set_col_name( variable.Column + 1, variable.Name );
      }
    }

    public override void SetLowerBound( Variable variable, double value ) => throw new NotImplementedException();

    public override void SetObjective( LinearCombination linearCombination, Objective objective = Objective.Minimize ) => throw new NotImplementedException();

    public override void SetUpperBound( Variable variable, double value ) => throw new NotImplementedException();

    public override void SetVariableBounds( Variable variable, double lowerBound, double upperBound ) => throw new NotImplementedException();

    public override void VariableIsBoolean( Variable variable ) => throw new NotImplementedException();

    public override void VariableIsInteger( Variable variable ) => throw new NotImplementedException();

    public override void VariableIsReal( Variable variable ) => throw new NotImplementedException();

    protected override bool SolveModel() => throw new NotImplementedException();
  }
}