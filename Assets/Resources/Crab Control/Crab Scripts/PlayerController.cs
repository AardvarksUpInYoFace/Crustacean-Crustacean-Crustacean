using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DialogueEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float CrabSpeed = 1f;

    private Rigidbody Rb;

    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        myAnimator.Play("Crab_Idle_Real");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DialogueController.Instance.ConvoDone)
        {
            Movement();
        }
        
        else
        {
            Rb.velocity = Vector3.zero;
            if(!myAnimator.GetBool("IsIdle")) myAnimator.CrossFadeInFixedTime("Crab_Idle_Real", 0.2f);
        }
        
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

        if((Mathf.Abs(forward) + Mathf.Abs(side) == 0) && !myAnimator.GetBool("IsIdle")) myAnimator.CrossFadeInFixedTime("Crab_Idle_Real", 0.2f);
        if(Mathf.Abs(forward) + Mathf.Abs(side) != 0 && myAnimator.GetBool("IsIdle")) myAnimator.CrossFadeInFixedTime("Crab_Idle", 0.2f);
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
