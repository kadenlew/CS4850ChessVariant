using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{
namespace AI
{

class AICommanderEval : AIActionEval 
{
    public List<Piece.SoldierPiece> soldier_pieces { get; protected set; }
    public List<AI.AIActionEval> soldier_AI { get; protected set; }
    // Start is called before the first frame update

    public void commander_init() {
        // init the soldier AI data structure
        soldier_AI = new List<AIActionEval>();

        // copy access to the soldiers list
        soldier_pieces = GetComponent<CommanderPiece>().soldiers_;

        foreach(Piece.SoldierPiece soldier in soldier_pieces) {
            // add this soldiers AI controller
            soldier_AI.Add(
                soldier.gameObject.AddComponent<AIActionEval>()
            );

            // call its init function
            soldier_AI[soldier_AI.Count - 1].init();
        }

        // also call the init for AIBaseEval on this piece too
        init();
    }

    public (Definitions.Action action, double desireability) commander_eval(ref Definitions.ActionDatabase database) {
        // accumulator definition for best action
        Definitions.Action corp_action = null;
        double desireability = double.MinValue;

        // ask each soldier what his best move is
        foreach(AIActionEval soldier_ai in soldier_AI) {
            var result = soldier_ai.eval(ref database);

            if(result.desireability > desireability) {
                desireability = result.desireability;
                corp_action = result.action;
            }
        }

        // also weigh in on my own move
        var my_result = eval(ref database);
        if(my_result.desireability > desireability) {
            desireability = my_result.desireability;
            corp_action = my_result.action;
        }

        // signal the best corp move
        return (corp_action, desireability);
    }
    public void update_corp_rulebase(int game_state) {
        // ask each soldier what his best move is
        foreach(AIActionEval soldier_ai in soldier_AI) {
           soldier_ai.update_rulebase(game_state);
        }

        update_rulebase(game_state);
    }
}

} // AI
} // Piece
} // Chess