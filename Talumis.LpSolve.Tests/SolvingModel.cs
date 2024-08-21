using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class SolvingModel
  {
    [TestMethod]
    public void AccessingSolutionWithoutSolvingThrowsException()
    {
      var model = BuildModel();
      DummySolver solver = new( model );

      Assert.ThrowsException<InvalidOperationException>( () => solver.ObjectiveValue );
      Assert.ThrowsException<InvalidOperationException>( () => solver[ model[ "x" ] ] );
    }

    [TestMethod]
    public void CanBuildDemoModel()
    {
      var model = BuildModel();

      Assert.AreEqual( 2, model.NumberOfVariables );
      Assert.AreEqual( 5, model.NumberOfConstraints );

      Assert.AreEqual( Objective.Maximize, model.Objective );
      Assert.IsNotNull( model.Goal );
      Assert.AreEqual( "143 * x + 60 * y", model.Goal.ToString() );
    }

    [TestMethod]
    public void SolverSetsObjective()
    {
      var model = BuildModel();
      var solver = new DummySolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "SetObjective(143 * x + 60 * y, Maximize)" ), "Expected call to AddObjective." );
    }

    [TestMethod]
    public void SolverSetsConstraints()
    {
      var model = BuildModel();
      var solver = new DummySolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(120 * x + 210 * y <= 15000)" ), "Expected call to AddConstraint." );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(110 * x + 30 * y <= 4000)" ), "Expected call to AddConstraint." );
      Assert.IsTrue( solver.Log.Contains( "AddConstraint(x + y <= 75)" ), "Expected call to AddConstraint." );
    }

    [TestMethod]
    public void SolverConvertsConstraintsToVariableBounds()
    {
      var model = BuildModel();
      var solver = new DummySolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.IsTrue( solver.Log.Contains( "SetLowerBound(x, 0)" ), "Expected call to SetLowerBound for x." );
      Assert.IsTrue( solver.Log.Contains( "SetLowerBound(y, 0)" ), "Expected call to SetLowerBound for y." );
    }

    [TestMethod]
    public void SolveModel()
    {
      var model = BuildModel();
      DummySolver solver = new( model );

      Assert.AreEqual( model, solver.Model );
      Assert.IsTrue( solver.Solve() );
      Assert.AreEqual( 6326.625, solver.ObjectiveValue );
      Assert.AreEqual( 21.875, solver[ model[ "x" ] ], 1.0E-6 );
      Assert.AreEqual( 53.125, solver[ model[ "y" ] ], 1.0E-6 );
    }

    [TestMethod]
    public void SolveWithLpSolveSolver()
    {
      var model = BuildModel();
      var solver = new LpSolveSolver( model );

      Assert.IsTrue( solver.Solve() );
      Assert.AreEqual( 6326.625, solver.ObjectiveValue );
      Assert.AreEqual( 21.875, solver[ model[ "x" ] ], 1.0E-6 );
      Assert.AreEqual( 53.125, solver[ model[ "y" ] ], 1.0E-6 );
    }

    private Model BuildModel()
    {
      /*
       max(143x + 60y)
       s.t.
        120x + 210y <= 15000
        110x + 30y <= 4000
        x + y <= 75
        x >= 0
        y >= 0
      */
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );

      return model.Maximize( 143 * x + 60 * y )
        .SuchThat(
          120 * x + 210 * y <= 15000,
          110 * x + 30 * y <= 4000,
          x + y <= 75,
          x >= 0,
          y >= 0
        );
    }
  }

  internal class DummySolver( Model model ) : Solver( model )
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

    public override void VariableIsBoolean( Variable variable ) => throw new NotImplementedException();

    public override void VariableIsInteger( Variable variable ) => throw new NotImplementedException();

    public override void VariableIsReal( Variable variable ) => throw new NotImplementedException();
  }
}