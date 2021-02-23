using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    namespace Piece
    {

        public abstract class SoldierPiece : GamePieceBase
        {
            public CommanderPiece commander;

            public override abstract List<Definitions.Action> Explore();
        }

    }
}
