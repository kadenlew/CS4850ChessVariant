using Chess.AI;

namespace Chess
{
namespace Piece
{
namespace AI
{

public class AIReward {
    public static double compute_reward(Definitions.ActionDatabase database, Definitions.Action action) {
        if(action is Definitions.MoveAction)
        {
            // moves that move further are better
            Definitions.MoveAction move_action = action as Definitions.MoveAction;
            var difference = (move_action.target - move_action.agent.position); 

            return (
                0.1 * difference.special_mag +                                      // any movement is good
                1.25 * difference.rank_length * ((action.agent.is_white) ? 1 : -1)     // forward movement is particularly good
            );
        }
        else
        {
            // the attack is worth as much as the material of that piece times how likely you are to kill it
            Definitions.AttackAction attack_action = action as Definitions.AttackAction; 
            return attack_action.target.material_value * (attack_action.roll_modifer + Definitions.AttackAction.get_roll_prob(attack_action.agent.type, attack_action.target.type));
        }
    }
}
    
}
}
}