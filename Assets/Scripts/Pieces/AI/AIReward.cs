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
            return (move_action.target - move_action.agent.position).special_mag * 0.1;
        }
        else
        {
            // the attack is worth as much as the material of that piece times how likely you are to kill it
            Definitions.AttackAction attack_action = action as Definitions.AttackAction; 
            return attack_action.target.material_value * Definitions.AttackAction.captureTable[(attack_action.agent.type, attack_action.target.type)];
        }
    }
}
    
}
}
}