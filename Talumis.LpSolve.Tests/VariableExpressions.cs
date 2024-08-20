using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class VariableExpressions
  {
    [TestMethod]
    public void CanMultiplyByConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 5 * x;
      Assert.AreEqual( "5 * x", expression.ToString() );
    }

    [TestMethod]
    public void MultiplyByZeroIsZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 0 * x;
      Assert.AreEqual( "0", expression.ToString() );
    }

    [TestMethod]
    public void MultiplyByOneIsVariable()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 1 * x;
      Assert.AreEqual( expression, x );
    }

    [TestMethod]
    public void MultiplyByNegativeOneIsMinusVariable()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = -1.0 * x;
      Assert.AreEqual( "-x", expression.ToString() );
    }


    [TestMethod]
    public void CanDivideByConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = x / 2;
      Assert.AreEqual( "0.5 * x", expression.ToString() );
    }

    [TestMethod]
    public void CanMultiplyByConstantFromTheRight()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = x * 5;
      Assert.AreEqual( "5 * x", expression.ToString() );
    }

    [TestMethod]
    public void CanChainMultiplications()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 3 * 5 * x;
      Assert.AreEqual( "15 * x", expression.ToString() );
    }

    [TestMethod]
    public void CanAddLinearCombinations()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var expression = 3 * x + 5 * y;
      Assert.AreEqual( "3 * x + 5 * y", expression.ToString() );
    }

    [TestMethod]
    public void CanAddAndSubtractForZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var expression = ( 3 * x ) + ( 5 * y ) - ( 3 * x ) - ( 5 * y );
      Assert.AreEqual( "0", expression.ToString() );
    }

    [TestMethod]
    public void ThreeMinusTwoEqualsOne()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var three = 3 * x;
      var two = 2 * x;
      var res = three - two;
      var expression = 3 * x - 2 * x;
      Assert.AreEqual( "x", expression.ToString() );

      var expression2 = 2 * x - 3 * x;
      Assert.AreEqual( "-x", expression2.ToString() );
    }

    [TestMethod]
    public void TwoTimesThreeIsSix()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 3 * x * 2;
      Assert.AreEqual( "6 * x", expression.ToString() );

      var expression2 = 3 * 2 * x;
      Assert.AreEqual( "6 * x", expression2.ToString() );
    }

    [TestMethod]
    public void SixByTwoIsThree()
    {

      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 6 * x / 2;
      Assert.AreEqual( "3 * x", expression.ToString() );
    }

  }
}
