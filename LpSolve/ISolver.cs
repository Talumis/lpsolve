﻿namespace Talumis.LpSolver
{
  public interface ISolver
  {
    Model Model { get; }

    bool Solve();

    double ObjectiveValue { get; }

    double this[ Variable variable ] { get; }

    Dictionary<Variable, double> Solution { get; }

    void AddVariable( Variable variable );

    void SetUpperBound( Variable variable, double value );

    void SetLowerBound( Variable variable, double value );

    void SetVariableBounds( Variable variable, double lowerBound, double upperBound );

    void SetObjective( LinearCombination linearCombination, Objective objective = Objective.Minimize );

    void AddConstraint( Constraint constraint );
  }
}