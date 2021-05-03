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

    public static double compute_risk(Definitions.ActionDatabase database, Definitions.Action action) {
        // the risk is defined as the change in risk between our current position and our new potential postion
        // negative values mean our current position is more risky than our new position
        return compute_final_risk(database, action) - compute_base_risk(database, action.agent);
    }

    public static double compute_final_risk(Definitions.ActionDatabase database, Definitions.Action action) {
        Dictionary<Piece.CommanderPiece, double> total_risk = new Dictionary<CommanderPiece, double>();

        Definitions.BoardPosition final_pos = get_ending_position(action);

        HashSet<Definitions.Action> actions_moving_to_final;
        if(database.get_actions(final_pos, out actions_moving_to_final))
        {
            // for each possible action, find the one with the highest chance of killing me
            foreach(Definitions.MoveAction potential_attack in actions_moving_to_final) {
                // what is the chance that this attack succeeds
                // note: Agent is the piece attacking OUR agent in this instance
                double risk = Definitions.AttackAction.get_roll_prob(potential_attack.agent.type, action.agent.type); 

                // determine which commander is commanding this agent
                Piece.CommanderPiece commander = (potential_attack.agent is Piece.SoldierPiece) ? (potential_attack.agent as Piece.SoldierPiece).commander : potential_attack.agent as Piece.CommanderPiece;

                // update with new maximum risk for this commander
                if(total_risk.ContainsKey(commander)) {
                    if(risk > total_risk[commander])
                        total_risk[commander] = risk;
                }
                else {
                    total_risk[commander] = risk;
                }
            } 
        }

        // also include hypothesis (TODO)

        // compute the liklihood that all attacks against this piece would fail
        double final_risk = 1;
        foreach(var risk in total_risk) {
            final_risk *= (1 - risk.Value);
        }

        return final_risk;
    }

    public static double compute_base_risk(Definitions.ActionDatabase database, GamePieceBase piece_ref) {
        Dictionary<Piece.CommanderPiece, double> total_risk = new Dictionary<CommanderPiece, double>();

        // get all of the moves that currently target this piece if it does not move
        HashSet<Definitions.Action> current_risky_actions;
        if(database.all_attacks_targeting(piece_ref, out current_risky_actions))
        {
            // for each possible attack, find the one with the highest chance of killing me
            foreach(Definitions.AttackAction attack in current_risky_actions) {
                // what is the chance that this attack succeeds
                // note: Agent is the piece attacking OUR agent in this instance
                double risk = Definitions.AttackAction.get_roll_prob(attack.agent.type, attack.target.type); 

                // determine which commander is commanding this agent
                Piece.CommanderPiece commander = (attack.agent is Piece.SoldierPiece) ? (attack.agent as Piece.SoldierPiece).commander : attack.agent as Piece.CommanderPiece;

                // update with new maximum risk for this commander
                if(total_risk.ContainsKey(commander)) {
                    if(risk > total_risk[commander])
                        total_risk[commander] = risk;
                }
                else {
                    total_risk[commander] = risk;
                }
            } 
        }

        // compute the liklihood that all attacks against this piece would fail
        double base_risk = 1;
        foreach(var risk in total_risk) {
            base_risk *= (1 - risk.Value);
        }

        return base_risk;
    }
}
    
}
}
}