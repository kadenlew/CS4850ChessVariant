using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{
namespace AI
{

public class AIRisk {
    public static Definitions.BoardPosition get_ending_position(Definitions.Action action) {
        if(action is Definitions.MoveAction)
            return (action as Definitions.MoveAction).target;
        else
        {
            return (action as Definitions.AttackAction).target.position; 
        }
    }

    public static double compute_base_risk(Definitions.ActionDatabase database, GamePieceBase piece_ref) {
        double current_risk = 0;

        // get all of the moves that currently target this piece if it does not move
        HashSet<Definitions.Action> current_risky_actions;
        if(database.all_attacks_targeting(piece_ref, out current_risky_actions))
        {
            // for each possible attack, find the one with the highest chance of killing me
            foreach(Definitions.AttackAction attack in current_risky_actions) {
                // what is the chance that this attack succeeds
                // note: Agent is the piece attacking OUR agent in this instance
                double risk = Definitions.AttackAction.captureTable[(attack.agent.type, attack.target.type)]; 

                if(risk > current_risk)
                    current_risk = risk;
            } 
        }

        return current_risk;
    }

}
    
}
}
}