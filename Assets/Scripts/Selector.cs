using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{

    private GamePieceBase selected = null;

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
                }
            }
            else
            {
                if (selected)
                    selected.Deselect();
                selected = null;
            }
        }
    }
}
