using System.Collections.Generic;

namespace Chess 
{
namespace Definitions
{

public class ActionDatabase {
    // stores references to actions which are indicative that the agent of that move 
    // can reach that space within 1 game action
    private Dictionary<BoardPosition, HashSet<Action>> move_to_action_map;

    // stores references to actions that are specific to 1 agent game piece
    private Dictionary<Piece.GamePieceBase, HashSet<Action>> agent_action_map;

    // stores references to actions that all contain actions that will have the 
    // key as the target of the action, that is the key is being attacked in all
    // of these actions by some other agent
    private Dictionary<Piece.GamePieceBase, HashSet<Action>> defending_action_map;

    private Dictionary<BoardPosition, HashSet<Action>> hypothetical_moves;

    // the entire set of actions    
    private HashSet<Action> total_action_set;

    public ActionDatabase() {
        move_to_action_map = new Dictionary<BoardPosition, HashSet<Action>>();
        agent_action_map = new Dictionary<Piece.GamePieceBase, HashSet<Action>>();
        defending_action_map = new Dictionary<Piece.GamePieceBase, HashSet<Action>>();
        total_action_set = new HashSet<Action>();
        hypothetical_moves = new Dictionary<BoardPosition, HashSet<Action>>();
    }     

    public void clear() {
        move_to_action_map.Clear();
        agent_action_map.Clear();
        defending_action_map.Clear();
        total_action_set.Clear();
    }

    private void append_to_dict<T>(Dictionary<T, HashSet<Action>> dict, T key, Action value) {
        // instantiate if a new key
        if(!dict.ContainsKey(key))
            dict[key] = new HashSet<Action>();

        // add the action to that hash set
        dict[key].Add(value);
    }

    public void add_action(Action value) {
        // store this move in the general set
        total_action_set.Add(value);

        // this move is related to this specific agent
        append_to_dict<Piece.GamePieceBase>(
            agent_action_map,
            value.agent,
            value
        );

        // if this is an attack
        if(value is AttackAction)
        {
            append_to_dict<Piece.GamePieceBase>(
                defending_action_map,
                (value as AttackAction).target,
                value
            );
        }

        // if this a move
        if(value is MoveAction)
        {
            append_to_dict<BoardPosition>(
                move_to_action_map,
                (value as MoveAction).target,
                value
            );
        }
    }

    public void add_hypothetical(MoveAction value) {
        append_to_dict<BoardPosition>(
            hypothetical_moves,
            value.target,
            value
        );
    }

    public bool contains_value(Action value) {
        return total_action_set.Contains(value);
    }
    
    public bool get_actions(Piece.GamePieceBase agent, out HashSet<Action> result) {
        return agent_action_map.TryGetValue(
            agent,
            out result
        );
    }

    public bool get_actions(BoardPosition position, out HashSet<Action> result) {
        return move_to_action_map.TryGetValue(
            position,
            out result
        );
    }

    public bool all_attacks_targeting(Piece.GamePieceBase defender, out HashSet<Action> result) {
        return defending_action_map.TryGetValue(
            defender,
            out result
        );
    }

    public bool all_hypothetical_moves_to(BoardPosition position, out HashSet<Action> result) {
        return hypothetical_moves.TryGetValue(
            position,
            out result
        );
    }

}


} // Definitions
} // Chess