using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        var risk = logic_controller.add_input_variable("risk", -1, 1);
        risk.add_set_left_shoulder("low_risk", -0.5, 0);
        risk.add_set_triangular("medium_risk", -0.5, 0, 0.5);
        risk.add_set_right_shoulder("high_risk", 0, 0.5);

        var reward = logic_controller.add_input_variable("reward", -10, 10);
        reward.add_set_left_shoulder("low_reward", -5, 0);
        reward.add_set_triangular("medium_reward", -5, 0, 5);
        reward.add_set_right_shoulder("high_reward", 0, 5);

        var desire = logic_controller.create_output_variable("desire", 0, 100); 
        desire.add_set_left_shoulder("low_desire", 25, 50);
        desire.add_set_triangular("medium_desire", 25, 50, 75);
        desire.add_set_right_shoulder("high_desire", 50, 75);

        // load the correct base
        logic_controller.set_rules_from_xml(get_xml_file_path(0));
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
                logic_controller.set_input("reward", AIReward.compute_reward(database, action));
                logic_controller.set_input("risk", AIRisk.compute_risk(database, action));
                double desireability = logic_controller.get_output();

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

    private string get_xml_file_path(int game_state) {
        string path = Path.Combine("ai_rules", piece_ref.type.ToString());

        switch (game_state) {
            case 0:
                path = Path.Combine(path, "early.xml");
                break;
            case 1:
                path = Path.Combine(path, "mid.xml");
                break;
            case 2:
                path = Path.Combine(path, "late.xml");
                break;
        }

        return path;
    }

}

} // AI
} // Piece
} // Chess
