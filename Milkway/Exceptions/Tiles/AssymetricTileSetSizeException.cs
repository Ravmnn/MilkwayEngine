using System;


namespace Milkway.Exceptions.Tiles;


public class AssymetricTileSetSizeException : MilkwayException
{
    private static string LiteralMessage => "Tile set width must be equal to its height (symmetric).";


    public AssymetricTileSetSizeException() : base(LiteralMessage)
    {
    }

    public AssymetricTileSetSizeException(Exception inner) : base(LiteralMessage, inner)
    {
    }
}
