using Talumis.LpSolver;

namespace Test
{
  internal class Program
  {
    private static void Main()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var z = model.AddVariable( "z" );
      var Z = model.AddVariable( "Z" );

      model
        .Maximize( ( 143 * x ) + ( 60 * y ) )
        .SuchThat(
          ( 120 * x ) + ( 210 * y ) <= 15000,
          ( 110 * x ) + ( 30 * y ) <= 4000,
          x + y <= 75,
          x >= 0,
          y >= 0,
          z == ( 143 * x ) + ( 60 * y ),
          z == -Z
        );

      using var solver = new LpSolveSolver( model );
      solver.Solve();

      Console.WriteLine( "Objective value: " + solver.ObjectiveValue );
      Console.WriteLine( "Variable values: " );
      foreach( var variable in model.Variables )
      {
        Console.WriteLine( $"* {variable.Name} = " + solver[ variable ] );
      }
    }
  }
}