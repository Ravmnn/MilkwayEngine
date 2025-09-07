using System;


namespace Milkway.Exceptions.Tiles;

public class InvalidTileSetSizeException : MilkwayException
{
    private static string LiteralMessage => "Tile set size is not a multiple of the tile size.";


    public InvalidTileSetSizeException() : base(LiteralMessage)
    {
    }

    public InvalidTileSetSizeException(Exception inner) : base(LiteralMessage, inner)
    {
    }
}
