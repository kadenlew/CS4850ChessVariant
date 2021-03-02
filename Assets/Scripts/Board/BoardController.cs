using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Chess.Piece;
using Chess.Control;

namespace Chess
{

    public class BoardController : MonoBehaviour
    {
        public int dimensions = 8;
        public float pieceSize = 2f;
        public GameObject plane;
        public Material[] BoardMaterials;

        public Definitions.PrefabCollection prefabs;

        protected List<Control.PlayerBase> players_;

        //comment
        // Start is called before the first frame update
        void Start()
        {
            players_ = new List<Control.PlayerBase>(2);

            players_.Add(
                new Control.PlayerBase(
                    true,
                    prefabs
                )
            );

            players_.Add(
                new Control.PlayerBase(
                    false,
                    prefabs
                )
            );

            InitializeBoard();
            set_transforms();
            init_colors();

        }

        // Update is called once per frame
        void Update()
        {
            set_transforms();
        }

        protected void set_transforms()
        {
            foreach (Control.PlayerBase player in players_)
            {
                foreach ((GameObject commander, List<GameObject> soldiers) in player.getPieces())
                {
                    foreach (GameObject piece in soldiers)
                    {
                        piece.transform.position = compute_transform(
                            piece.GetComponent<GamePieceBase>().GetBoardPosition()
                        );
                        piece.transform.rotation = compute_rotation(
                        piece.GetComponent<GamePieceBase>().is_white()
                    );
                    }
                    commander.transform.position = compute_transform(
                        commander.GetComponent<GamePieceBase>().GetBoardPosition()
                    );
                    commander.transform.rotation = compute_rotation(
                        commander.GetComponent<GamePieceBase>().is_white()
                        );
                }
            }
        }

        protected Vector3 compute_transform(Definitions.BoardPosition pos)
        {
            return new Vector3(
                (pos.get_file() - dimensions / pieceSize) * pieceSize - pieceSize / 2f,
                0f,
                (pos.get_rank() - dimensions / pieceSize) * pieceSize - pieceSize / 2f
            );
        }

        protected Quaternion compute_rotation(bool isWhite)
        {
            if(isWhite)
            {
                return Quaternion.Euler(0, 180, 0);
            }
            else
            {
                return new Quaternion();
            }
        }

        protected void init_colors()
        {
            foreach (Control.PlayerBase player in players_)
            {
                foreach ((GameObject commander, List<GameObject> soldiers) in player.getPieces())
                {
                    foreach (GameObject piece in soldiers)
                    {
                        GamePieceBase p = piece.GetComponent<GamePieceBase>();
                        piece.GetComponentInChildren<Renderer>().material = prefabs.pieceColors[p.is_white() ? 0 : 1];
                        p.Deselect();
                    }

                    GamePieceBase c = commander.GetComponent<GamePieceBase>();
                    commander.GetComponentInChildren<Renderer>().material = prefabs.pieceColors[c.is_white() ? 0 : 1];
                    c.Deselect();
                }
            }
        }

        private void InitializeBoard()
        {
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
                    p.GetComponent<BoardLocation>().location[0] = k;
                    p.GetComponent<BoardLocation>().location[1] = i;

                    cycler++;
                    if (cycler == BoardMaterials.Length)
                        cycler = 0;
                }
                cycler--;
                if (cycler < 0)
                    cycler = BoardMaterials.Length - 1;
            }
        }
    }

}
