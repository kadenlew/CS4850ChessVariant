using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Chess
{
namespace Control
{

public class PlayerAI : PlayerBase {
    protected List<Piece.AI.AICommanderEval> commander_AI { get; set; }

    protected BoardController controller_ref { get; set; }

    public float move_delay = 0.25f;

    // echo the base constructor and execute the base constructor
    public PlayerAI(
        bool is_white,
        Definitions.PrefabCollection prefabs,
        BoardController controller,
        Definitions.ActionDatabase action_database
    ) : base(
        is_white, 
        prefabs, 
        controller, 
        action_database
    ) {
        // initialize the commmander AI list
        commander_AI = new List<Piece.AI.AICommanderEval>();

        // after spawning everything like normal, add the commander AI controllers which
        // will in turn add the soldier AI controllers
        foreach(Piece.CommanderPiece commmander in commanders_) {
            // add AI controller to this commander
            commander_AI.Add(
                commmander.gameObject.AddComponent<Piece.AI.AICommanderEval>()
            );

            // call its init function 
            commander_AI[commander_AI.Count - 1].commander_init();
        }

        // save a reference to the board controller for sending it moves
        // to execute
        controller_ref = controller;
    }

    public override void begin_turn()
    {
        controller_ref.StartCoroutine(TurnControlRoutine());
    }

    IEnumerator TurnControlRoutine()
    {
        Debug.Log($"Starting my turn!");
        
        // play each commander in any order (based on desireability scores)
        while(true) 
        {
            Debug.Log("Evaluating my choices...");

            Definitions.Action player_action = null;
            double desireability = double.MinValue;

            // see what the best move is out of all the corps
            foreach(Piece.AI.AICommanderEval commander_ai in commander_AI) {
                var corp_result = commander_ai.commander_eval(ref possible_actions);

                if(corp_result.desireability > desireability) {
                    desireability = corp_result.desireability;
                    player_action = corp_result.action;
                }
            }

            // we have exhausted all of the moves, and must end turn
            if(player_action == null)
                break;

            Debug.Log($"With a score of {desireability}, im making this move! {player_action}");
            yield return new WaitForSeconds(move_delay);

            // request that the board execute the action
            var result = controller_ref.execute_action(player_action); 

            Debug.Log($"I did my action, with a result of {result}");
            yield return new WaitForSeconds(move_delay);
        }

        Debug.Log("I've used all my moves, I'm ending my turn!");
        controller_ref.end_turn();
    }
        
}

} // Control
} // Chess