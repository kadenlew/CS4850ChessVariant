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
    private bool bezierMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(startTransform.position, endTransform.position) < 0.001)
            animating = false;
        if(animating && bezierMove)
        {
            currentTime += Time.deltaTime;
            animationTarget.transform.position = CalculateQuadBezierCurve(currentTime / movingTime);
            if (currentTime >= movingTime)
                animating = false;
        }
        if(animating && !bezierMove)
        {
            currentTime += Time.deltaTime;
            if(currentTime <= movingTime/2)
            {
                animationTarget.transform.position = (startTransform.position + (endTransform.position - startTransform.position) * ((currentTime / (movingTime / 2))));
            }
            else
            {
                animationTarget.transform.position = (startTransform.position + (endTransform.position - startTransform.position) * (((movingTime - currentTime) / (movingTime / 2))));
            }
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

    public void AnimateFailedAttack(GameObject piece, Vector3 bumpTowards, float distance)
    {
        bezierMove = false;
        startTransform.position = piece.transform.position;
        endTransform.position = Vector3.MoveTowards(startTransform.position, bumpTowards, distance);
        movingTime = 0.4f;
        currentTime = 0f;
        animationTarget = piece;
        animating = true;
    }

    public void ConfigureBezier(Vector3 startPosition, Vector3 endPosition)
    {
        startTransform.position = startPosition;
        endTransform.position = endPosition;
        movingTime = Mathf.Log((Vector3.Distance(startPosition, endPosition)) + 1.0f)/4;
    }

    public void Animate(GameObject piece)
    {
        bezierMove = true;
        animationTarget = piece;
        currentTime = 0f;
        animating = true;
    }


}
