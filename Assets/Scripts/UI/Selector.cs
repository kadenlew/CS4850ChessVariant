using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Chess.Piece;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public bool moveAllowed = true;
    public UIController uiController;
    private GameObject selectedPiece = null;
    private Chess.Definitions.BoardPosition selectedSpace = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit.gameObject.CompareTag("Player"))
                {
                    selectedPiece = objectHit; 
                    Debug.Log($"Selected: {selectedPiece.GetComponent<GamePieceBase>()}");
                }
                if(objectHit.gameObject.CompareTag("Board"))
                {
                    selectedSpace = objectHit.GetComponent<Chess.Definitions.Tile>().position;
                    Debug.Log($"Selected Position: {selectedSpace}");
                }
            }
        }

        if(selectedPiece != null && selectedSpace != null) {
            uiController.activeAction = true;
        }
        else {
            uiController.activeAction = false;
        }
    }

    public void send_move_action() {
        Object.FindObjectOfType<Chess.BoardController>().execute_action(
            new Chess.Definitions.MoveAction(
                selectedPiece,
                selectedSpace
            )
        );

        selectedPiece = null;
        selectedSpace = null;
    }
}
