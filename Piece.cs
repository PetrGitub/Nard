using System;
using System.Collections.Generic;
using System.Text;

namespace _07_B___poleCELL_piece_STRING
{
    class Piece
    {
        public bool Red { get; set; }
        //public bool Blue { get; set; }
        public Coordinates Coord { get; set; }
        public ShapePiece Shape { get; set; }

        public Piece(Coordinates coord, ShapePiece shape)
        {
            Coord = coord;
            Shape = shape;
        }


        public override string ToString()
        {
            switch (Shape)
            {
                case ShapePiece.WhiteStone:
                    return "o";

                case ShapePiece.BlackStone:
                    return "x";

                case ShapePiece.RedQueen:
                    return (-2).ToString();

                case ShapePiece.BlueQueen:
                    return (2).ToString();

                case ShapePiece.Empty:
                    return ".";

                default:
                    throw new InvalidOperationException("property Shape must be filled up");
            }
        }


        public enum ShapePiece
        {
            WhiteStone,
            BlackStone,
            RedQueen,
            BlueQueen,
            Empty
        }

        public void NewCoords(int x, int y)
        {
            Coord = new Coordinates(x, y);
        }
    }
}
