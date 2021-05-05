using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Profiling;

namespace Chess
{
namespace Control
{

class PlayerAI : PlayerBase {
    protected List<Piece.AI.AICommanderEval> commander_AI { get; set; }


    public float move_delay = .25f;

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
    }

    public void update_rulebase(int turn) {
        foreach(Piece.AI.AICommanderEval commander in commander_AI) {
            if(turn == 3) {
                commander.update_corp_rulebase(1);
                Debug.Log("Turn 3!");
            }
            else if(turn == 10) {
                commander.update_corp_rulebase(2);
                Debug.Log("Turn 10!");
            }
        }
    }

    public override void begin_turn()
    {
        update_rulebase(controller_ref.get_game_turn());
        controller_ref.StartCoroutine(TurnControlRoutine());
    }

    IEnumerator TurnControlRoutine()
    {
        // Debug.Log($"Starting my turn!");
        
        // play each commander in any order (based on desireability scores)
        while(true) 
        {

            while (controller_ref.pauseAI) yield return new WaitForSeconds(0.1f);

            Profiler.BeginSample("AI_One_Move");

            Definitions.Action player_action = null;
            double desireability = double.MinValue;

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // see what the best move is out of all the corps
            foreach(Piece.AI.AICommanderEval commander_ai in commander_AI) {
                var corp_result = commander_ai.commander_eval(ref possible_actions);

                if(corp_result.desireability > desireability) {
                    desireability = corp_result.desireability;
                    player_action = corp_result.action;
                }
            }

            Profiler.EndSample();

            watch.Stop();
            // Debug.Log($"Finished in {watch.ElapsedMilliseconds} ms.");

            // we have exhausted all of the moves, and must end turn
            if(player_action == null)
                break;

            // Debug.Log($"With a score of {desireability}, im making this move! {player_action}");
            yield return new WaitForSeconds(move_delay);

            // request that the board execute the action
            var result = controller_ref.execute_action(player_action); 

            // wait until the animation has completed
            while(controller_ref.pauseAI) yield return new WaitForSeconds(0.1f);

            // Debug.Log($"I did my action, with a result of {result}");
        }

        // Debug.Log("I've used all my moves, I'm ending my turn!");
        controller_ref.end_turn();
    }
        
}

} // Control
} // Chess