using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talumis.LpSolver.Tests
{
  [TestClass]
  public class AddingVariables
  {
    [TestMethod]
    public void VariablesGetZeroBasedColumnIndex()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      Assert.AreEqual( 0, x.Column );
      Assert.AreEqual( 1, y.Column );
    }

    [TestMethod]
    public void CanAddVariables()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      Assert.AreEqual( "x", x.Name );
      Assert.AreEqual( "y", y.Name );
    }

    [TestMethod]
    public void CanAddMultipleVariables()
    {
      var model = new Model();
      var xy = model.AddVariables( "x", "y" );
      Assert.AreEqual( "x", xy[ 0 ].Name );
      Assert.AreEqual( "y", xy[ 1 ].Name );
    }

    [TestMethod]
    public void CanAddAnonymousVariable()
    {
      var model = new Model();
      var x = model.AddVariable();
      Assert.IsNull( x.Name );
    }

    [TestMethod]
    public void CanAddAnonymousVariables()
    {
      var model = new Model();
      var x = model.AddVariable();
      var y = model.AddVariable();
      Assert.IsNull( x.Name );
      Assert.IsNull( y.Name );
    }

    [TestMethod]
    public void CanAddAnonymousVariablesInBulk()
    {
      var model = new Model();
      var x = model.AddVariables( 5 );
      Assert.AreEqual( 5, x.Length );
    }

    [TestMethod]
    public void CannotAddVariableTwice()
    {
      var model = new Model();
      model.AddVariable( "x" );
      Assert.ThrowsException<ArgumentException>( () => model.AddVariable( "x" ) );
    }

    [TestMethod]
    public void CanAccessVariablesThroughIndexer()
    {
      var model = new Model();
      model.AddVariables( "x", "y" );
      Assert.AreEqual( "x", model[ "x" ].Name );
      Assert.AreEqual( "y", model[ "y" ].Name );
    }

    [TestMethod]
    public void CannotAccessNonExistingVariable()
    {
      var model = new Model();
      Assert.ThrowsException<KeyNotFoundException>( () => model[ "x" ] );
    }

    [TestMethod]
    public void CanAccessVariableByIndex()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      var y = model.AddVariable( "y" );
      var z = model.AddVariable();
      Assert.AreEqual( x, model[ 0 ] );
      Assert.AreEqual( y, model[ 1 ] );
      Assert.AreEqual( z, model[ 2 ] );
    }

    [TestMethod]
    public void CannotAccessNonExistingVariableByIndex()
    {
      var model = new Model();
      Assert.ThrowsException<ArgumentOutOfRangeException>( () => model[ 0 ] );
    }

    [TestMethod]
    public void VariableNameMustContainLettersAndDigits()
    {
      var model = new Model();
      Assert.ThrowsException<ArgumentException>( () => model.AddVariable( "#x" ) );
      Assert.ThrowsException<ArgumentException>( () => model.AddVariable( "x[1]" ) );
      Assert.ThrowsException<ArgumentException>( () => model.AddVariable( "x.y" ) );
      Assert.AreEqual( "_x", model.AddVariable( "_x" ).Name );
    }

    [TestMethod]
    public void ToStringReturnsVariableName()
    {
      var model = new Model();
      var x = model.AddVariable();
      var y = model.AddVariable();
      var z = model.AddVariable( "z" );
      Assert.AreEqual( "x[0]", x.ToString() );
      Assert.AreEqual( "x[1]", y.ToString() );
      Assert.AreEqual( "z", z.ToString() );
    }

    [TestMethod]
    public void DefaultVariableTypeIsUnboundedReal()
    {
      var model = new Model();
      var x = model.AddVariable( "x" );
      Assert.AreEqual( VariableType.Real, x.Type );
    }

    [TestMethod]
    public void CanAddVariableWithType()
    {
      var model = new Model();
      var x = model.AddVariable( "x", VariableType.Integer );
      var y = model.AddVariable( "y", VariableType.Boolean );

      Assert.AreEqual( VariableType.Integer, x.Type );
      Assert.AreEqual( VariableType.Boolean, y.Type );
    }
  }
}