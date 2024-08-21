namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class VariableExpressions
  {
    [TestMethod]
    public void CanConvertConstantToLinearCombination()
    {
      var model = new Model
      {
        Goal = 42.0
      };

      Assert.IsTrue( model.Goal.IsConstant );
      Assert.AreEqual( "42", model.Goal.ToString() );
    }

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
      Assert.IsTrue( x.Equals( expression ) );
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
      var expression = ( 3 * x ) + ( 5 * y );
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

      var expression = ( 3 * x ) - ( 2 * x );
      Assert.AreEqual( "x", expression.ToString() );

      var expression2 = ( 2 * x ) - ( 3 * x );
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
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void SingleVariableIsSingleVariable()
    {
      var model = new Model();
      var x = 1.0 * model.AddVariable( "x" );
      Assert.IsTrue( x.HasSingleVariable );
      Assert.IsFalse( x.IsConstant );
      Assert.IsFalse( x.IsZero );
    }

    [TestMethod]
    public void MultipleOfSingleVariableIsSingleVariable()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 3 * x;
      Assert.IsTrue( expression.HasSingleVariable );
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void SumOfSingleVariablesIsNotSingleVariable()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var expression = ( 3 * x ) + ( 5 * y );
      Assert.IsFalse( expression.HasSingleVariable );
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void ZeroTimesXIsZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 0 * x;
      Assert.IsTrue( expression.IsZero );
      Assert.IsTrue( expression.IsConstant );
    }

    [TestMethod]
    public void XMinusXIsZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = x - x;
      Assert.IsTrue( expression.IsConstant );
      Assert.IsTrue( expression.IsZero );
    }

    [TestMethod]
    public void PositiveConstantWithoutTerms()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = x - x + 5;
      Assert.AreEqual( "5", expression.ToString() );
      Assert.IsTrue( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void NegativeConstantWithoutTerms()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = x - x - 5;
      Assert.AreEqual( "-5", expression.ToString() );
      Assert.IsTrue( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void CanAddConstantToLinearCombination()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = ( 3 * x ) + 5;
      Assert.AreEqual( "3 * x + 5", expression.ToString() );
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void CanSubtractConstant()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = -2 + ( 3 * x ) + 5;
      Assert.AreEqual( "3 * x + 3", expression.ToString() );
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void CanSubtractConstantToZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = ( 3 * x ) + 5 - x - 5;
      Assert.AreEqual( "2 * x", expression.ToString() );
      Assert.IsFalse( expression.IsConstant );
      Assert.IsFalse( expression.IsZero );
    }

    [TestMethod]
    public void CanSubtractEverythingToZero()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = ( 3 * x ) + 5 - ( 3 * x ) - 5;
      Assert.AreEqual( "0", expression.ToString() );
      Assert.IsTrue( expression.IsConstant );
      Assert.IsTrue( expression.IsZero );
    }

    [TestMethod]
    public void ConstantIsMultipliedByCoefficient()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 3 * ( 5 + x );
      Assert.AreEqual( "3 * x + 15", expression.ToString() );
    }

    [TestMethod]
    public void OperationWithZeroIsOptimizedAway()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 3 * x;

      Assert.AreSame( expression, expression + 0.0 );
      Assert.AreSame( expression, expression - 0.0 );
      Assert.AreNotSame( expression, expression + 1.0 );
    }

    [TestMethod]
    public void EqualExpressionCompareEqual()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var expression1 = ( 3 * x ) + ( 4 * y ) + 5;
      var expression2 = ( 4 * y ) + 5 - ( ( -3 ) * x );
      Assert.AreEqual( "3 * x + 4 * y + 5", expression1.ToString() );
      Assert.AreEqual( "3 * x + 4 * y + 5", expression2.ToString() );
      Assert.AreEqual( expression1, expression2 );
    }

    [TestMethod]
    public void VariableIsEqualToUnitLinearCombination()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var expression = 1 * x;

      Assert.IsTrue( x.Equals( expression ) );
    }
  }
}