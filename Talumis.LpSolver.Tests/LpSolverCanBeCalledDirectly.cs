using LpSolveDotNet;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class LpSolverCanBeCalledDirectly
  {
    [TestMethod]
    public void WithRealSampleModel()
    {
      using var lp = CreateTestModel();

      Assert.AreEqual( lpsolve_return.OPTIMAL, lp.solve() );
      Assert.AreEqual( 6315.625, lp.get_objective() );

      var values = new double[ 2 ];
      lp.get_variables( values );

      Assert.AreEqual( 21.875, values[ 0 ], 1.0E-6 );
      Assert.AreEqual( 53.125, values[ 1 ], 1.0E-6 );
    }

    [TestMethod]
    public void WithIntSampleModel()
    {
      using var lp = CreateTestModel();
      lp.set_int( 1, true );
      lp.set_int( 2, true );

      Assert.AreEqual( lpsolve_return.OPTIMAL, lp.solve() );
      Assert.AreEqual( 6266, lp.get_objective() );

      var values = new double[ 2 ];
      lp.get_variables( values );

      Assert.AreEqual( 22, values[ 0 ] );
      Assert.AreEqual( 52, values[ 1 ] );
    }

    private static LpSolve CreateTestModel()
    {
      LpSolve.Init();
      var lp = LpSolve.make_lp( 0, 2 );

      lp.add_constraint( [ 0, 120, 210 ], lpsolve_constr_types.LE, 15000 );
      lp.add_constraint( [ 0, 110, 30 ], lpsolve_constr_types.LE, 4000 );
      lp.add_constraint( [ 0, 1, 1 ], lpsolve_constr_types.LE, 75 );
      lp.add_constraint( [ 0, 1, 0 ], lpsolve_constr_types.GE, 0 );
      lp.add_constraint( [ 0, 0, 1 ], lpsolve_constr_types.GE, 0 );

      lp.set_maxim();
      lp.set_obj_fn( [ 0, 143, 60 ] );

      return lp;
    }
  }
}