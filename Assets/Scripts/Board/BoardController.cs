using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Chess.AI;

using Chess.Piece;
using Chess.Control;

namespace Chess {

public class BoardController : MonoBehaviour {
    public int dimensions = 8;
    public float pieceSize = 2f;

    public bool is_white_turn { get; protected set; } = true;

    public Definitions.PrefabCollection prefabs;

    protected List<Control.PlayerBase> players_;

    public Definitions.ActionDatabase possible_actions { get; protected set; }

    public Dictionary<Definitions.BoardPosition, Definitions.Tile> board_tiles { get; protected set; }
    public Dictionary<Definitions.BoardPosition, Piece.GamePieceBase> board_lookup { get; protected set; }

    public bool do_update = true;

    //comment
    // Start is called before the first frame update
    void Start() {
        
        // // define the controller
        // FuzzyController fc = new FuzzyController();

        // // here are all of the input variables and the output variable
        // FuzzyVariable distance = fc.add_input_variable("DistanceToTarget", 0, 400);
        // FuzzyVariable ammo = fc.add_input_variable("AmmoStatus", 0, 40);
        // FuzzyVariable desire = fc.create_output_variable("Desireability", 0, 100);

        // // first input variable sets
        // FT_Set distance_close = distance.add_set_left_shoulder("Close", 25, 150);
        // FT_Set distance_medium = distance.add_set_triangular("Medium", 25, 150, 300);
        // FT_Set distance_far = distance.add_set_right_shoulder("Far", 150, 300);

        // // second input sets
        // FT_Set ammo_low = ammo.add_set_left_shoulder("Low", 0, 10);
        // FT_Set ammo_okay = ammo.add_set_triangular("Okay", 0, 10, 30);
        // FT_Set ammo_loads = ammo.add_set_right_shoulder("Loads", 10, 30);

        // // output sets
        // FT_Set desire_low = desire.add_set_left_shoulder("low", 25, 50);
        // FT_Set desire_good = desire.add_set_triangular("good", 25, 50, 75);
        // FT_Set desire_great = desire.add_set_right_shoulder("great", 50, 75);
        
        // // Far distance
        // fc.add_rule(
        //     new FT_And(
        //         distance_far, 
        //         ammo_loads
        //     ),
        //     desire_good
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_far, 
        //         ammo_okay
        //     ),
        //     desire_low
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_far, 
        //         ammo_low
        //     ),
        //     desire_low
        // );

        // // Medium Distance
        // fc.add_rule(
        //     new FT_And(
        //         distance_medium, 
        //         ammo_loads
        //     ),
        //     desire_great
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_medium, 
        //         ammo_okay
        //     ),
        //     desire_great
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_medium, 
        //         ammo_low
        //     ),
        //     desire_good
        // );

        // // Close Distance
        // fc.add_rule(
        //     new FT_And(
        //         distance_close, 
        //         ammo_loads
        //     ),
        //     desire_low
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_close, 
        //         ammo_okay
        //     ),
        //     desire_low
        // );

        // fc.add_rule(
        //     new FT_And(
        //         distance_close, 
        //         ammo_low
        //     ),
        //     desire_low
        // );


        // fc.set_input(distance, 200);
        // fc.set_input(ammo, 8);

        // Debug.Log($"The output is: {fc.get_output()}");

        // reserve storage structures
        players_ = new List<Control.PlayerBase>(2);
        board_lookup = new Dictionary<Definitions.BoardPosition, Piece.GamePieceBase>();
        possible_actions = new Definitions.ActionDatabase();

        players_.Add(
            new Control.PlayerAI(
                true,
                prefabs,
                this,
                possible_actions
            )
        );

        // Second player is AI for testing for now        
        players_.Add(
            new Control.PlayerAI(
                false,
                prefabs,
                this,
                possible_actions
            )
        );


        // create the physical board with entitites for clicking and storing positions in them
        InitializeBoard();

        // create the board look up table used to query whether a space is occupied and by who
        update_lookup();

        // start the turn for white
        start_turn();
    }

    // Update is called once per frame
    void Update(){
        if(do_update)
            // set the piece and board tile unity transforms according to board space configuration
            set_transforms();
    }

    public void start_turn() {
        if(!do_update)
            return;
        // explore the current board state
        player_explore(); 

        // inidicate to this player that they may start their turn
        players_[is_white_turn ? 0 : 1].begin_turn();
    }

    public void end_turn() {
        // do end step 
        players_[is_white_turn ? 0 : 1].end_turn();

        // flip to other person
        is_white_turn = !is_white_turn;

        // start the next turn
        start_turn();
    }

    public void end_game(bool is_white) {
        // do_update = false;
        Debug.Log($"{(is_white ? "White" : "Black")} Wins!");
    }

    public Definitions.Result execute_action(Definitions.Action action) {
        // check if this is a valid action
        if(!possible_actions.contains_value(action))
        {
            Debug.Log("Invalid Action!");
            return new Definitions.InvalidResult();
        }
        Debug.Log($"{action}");

        // execute the action
        var result = action.Execute(this);
        Debug.Log($"{result}");

        // the board state has change, update the lookup table
        update_lookup();

        // re-explore the space since the board state has changed
        player_explore();

        // indicate the action executed successfully
        return result;
    }

    public void player_explore() {
        possible_actions.clear();
        // get all the actions
        foreach(var player in players_)
            player.explore_actions();
    }

    protected void set_transforms(){
        // set the positions for pieces of each player
        foreach(Control.PlayerBase player in players_)
        {
            // set the positions of each corp
            foreach((Piece.CommanderPiece commander, List<Piece.SoldierPiece> soldiers) in player.pieces)
            {
                // update the soldiers of this corp
                foreach(Piece.SoldierPiece piece in soldiers)
                {
                    // update the position in unity space
                    piece.gameObject.transform.position = compute_transform(
                        // given the piece's position in board space
                        piece.position
                    );
                    
                }

                // update the commander as well
                // update the position in unity space
                commander.gameObject.transform.position = compute_transform(
                    // given the piece's position in board space
                    commander.position
                );
            }
        }

        // set the positions of each game tile
        foreach(KeyValuePair<Definitions.BoardPosition, Definitions.Tile> kvp in board_tiles)
        {
            // update the position in unity space
            kvp.Value.gameObject.transform.position = compute_transform(
                // given the position in board space
                kvp.Value.position
            );
        }
    }

    protected Vector3 compute_transform(Definitions.BoardPosition pos) {
        return new Vector3(
            (pos.file - dimensions / pieceSize) * pieceSize - pieceSize / 2f,
            0f,
            (pos.rank - dimensions / pieceSize) * pieceSize - pieceSize / 2f
        );
    }

    private void InitializeBoard() {
        board_tiles = new Dictionary<Definitions.BoardPosition, Definitions.Tile>();
        for (int i = 1; i <= dimensions; i++)
        {
            for (int k = 1; k <= dimensions; k++)
            {
                // create the key for this dictionary position
                var pos = new Definitions.BoardPosition(i, k);

                // create the board tile using the prefab, and store a reference to the script component
                board_tiles[pos] = Instantiate(prefabs.Tile).GetComponent<Definitions.Tile>();

                // initialize the script for this tile
                board_tiles[pos].init(
                    pos,
                    prefabs
                );
            }
        }
    }

    private void update_lookup() {
        board_lookup.Clear();
        foreach(Control.PlayerBase player in players_)
        {
            foreach((Piece.CommanderPiece commander, List<Piece.SoldierPiece> soldiers) in player.pieces)
            {
                foreach(Piece.SoldierPiece soldier in soldiers)
                {
                    board_lookup[soldier.position] = soldier;
                }

                board_lookup[commander.position] = commander;
            }
        }
    }

    public bool checkPosition(Definitions.BoardPosition pos, out Piece.GamePieceBase result) {
        if(this.board_lookup.TryGetValue(pos, out result))  {
            return true;
        }   
        return false;
    } 

    public HashSet<Definitions.Action> get_piece_actions(Piece.GamePieceBase agent) {
        // get the possible actions for the specified agent piece
        HashSet<Definitions.Action> results;
        possible_actions.get_actions(agent, out results); 

        // return those results
        return results;
    }
}

} // Chess
