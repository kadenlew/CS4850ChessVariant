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

    // allow the AI to selectively choose different
    // behavior strategies per piece at different stages of the game
    // as, depending on board development, some pieces become 
    // more or less important and more or less aggressive
    protected FuzzyRuleBase early_game_behavior;
    protected FuzzyRuleBase mid_game_behavior;
    protected FuzzyRuleBase late_game_behavior;

    public void init () {
        // store a reference to the game piece information
        piece_ref = GetComponent<GamePieceBase>();

        // initialize the controller
        logic_controller = new FuzzyController();

        // create the inputs
        logic_controller.add_input_variable(
            "risk",
            -10,
             10
        );

        logic_controller.add_input_variable(
            "reward",
            -10,
            10
        );

        // label the output
        logic_controller.create_output_variable(
            "desireability",
            0,
            100
        );

        // apply the early game rulebase since we just started
        logic_controller.swap_rule_base(
            early_game_behavior
        );

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

                // set the inputs to the controller
                logic_controller.set_input("risk", AIRisk.compute_base_risk(database, piece_ref));
                logic_controller.set_input("reward", AIReward.compute_reward(database, action));

                // grab the output
                // double desireability = logic_controller.get_output();

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
