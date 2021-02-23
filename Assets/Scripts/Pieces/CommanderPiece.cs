using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    namespace Piece
    {

        public abstract class CommanderPiece : GamePieceBase
        {
            public List<SoldierPiece> soldiers;

            public override abstract List<Definitions.Action> Explore();

            /* public List<Definitions.Action> explore(board_state) {
                List<Definitions.Action> res = this.explore();
                foreach(GameObject p : this.soldiers_)
                res.AddRange(p.expolore(board_state));
            } */

            Definitions.PrefabCollection prefabs_;

            public virtual void commander_init(
                bool is_white,
                Definitions.BoardPosition starting_position,
                Definitions.PrefabCollection prefabs
            )
            {
                // save the generic information that all commanders will require
                prefabs_ = prefabs;
                this.position_ = starting_position;
            }
        }

    }
}
