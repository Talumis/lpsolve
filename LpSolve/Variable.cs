namespace Talumis.LpSolver
{
  public class Variable : LinearCombination
  {
    internal Variable( int column, string name )
      : this( column )
    {
      if( name.Any( c => !char.IsLetterOrDigit( c ) && ( c != '_' ) ) )
      {
        throw new ArgumentException( "Variable names can only contain letters, digits and underscores." );
      }

      this.Name = name;
    }

    internal Variable( int column )
    {
      base.terms.Add( this, 1.0 );
      this.Column = column;
    }

    public string? Name { get; init; }

    internal int Column { get; init; }

    public override string ToString() => Name ?? $"x[{Column}]";

  }
}