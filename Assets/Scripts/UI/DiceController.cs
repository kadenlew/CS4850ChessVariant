using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public Transform dice;
    public Transform tumbler;
    public Animator animationController;

    // Degrees per second
    public float rollSpeedUpper = 2.0f;
    public float rollSpeedLower = 1.0f;

    // Target rotations
    private Vector3[] rollRotations = new Vector3[6] { new Vector3(-90, 0, 0),
                                                       new Vector3(0, -90, 90),
                                                       new Vector3(0, 0, 180),
                                                       new Vector3(0, 0, 0),
                                                       new Vector3(0, 0, -90),
                                                       new Vector3(90, 0, 0) };

    private float rollSpeed = 1f;
    private float rollAngle = 0f;

    public bool rolling { get; protected set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rolling)
        {
            if (rollAngle > 0)
            {
                rollAngle -= rollSpeed * 60 * Time.deltaTime;
                tumbler.localRotation = Quaternion.Euler(new Vector3(rollAngle, 0f, 0f));
            }
            else
            {
                tumbler.localRotation = Quaternion.Euler(Vector3.zero);
                rolling = false;
            }
        }
    }

    public void RollDice(int roll)
    {
        if(roll > 6 || roll < 1)
        {
            Debug.LogWarning("Dice cannot roll a " + roll.ToString());
            return;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(-180f, 180f), 0f));
        dice.localRotation = Quaternion.Euler(rollRotations[roll - 1]);
        tumbler.localRotation = Quaternion.Euler(Vector3.zero);
        rollAngle = 0f;
        rollSpeed = Random.Range(rollSpeedLower, rollSpeedUpper);
        rollAngle = rollSpeed * 60;
        rolling = true;
        animationController.SetTrigger("Roll");
    }
}
