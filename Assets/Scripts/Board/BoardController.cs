﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Chess.AI;

using Chess.Piece;
using Chess.Control;

using System.Xml.Serialization;
using System.Xml;
using System.IO;

using Newtonsoft.Json;

namespace Chess
{

public class BoardController : MonoBehaviour
{
    public int dimensions = 8;
    public float pieceSize = 2f;

    public bool is_white_turn { get; protected set; } = true;

    public Definitions.PrefabCollection prefabs;

    protected List<Control.PlayerBase> players_;

    public Definitions.ActionDatabase possible_actions { get; protected set; }

    public Dictionary<Definitions.BoardPosition, Definitions.Tile> board_tiles { get; protected set; }
    public Dictionary<Definitions.BoardPosition, Piece.GamePieceBase> board_lookup { get; protected set; }

    public bool setPositions = true;

    public bool do_update = true;

    public UIController UIController;
    public BezierMovement bezierMover;


    // Start is called before the first frame update
    void Start()
    {
        
        FuzzyController fc = new FuzzyController();
        var t1 = fc.add_input_variable("test_1", 0, 10);
        var t2 = fc.add_input_variable("test_2", 0, 10);
        var output = fc.create_output_variable("output", 0, 10);

        var t1_s = t1.add_set_triangular("main", 0, 5, 10);
        var t2_s = t2.add_set_triangular("main", 0, 5, 10);
        var output_s = output.add_set_triangular("main", 0, 5, 10);

        fc.add_rule(
            new FT_And(t1_s, t2_s),
            output_s
        );

        fc.add_rule(
            new FT_Or(t1_s, t2_s),
            output_s
        );

        // XmlSerializer xs = new XmlSerializer(typeof(FuzzyRuleBase));
        // using (var sww = new StringWriter())
        // {
        //     using (XmlWriter writer = XmlWriter.Create(sww))
        //     {
        //         xs.Serialize(writer, fc.rule_base);
        //         File.WriteAllText("output.xml", sww.ToString());
        //     }
        // }

        Dictionary<string, string> test = new Dictionary<string, string>();
        test["hello"] = "world";
        test["foo"] = "bar";

        Debug.Log(JsonConvert.SerializeObject(test));


        // reserve storage structures
        players_ = new List<Control.PlayerBase>(2);
        board_lookup = new Dictionary<Definitions.BoardPosition, Piece.GamePieceBase>();
        possible_actions = new Definitions.ActionDatabase();

        players_.Add(
            new Control.PlayerBase(
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
    void Update()
    {
        // renable positon control after the animation is finished
        // if(!setPositions && !bezierMover.animating)
            // setPositions = true;

        // set the piece and board tile unity transforms according to board space configuration
        if (setPositions && do_update)
            set_transforms();
    }

    public void start_turn()
    {
        if(!do_update)
            return;

        // explore the current board state
        player_explore();

        // inidicate to this player that they may start their turn
        players_[is_white_turn ? 0 : 1].begin_turn();
    }

    public void end_turn()
    {
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

    public Definitions.Result execute_action(Definitions.Action action)
    {
        // check if this is a valid action
        if (!possible_actions.contains_value(action))
        {
            Debug.Log("Invalid Action!");
            return new Definitions.InvalidResult();
        }
        Debug.Log($"{action}");

        // prepare the controller for animation
        this.setPositions = false;
        var start_position = compute_transform(action.agent.position);

        // execute the action
        var result = action.Execute(this);
        Debug.Log($"{result}");

        // store out ending position to send off the the animator
        var end_position = compute_transform(action.agent.position); 

        // only execute if the move caused a change to board state
        if(result.was_successful)
        {
            bezierMover.ConfigureBezier(start_position, end_position);
            bezierMover.Animate(action.agent.gameObject);
        }
        // nothing changed, reenable piece position control
        else
        {
            this.setPositions = true;
        }

        // the board state has change, update the lookup table
        update_lookup();

        // re-explore the space since the board state has changed
        player_explore();

        // indicate the action executed successfully
        return result;
    }

    public void player_explore()
    {
        possible_actions.clear();
        // get all the actions
        foreach (var player in players_)
            player.explore_actions();
    }

    protected void set_transforms()
    {
        // set the positions for pieces of each player
        foreach (Control.PlayerBase player in players_)
        {
            // set the positions of each corp
            foreach ((Piece.CommanderPiece commander, List<Piece.SoldierPiece> soldiers) in player.pieces)
            {
                // update the soldiers of this corp
                foreach (Piece.SoldierPiece piece in soldiers)
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
        foreach (KeyValuePair<Definitions.BoardPosition, Definitions.Tile> kvp in board_tiles)
        {
            // update the position in unity space
            kvp.Value.gameObject.transform.position = compute_transform(
                // given the position in board space
                kvp.Value.position
            );
        }
    }

    protected Vector3 compute_transform(Definitions.BoardPosition pos)
    {
        return new Vector3(
            (pos.file - dimensions / pieceSize) * pieceSize - pieceSize / 2f,
            0f,
            (pos.rank - dimensions / pieceSize) * pieceSize - pieceSize / 2f
        );
    }

    private void InitializeBoard()
    {
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

    private void update_lookup()
    {
        board_lookup.Clear();
        foreach (Control.PlayerBase player in players_)
        {
            foreach ((Piece.CommanderPiece commander, List<Piece.SoldierPiece> soldiers) in player.pieces)
            {
                foreach (Piece.SoldierPiece soldier in soldiers)
                {
                    board_lookup[soldier.position] = soldier;
                }

                board_lookup[commander.position] = commander;
            }
        }
    }

    public bool checkPosition(Definitions.BoardPosition pos, out Piece.GamePieceBase result)
    {
        if (this.board_lookup.TryGetValue(pos, out result))
        {
            return true;
        }
        return false;
    }

    public HashSet<Definitions.Action> get_piece_actions(Piece.GamePieceBase agent)
    {
        // get the possible actions for the specified agent piece
        HashSet<Definitions.Action> results;
        possible_actions.get_actions(agent, out results);

        // return those results
        return results;
    }
}

} // Chess
