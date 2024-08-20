using LpSolveDotNet;

namespace Test
{
  internal class Program
  {
    static void Main( string[] args )
    {
      LpSolve.Init();
      using var lp = LpSolve.make_lp( 0, 2 );
      lp.str_add_column( "x" );
      lp.str_add_column( "y" );
      lp.add_constraint( [ 0, 120, 210 ], lpsolve_constr_types.LE, 15000 );
      lp.add_constraint( [ 0, 110, 30 ], lpsolve_constr_types.LE, 4000 );
      lp.add_constraint( [ 0, 1, 1 ], lpsolve_constr_types.LE, 75 );
      lp.add_constraint( [ 0, 1, 0 ], lpsolve_constr_types.GE, 0 );
      lp.add_constraint( [ 0, 0, 1 ], lpsolve_constr_types.GE, 0 );

      lp.set_int( 1, true );
      lp.set_int( 2, true );

      lp.set_obj_fn( [ 0, 143, 60 ] );
      lp.set_maxim();
      lp.print_lp();

      lp.solve();
      lp.print_objective();
      lp.print_solution( 1 );
    }
  }
}
