namespace Talumis.LpSolver.Tests
{
  internal class MockSolver( Model model ) : Solver( model )
  {
    public List<string> Log = new();

    public override void AddConstraint( Constraint constraint )
      => Log.Add( $"AddConstraint({constraint})" );

    public override void AddVariable( Variable variable )
      => Log.Add( $"AddVariable({variable})" );

    public override void SetLowerBound( Variable variable, double value )
      => Log.Add( $"SetLowerBound({variable}, {value})" );

    public override void SetObjective( LinearCombination linearCombination, Objective objective = Objective.Minimize )
      => Log.Add( $"SetObjective({linearCombination}, {objective})" );

    public override void SetUpperBound( Variable variable, double value )
      => Log.Add( $"SetUpperBound({variable}, {value})" );

    public override void SetVariableBounds( Variable variable, double lowerBound, double upperBound )
      => Log.Add( $"SetVariableBounds({variable}, {lowerBound}, {upperBound})" );

    protected override bool SolveModel()
    {
      this.hasSolution = true;
      this.objectiveValue = 6326.625;
      this.solution = new Dictionary<Variable, double> {
        { this.Model[ "x" ], 21.875 },
        { this.Model[ "y" ], 53.125 }
      };

      return true;
    }
  }
}