using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverUI : MonoBehaviour
{
    public Vector3 target;
    public Transform dynamic;
    public bool adapt = false;
    public float offset = 0f;

    // Update is called once per frame
    void Update()
    {
        if (adapt)
        {
            if(dynamic)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(dynamic.position);
                transform.position = screenPos + new Vector3(0, offset, 0);
            }
            else
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(target);
                transform.position = screenPos + new Vector3(0, offset, 0);
            }  
        }
    }

    public void SetText(string newText)
    {
        gameObject.GetComponent<Text>().text = newText;
    }

    public void SetColor(Color newColor)
    {
        gameObject.GetComponent<Text>().color = newColor;
    }
}
