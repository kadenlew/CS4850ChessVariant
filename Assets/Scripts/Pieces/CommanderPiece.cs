using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    namespace Piece
    {

        public class CommanderPiece : GamePieceBase
        {
            public List<SoldierPiece> soldiers;

            // method to work with Explore() method from GamePieceBase class
            public List<string> CollectActions() 
            {
                List<string> actions = new List<string>();
                foreach (var piece in soldiers) 
                {
                    List<string> tempActions = piece.Explore();
                    foreach (var action in tempActions)
                    {
                        actions.Add(action);
                    }
                }
                return actions;
            }

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
