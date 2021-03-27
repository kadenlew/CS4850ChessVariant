using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


using Chess.Piece;
using Chess.Control;

namespace Chess {

public class BoardController : MonoBehaviour {
    public int dimensions = 8;
    public float pieceSize = 2f;

    public bool is_white_turn { get; protected set; } = true;

    public Definitions.PrefabCollection prefabs;

    protected List<Control.PlayerBase> players_;

    public HashSet<Definitions.Action> possible_actions { get {
        return players_[is_white_turn ? 0 : 1].get_possible_actions();
    }}

    public Dictionary<Definitions.BoardPosition, Definitions.Tile> board_tiles { get; protected set; }
    public Dictionary<Definitions.BoardPosition, Piece.GamePieceBase> board_lookup { get; protected set; }

    //comment
    // Start is called before the first frame update
    void Start() {
        // reserve storage structures
        players_ = new List<Control.PlayerBase>(2);
        board_lookup = new Dictionary<Definitions.BoardPosition, Piece.GamePieceBase>();

        players_.Add(
            new Control.PlayerBase(
                true,
                prefabs,
                this
            )
        );

        players_.Add(
            new Control.PlayerBase(
                false,
                prefabs,
                this
            )
        );

        InitializeBoard();
        set_transforms();
        update_lookup();
        start_turn();
    }

    // Update is called once per frame
    void Update(){
        set_transforms();
    }

    public void start_turn() {
        // do start turn step for this player
        players_[is_white_turn ? 0 : 1].begin_turn();

        player_explore(); 
    }

    public void end_turn() {
        // do end step 
        players_[is_white_turn ? 0 : 1].end_turn();

        // flip to other person
        is_white_turn = !is_white_turn;

        // start the next turn
        start_turn();
    }

    public Definitions.Result execute_action(Definitions.Action action) {
        // check if this is a valid action
        if(!possible_actions.Contains(action))
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

    public HashSet<Definitions.Action> get_piece_actions(Piece.GamePieceBase piece) {
        var res = new HashSet<Definitions.Action>();
        foreach(Definitions.Action action in possible_actions) {
            if(Object.ReferenceEquals(action.agent, piece))
                res.Add(action);
        }
        return res;
    }
}

} // Chess
