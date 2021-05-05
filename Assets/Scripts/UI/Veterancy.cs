using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    namespace Piece
    {
        public class Veterancy
        {
            public PieceType pieceType;
            public uint kills = 0;
            public uint survival = 0;
            public bool isWhite = true;

            public Veterancy(GamePieceBase piece)
            {
                pieceType = piece.type;
            }
        }
    }
}
