using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver
{
  public abstract class Solver( Model model ) : ISolver
  {
    protected bool hasSolution;
    protected double objectiveValue;
    protected Dictionary<Variable, double> solution;

    public Model Model { get; } = model;

    public double ObjectiveValue
      => this.hasSolution ? objectiveValue : throw new InvalidOperationException( "Cannot access the solution before the solver has run." );

    public Dictionary<Variable, double> Solution
      => this.hasSolution
      ? this.solution
      : throw new InvalidOperationException( "Cannot access the solution before the solver has run." );

    public double this[ Variable variable ]
                  => this.hasSolution ? solution[ variable ] : throw new InvalidOperationException( "Cannot access the solution before the solver has run." );

    public abstract void AddConstraint( Constraint constraint );

    public abstract void AddVariable( Variable variable );

    public abstract void SetLowerBound( Variable variable, double value );

    public abstract void SetObjective( LinearCombination linearCombination, Objective objective = Objective.Minimize );

    public abstract void SetUpperBound( Variable variable, double value );

    public abstract void SetVariableBounds( Variable variable, double lowerBound, double upperBound );

    public bool Solve()
      => this.BuildModel() && this.SolveModel();

    protected virtual bool BuildModel()
    {
      SetObjective( this.Model.Goal, this.Model.Objective );

      foreach( var variable in Model.Variables )
      {
        AddVariable( variable );
      }

      foreach( var constraint in Model.Constraints )
      {
        if( constraint.Expression.HasSingleVariable )
        {
          (var variable, var coefficient) = constraint.Expression.Coefficients.SingleOrDefault( kvp => kvp.Value != 0 );
          if( coefficient == 0 )
          {
            throw new InvalidOperationException( "Cannot put a constraint in the model that has a zero coefficient for the single variable." );
          }

          if( ( constraint.Operator == ComparisonOperator.LessThan ) || ( constraint.Operator == ComparisonOperator.LessThanOrEqual ) )
          {
            SetUpperBound( variable, constraint.Value / coefficient );
          }
          else if( ( constraint.Operator == ComparisonOperator.GreaterThan ) || ( constraint.Operator == ComparisonOperator.GreaterThanOrEqual ) )
          {
            SetLowerBound( variable, constraint.Value / coefficient );
          }
        }
        else
        {
          AddConstraint( constraint );
        }
      }

      return true;
    }

    protected abstract bool SolveModel();
  }
}