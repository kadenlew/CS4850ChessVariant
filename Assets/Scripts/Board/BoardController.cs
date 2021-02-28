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

    public List<(bool, List<(GameObject, List<GameObject>)>)> piece_map { get; protected set; }

    public Dictionary<Definitions.BoardPosition, GameObject> board_tiles { get; protected set; }
    public Dictionary<Definitions.BoardPosition, GameObject> board_lookup { get; protected set; }

    //comment
    // Start is called before the first frame update
    void Start() {
        // reserve storage structures
        players_ = new List<Control.PlayerBase>(2);
        piece_map = new List<(bool, List<(GameObject, List<GameObject>)>)>(2);
        board_lookup = new Dictionary<Definitions.BoardPosition, GameObject>();

        players_.Add(
            new Control.PlayerBase(
                true,
                prefabs,
                this
            )
        );

        piece_map.Add(
            (true, players_[players_.Count - 1].pieces)
        );

        players_.Add(
            new Control.PlayerBase(
                false,
                prefabs,
                this
            )
        );

        piece_map.Add(
            (false, players_[players_.Count - 1].pieces)
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
        player_explore();
    }

    public Definitions.Result execute_action(Definitions.Action action) {
        // execute the action
        var result = action.Execute(this);

        // the board state has change, update the lookup table
        update_lookup();

        // re-explore the space since the board state has changed
        player_explore();

        // indicate the action executed successfully
        return result;
    }

    public void player_explore() {
        players_[is_white_turn ? 0 : 1].explore_actions();
    }

    protected void set_transforms(){
        UnityEngine.Profiling.Profiler.BeginSample("Set Piece Transforms");
        foreach(Control.PlayerBase player in players_)
        {
            foreach((GameObject commander, List<GameObject> soldiers) in player.pieces)
            {
                foreach(GameObject piece in soldiers)
                {
                    piece.transform.position = compute_transform(
                        piece.GetComponent<GamePieceBase>().position
                    );

                    var p = piece.GetComponent<GamePieceBase>();
                    var n = p.position + new Definitions.BoardVector(1, 1);
                    
                }
                commander.transform.position = compute_transform(
                    commander.GetComponent<GamePieceBase>().position
                );
            }
        }
        foreach(KeyValuePair<Definitions.BoardPosition, GameObject> kvp in board_tiles)
        {
            kvp.Value.transform.position = compute_transform(
                kvp.Value.GetComponent<Definitions.Tile>().position
            );
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }

    protected Vector3 compute_transform(Definitions.BoardPosition pos) {
        return new Vector3(
            (pos.file - dimensions / pieceSize) * pieceSize - pieceSize / 2f,
            0f,
            (pos.rank - dimensions / pieceSize) * pieceSize - pieceSize / 2f
        );
    }

    private void InitializeBoard() {
        board_tiles = new Dictionary<Definitions.BoardPosition, GameObject>();
        for (int i = 1; i <= dimensions; i++)
        {
            for (int k = 1; k <= dimensions; k++)
            {
                var pos = new Definitions.BoardPosition(i, k);
                // create and init the board
                board_tiles[pos] = Instantiate(prefabs.Tile);

                board_tiles[pos].GetComponent<Definitions.Tile>().init(
                    new Definitions.BoardPosition(
                        i, k
                    ),
                    prefabs
                );
            }
        }
    }

    private void update_lookup() {
        board_lookup.Clear();

        foreach((bool color, List<(GameObject, List<GameObject>)> player_pieces) in piece_map)
        {
            foreach((GameObject commander, List<GameObject> soldiers) in player_pieces)
            {
                foreach(GameObject soldier in soldiers)
                {
                    board_lookup[soldier.GetComponent<GamePieceBase>().position] = soldier;
                }

                board_lookup[commander.GetComponent<GamePieceBase>().position] = commander;
            }
        }
    }

    public void remove_piece(
        GameObject piece,
        GameObject pieces_commander
    ) {

    }

    public bool checkPosition(Definitions.BoardPosition pos, out GameObject result) {
        UnityEngine.Profiling.Profiler.BeginSample("Search for Position DICT");
        if(this.board_lookup.TryGetValue(pos, out result))  {
            UnityEngine.Profiling.Profiler.EndSample();
            return true;
        }   
        UnityEngine.Profiling.Profiler.EndSample();
        return false;
    } 
}

} // Chess
