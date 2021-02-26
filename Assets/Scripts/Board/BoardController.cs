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
    public GameObject plane;
    public Material[] BoardMaterials;

    public Definitions.PrefabCollection prefabs;

    protected List<Control.PlayerBase> players_;

    public List<(bool, List<(GameObject, List<GameObject>)>)> piece_map { get; protected set; }
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
        init_colors();
        update_lookup();
    }

    // Update is called once per frame
    void Update(){
        set_transforms();
    }

    protected void set_transforms(){
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
                    var k = (p.position - p.position).manhattan_mag;
                }
                commander.transform.position = compute_transform(
                    commander.GetComponent<GamePieceBase>().position
                );
            }
        }
    }

    protected Vector3 compute_transform(Definitions.BoardPosition pos) {
        return new Vector3(
            (pos.file - dimensions / pieceSize) * pieceSize - pieceSize / 2f,
            0f,
            (pos.rank - dimensions / pieceSize) * pieceSize - pieceSize / 2f
        );
    }

    protected void init_colors() {
        foreach(Control.PlayerBase player in players_)
        {
            foreach((GameObject commander, List<GameObject> soldiers) in player.pieces)
            {
                foreach(GameObject piece in soldiers)
                {
                    GamePieceBase p = piece.GetComponent<GamePieceBase>();
                    p.standard = prefabs.pieceColors[p.is_white ? 0 : 1];
                    p.selected = prefabs.pieceColorsSelected[p.is_white ? 0 : 1];
                    p.Deselect();
                }

                GamePieceBase c = commander.GetComponent<GamePieceBase>();
                c.standard = prefabs.pieceColors[c.is_white ? 0 : 1];
                c.selected = prefabs.pieceColorsSelected[c.is_white ? 0 : 1];
                c.Deselect();
            }
        }
    }

    private void InitializeBoard() {
        int cycler = 0;
        for (int i = 1; i <= dimensions; i++)
        {
            for (int k = 1; k <= dimensions; k++)
            {
                GameObject p = Instantiate(plane);
                p.transform.position = compute_transform(
                    new Definitions.BoardPosition(
                        k, i
                    )
                );

                Renderer target = p.GetComponent<Renderer>();
                target.material = BoardMaterials[cycler];
                
                cycler++;
                if (cycler == BoardMaterials.Length)
                    cycler = 0;
            }
            cycler--;
            if (cycler < 0)
                cycler = BoardMaterials.Length - 1;
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
        Debug.Log(board_lookup.Count);
    }

    public bool checkPosition(Definitions.BoardPosition pos, out GameObject result) {
        if(this.board_lookup.TryGetValue(pos, out result)) 
            return true;
        return false;
    } 

    public bool checkPositionFull(Definitions.BoardPosition position, out GameObject result) {
        foreach((bool color, List<(GameObject, List<GameObject>)> player_pieces) in piece_map)
        {
            foreach((GameObject commander, List<GameObject> soldiers) in player_pieces)
            {
                foreach(GameObject soldier in soldiers)
                {
                    if(soldier.GetComponent<GamePieceBase>().position == position)
                    {
                        result = soldier;
                        return true;
                    }
                }

                if(commander.GetComponent<GamePieceBase>().position == position)
                {
                        result = commander;
                        return true;
                }
            }
        }
        result = null;
        return false;
    }
}

} // Chess
