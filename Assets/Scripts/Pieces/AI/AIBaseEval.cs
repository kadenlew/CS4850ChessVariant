using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using Chess.AI;

namespace Chess
{
namespace Piece
{
namespace AI
{

class AIActionEval : MonoBehaviour
{
    protected Piece.GamePieceBase piece_ref { get; set; }

    protected FuzzyController logic_controller { get; set; }
    // Start is called before the first frame update

    public void init () {
        // store a reference to the game piece information
        piece_ref = GetComponent<GamePieceBase>();

        // initialize the controller
        logic_controller = new FuzzyController();

    }

    public (Definitions.Action action, double desireability) eval(ref Definitions.ActionDatabase database) {
        Definitions.Action best_action = null;
        double best_desireability = double.MinValue;

        // get the actions we need to evaluate for this piece  
        HashSet<Definitions.Action> possible_actions;
        if(database.get_actions(piece_ref, out possible_actions))
        {
            foreach(Definitions.Action action in possible_actions)
            {
                double desireability = AIReward.compute_reward(database, action);
                AIRisk.compute_base_risk(database, piece_ref);

                // double desireability = Random.Range(0f, 10f);
                if(desireability > best_desireability)
                {
                    best_desireability = desireability;
                    best_action = action;
                }
            }
        }

        // there are no actions this piece can take, return
        return (best_action, best_desireability);
    }

}

} // AI
} // Piece
} // Chess
