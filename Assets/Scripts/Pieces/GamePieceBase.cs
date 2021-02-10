using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceBase : MonoBehaviour
{
    public Material standard;
    public Material selected;

    private Renderer targetRenderer;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
        Deselect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        targetRenderer.material = selected;
    }

    public void Deselect()
    {
        targetRenderer.material = standard;
    }
}
