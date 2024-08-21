using System.Reflection;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class SolvingModel
  {
    [TestMethod]
    public void AccessingSolutionWithoutSolvingThrowsException()
    {
      var model = DefaultModel;
      MockSolver solver = new( model );

      Assert.ThrowsException<InvalidOperationException>( () => solver.ObjectiveValue );
      Assert.ThrowsException<InvalidOperationException>( () => solver[ model[ "x" ] ] );
    }

    [TestMethod]
    public void CanBuildDemoModel()
    {
      var model = DefaultModel;

      Assert.AreEqual( 2, model.NumberOfVariables );
      Assert.AreEqual( 5, model.NumberOfConstraints );

      Assert.AreEqual( Objective.Maximize, model.Objective );
      Assert.IsNotNull( model.Goal );
      Assert.AreEqual( "143 * x + 60 * y", model.Goal.ToString() );
    }

    [TestMethod]
    public void SolverSetsObjective()
    {
      var model = DefaultModel;
      var solver = new MockSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "SetObjective(143 * x + 60 * y, Maximize)" ), "Expected call to AddObjective." );
    }

    [TestMethod]
    public void SolverSetsConstraints()
    {
      var model = DefaultModel;
      var solver = new MockSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(120 * x + 210 * y <= 15000)" ), "Expected call to AddConstraint." );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(110 * x + 30 * y <= 4000)" ), "Expected call to AddConstraint." );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(x + y <= 75)" ), "Expected call to AddConstraint." );
    }

    [TestMethod]
    public void SolverConvertsConstraintsToVariableBounds()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var z = model.AddVariable( "z" );

      model.SuchThat(
        x >= 0,
        y > 5,
        y <= 10,
        5 * z >= 10
      );

      var solver = new MockSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "SetLowerBound(x, 0)" ), "Expected call to SetLowerBound for x." );
      Assert.IsTrue( solver.Log.Contains( "SetLowerBound(y, 5)" ), "Expected call to SetLowerBound for y." );
      Assert.IsTrue( solver.Log.Contains( "SetUpperBound(y, 10)" ), "Expected call to SetUpperBound for y." );
      Assert.IsTrue( solver.Log.Contains( "SetLowerBound(z, 2)" ), "Expected call to SetLowerBound for x." );
    }

    [TestMethod]
    public void SolveModel()
    {
      var model = DefaultModel;
      MockSolver solver = new( model );

      Assert.AreEqual( model, solver.Model );
      Assert.IsTrue( solver.Solve() );
      Assert.AreEqual( 6326.625, solver.ObjectiveValue );
      Assert.AreEqual( 21.875, solver[ model[ "x" ] ], 1.0E-6 );
      Assert.AreEqual( 53.125, solver[ model[ "y" ] ], 1.0E-6 );
    }

    [TestMethod]
    public void SolveWithLpSolveSolver()
    {
      var model = DefaultModel;
      var solver = new LpSolveSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.AreEqual( 6315.625, solver.ObjectiveValue );
      Assert.AreEqual( 21.875, solver[ model[ "x" ] ], 1.0E-6 );
      Assert.AreEqual( 53.125, solver[ model[ "y" ] ], 1.0E-6 );
    }

    [TestMethod]
    public void GoalDefaultsToZeroConstant()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      var solver = new MockSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "SetObjective(0, Minimize)" ), "Expected objective to be set to minimize: '0'" );
    }

    [TestMethod]
    public void LpSolveSolverSetsCorrectVariableTypes()
    {
      var model = new Model();
      var real = model.AddVariable();
      var positive = model.AddVariable( VariableType.NonNegative );
      var integer = model.AddVariable( VariableType.Integer );
      var boolean = model.AddVariable( VariableType.Boolean );

      var solver = new LpSolveSolver( model );
      Assert.IsTrue( solver.Solve() );

      var lpSolver = GetPrivateSolver( solver );
      Assert.IsNotNull( lpSolver, "Expected access to the internal solver." );
      Assert.IsTrue( lpSolver.is_unbounded( real.Column + 1 ), "Expected 'real' to be unbounded." );
      Assert.AreEqual( 0.0, lpSolver.get_lowbo( positive.Column + 1 ) );
      Assert.IsTrue( lpSolver.is_int( integer.Column + 1 ), "Expected 'integer' to be integer." );
      Assert.IsTrue( lpSolver.is_binary( boolean.Column + 1 ), "Expected 'boolean' to be binary." );
    }

    private static LpSolveDotNet.LpSolve? GetPrivateSolver( LpSolveSolver solver )
      => (LpSolveDotNet.LpSolve?)solver.GetType()?.GetField( "solver", BindingFlags.NonPublic | BindingFlags.Instance )?.GetValue( solver );

    private static Model DefaultModel
    {
      get
      {
        var model = new Model();
        var x = model.AddVariable( "x" );
        var y = model.AddVariable( "y" );

        return model.Maximize( ( 143 * x ) + ( 60 * y ) )
          .SuchThat(
            ( 120 * x ) + ( 210 * y ) <= 15000,
            ( 110 * x ) + ( 30 * y ) <= 4000,
            x + y <= 75,
            x >= 0,
            y >= 0
          );
      }
    }
  }
}