using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Chess.Piece;
using UnityEngine.EventSystems;

using Chess.Piece;

public class Selector : MonoBehaviour
{
    public bool moveAllowed = true;
    public UIController uiController;

    private GamePieceBase selected = null;
    private GameObject selectedBoard = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                {
                    if (objectHit.gameObject.CompareTag("Player"))
                    {
                        if (selected)
                            selected.Deselect();

                        selected = objectHit.GetComponent<GamePieceBase>();
                        selected.Select();
                    }
                    if (moveAllowed && objectHit.gameObject.CompareTag("Board") && selected)
                    {
                        selectedBoard = objectHit.gameObject;
                        uiController.activeAction = true;
                    }
                }
            }
            else
            {
                if (selected)
                    selected.Deselect();
                selected = null;
                selectedBoard = null;
                uiController.activeAction = false;
            }

        }
    }

    public void _TempMovePiece()
    {
        if (moveAllowed)
        {
            selected.transform.position = selectedBoard.transform.position;
            if (selected)
                selected.Deselect();
            selected = null;
            selectedBoard = null;
        }
    }
}
