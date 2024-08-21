# Purpose

This module wraps the [lpsolve](https://lpsolve.sourceforge.net/5.5/) LP-solver in an easily usable interface. See the example program below.

# Example

```cpp
// Construct the model
var model = new Model();

// Add the variables. By default, variables are unconstrained.
var x = model.AddVariable( "x", VariableType.NonNegative );
var y = model.AddVariable( "y" );
var z = model.AddVariable( "z" );

// Specify the goal function and the constraints
model
  .Maximize( ( 143 * x ) + ( 60 * y ) )
  .SuchThat(
    ( 120 * x ) + ( 210 * y ) <= 15000,
    ( 110 * x ) + ( 30 * y ) <= 4000,
    x + y <= 75,
    // Alternative way to set the type of y to non-negative
    // The condition y >= 0 will be converted to a bound on y, rather than a constraint
    y >= 0, 
    // Redundant variable to show that equality constraints are supported too
    z == x - y
  );

// Create a new solver. Note: the LpSolveSolver is an IDispoable
using var solver = new LpSolveSolver( model );
solver.Solve();

// Retrieve the objective value and variable values from the solver
Console.WriteLine( "Objective value: " + solver.ObjectiveValue );
Console.WriteLine( "Variable values: " );
foreach( var variable in model.Variables )
{
  Console.WriteLine( $"* {variable.Name} = " + solver[ variable ] );
}
```
