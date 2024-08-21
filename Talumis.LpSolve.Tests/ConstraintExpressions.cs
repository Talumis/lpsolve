using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class ConstraintExpressions
  {
    [TestMethod]
    public void LessThanOrEqualConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = x <= 5;
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThanOrEqual, constraint.Operator );
      Assert.AreEqual( "x <= 5", constraint.ToString() );
    }

    [TestMethod]
    public void LessThanConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = x < -5;
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( -5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThan, constraint.Operator );
      Assert.AreEqual( "x < -5", constraint.ToString() );
    }

    [TestMethod]
    public void GreaterThanOrEqualConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = x >= 5;
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.GreaterThanOrEqual, constraint.Operator );
      Assert.AreEqual( "x >= 5", constraint.ToString() );
    }

    [TestMethod]
    public void GreaterThanConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = x > -5;
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( -5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.GreaterThan, constraint.Operator );
      Assert.AreEqual( "x > -5", constraint.ToString() );
    }

    [TestMethod]
    public void LessThanEqualConstantOnTheLeftHandSide()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = 5 <= x;
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.GreaterThanOrEqual, constraint.Operator );
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( "x >= 5", constraint.ToString() );
    }

    [TestMethod]
    public void GreaterThanConstantOnTheLeftHandSide()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = 0 > x;
      Assert.AreEqual( 0.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThan, constraint.Operator );
      Assert.IsTrue( x.Equals( constraint.Expression ) );
      Assert.AreEqual( "x < 0", constraint.ToString() );
    }

    [TestMethod]
    public void LinearCombinationOnTheLeftHandSide()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      var constraint = ( 3 * model[ "x" ] ) + ( 5 * model[ "y" ] ) <= 10;
      Assert.AreEqual( "3 * x + 5 * y <= 10", constraint.ToString() );
      Assert.AreEqual( 10.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThanOrEqual, constraint.Operator );
    }

    [TestMethod]
    public void LinearCombinationOnTheRightHandSide()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      var constraint = 5 >= ( 4 * model[ "x" ] ) + ( 2 * model[ "y" ] );
      Assert.AreEqual( "4 * x + 2 * y <= 5", constraint.ToString() );
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThanOrEqual, constraint.Operator );
    }

    [TestMethod]
    public void LinearCombinationOnBothSides()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      var constraint = ( 3 * model[ "x" ] ) + ( 5 * model[ "y" ] ) <= ( 4 * model[ "x" ] ) + ( 2 * model[ "y" ] );
      Assert.AreEqual( "-x + 3 * y <= 0", constraint.ToString() );
      Assert.AreEqual( 0.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThanOrEqual, constraint.Operator );
    }

    [TestMethod]
    public void LinearCombinationConstantIsPulledToRhs()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var constraint = x - 5 < 0;
      Assert.AreEqual( "x < 5", constraint.ToString() );
      Assert.IsTrue( constraint.Expression.HasSingleVariable );
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThan, constraint.Operator );
    }


    [TestMethod]
    public void LinearCombinationOnBothSidesWithConstants()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      var constraint = ( 3 * model[ "x" ] ) + ( 5 * model[ "y" ] ) - 6 <= ( 4 * model[ "x" ] ) + ( 2 * model[ "y" ] ) + 1;
      Assert.AreEqual( "-x + 3 * y <= 7", constraint.ToString() );
      Assert.AreEqual( 7.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.LessThanOrEqual, constraint.Operator );
    }

    [TestMethod]
    public void EqualityConstraintWithConstantRhsCanBeAdded()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var constraint = x + y == 5;
      Assert.AreEqual( "x + y = 5", constraint.ToString() );
      Assert.AreEqual( 5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.Equal, constraint.Operator );
    }

    [TestMethod]
    public void EqualityConstraintBetweenExpressionAndVariableCanBeAdded()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var constraint = x == 5 - y;
      Assert.AreEqual( "-x - y = -5", constraint.ToString() );
      Assert.AreEqual( -5.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.Equal, constraint.Operator );
    }

    [TestMethod]
    public void EqualityConstraintBetweenExpressionsCanBeAdded()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );

      var constraint = 2 * x == y + 3;
      Assert.AreEqual( "2 * x - y = 3", constraint.ToString() );
      Assert.AreEqual( 3.0, constraint.Value );
      Assert.AreEqual( ComparisonOperator.Equal, constraint.Operator );
    }

    [TestMethod]
    public void InequalityConstraintThrows()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      Assert.ThrowsException<InvalidOperationException>( () => x + y != 5 );
    }

    [TestMethod]
    public void SameConstraintsCompareTheSame()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var constraint1 = ( 3 * x ) + ( 4 * y ) + 5 <= 3;
      var constraint2 = 1 >= ( 4 * y ) - 2 + 5 - ( ( -3 ) * x );

      Assert.AreEqual( "3 * x + 4 * y <= -2", constraint1.ToString() );
      Assert.AreEqual( "3 * x + 4 * y <= -2", constraint2.ToString() );
      Assert.AreEqual( constraint1, constraint2 );
    }

  }
}