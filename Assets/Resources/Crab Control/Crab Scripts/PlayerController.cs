using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float CrabSpeed = 1f;

    private Rigidbody Rb;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }



    private void Movement()
    {
        //checking forwards and backwards.
        int forward = 0;
        int side = 0;

        forward += CheckInputHeld(new KeyCode[] { KeyCode.W, KeyCode.UpArrow });
        forward -= CheckInputHeld(new KeyCode[] { KeyCode.S, KeyCode.DownArrow });

        side += CheckInputHeld(new KeyCode[] { KeyCode.D, KeyCode.RightArrow });
        side -= CheckInputHeld(new KeyCode[] { KeyCode.A, KeyCode.LeftArrow });

        float diagonalOperator = 1;
        if (Mathf.Abs(forward) + Mathf.Abs(side) > 1) diagonalOperator = 0.707f;

        //GetComponent<Rigid.localPosition += new Vector3(CrabSpeed * diagonalOperator * side, 0, CrabSpeed * diagonalOperator * forward);

        Rb.velocity = new Vector3(CrabSpeed * diagonalOperator * side, 0, CrabSpeed * diagonalOperator * forward);
    }

    private int CheckInputHeld(KeyCode[] keycodes)
    {
        foreach(KeyCode code in keycodes)
        {
            if(Input.GetKey(code))
            {
                return 1;
            }
        }

        return 0;
    }
}
