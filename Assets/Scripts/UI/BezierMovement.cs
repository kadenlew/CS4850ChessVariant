using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMovement : MonoBehaviour
{
    public Transform startTransform, endTransform;
    public Transform firstPoint, secondPoint, thirdPoint, fourthPoint;
    private float movingTime;

    public bool animating { get; protected set; } = false;
    private GameObject animationTarget;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animating)
        {
            currentTime += Time.deltaTime;
            animationTarget.transform.position = CalculateQuadBezierCurve(currentTime / movingTime);
            if (currentTime >= movingTime)
                animating = false;
        }
    }

    public Vector3 CalculateQuadBezierCurve(float t)
    {
        Vector3 np = Mathf.Pow((1 - t), 3) * firstPoint.position +
            3 * t * Mathf.Pow((1 - t), 2) * secondPoint.position +
            3 * Mathf.Pow(t, 2) * (1 - t) * thirdPoint.position +
            Mathf.Pow(t, 3) * fourthPoint.position;
        return np;
    }

    public void ConfigureBezier(Vector3 startPosition, Vector3 endPosition)
    {
        startTransform.position = startPosition;
        endTransform.position = endPosition;
        movingTime = Mathf.Log((Vector3.Distance(startPosition, endPosition)) + 1.0f)/4;
    }

    public void Animate(GameObject piece)
    {
        animationTarget = piece;
        currentTime = 0f;
        animating = true;
    }


}
