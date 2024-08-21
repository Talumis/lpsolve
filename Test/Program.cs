using LpSolveDotNet;
using Talumis.LpSolver;

namespace Test
{
  internal class Program
  {
    static void Main( string[] args )
    {
      //LpSolve.Init();
      //using var lp = LpSolve.make_lp( 0, 2 );
      //lp.set_col_name( 1, "x" );
      //lp.set_col_name( 2, "y" );

      //lp.set_add_rowmode( true );
      //lp.set_obj_fnex( 2, [ 143, 60 ], [ 1, 2 ] );
      //lp.set_maxim();

      //lp.add_constraintex( 2, [ 120, 210 ], [ 1, 2 ], lpsolve_constr_types.LE, 15000 );
      //lp.add_constraintex( 2, [ 110, 30 ], [ 1, 2 ], lpsolve_constr_types.LE, 4000 );
      //lp.add_constraintex( 2, [ 1, 1 ], [ 1, 2 ], lpsolve_constr_types.LE, 75 );
      //lp.add_constraintex( 2, [ 1, 0 ], [ 1, 2 ], lpsolve_constr_types.GE, 0 );
      //lp.add_constraintex( 2, [ 0, 1 ], [ 1, 2 ], lpsolve_constr_types.GE, 0 );

      //lp.set_add_rowmode( false );

      //lp.set_int( 1, true );
      //lp.set_int( 2, true );

      //lp.print_lp();

      //lp.solve();
      //lp.print_objective();
      //lp.print_solution( 1 );

      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );

      model.Maximize( 143 * x + 60 * y )
        .SuchThat(
          120 * x + 210 * y <= 15000,
          110 * x + 30 * y <= 4000,
          x + y <= 75,
          x >= 0,
          y >= 0
        );

      using var solver = new LpSolveSolver( model );
      solver.Solve();
    }
  }
}
