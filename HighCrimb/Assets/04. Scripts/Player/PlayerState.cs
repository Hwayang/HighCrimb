using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    class State
    {
        public enum BasicState
        {
            Idle,
            Attack,
            Hit,
            Death
        }

        public enum MoveState
        {
            Walk,
            Run,
            BowWalk
        }

        public int currentState = (int)BasicState.Idle;
    }

    State state;

    private void Update()
    {
        state.currentState = (int)ChangeState();
    }

    public int GetCurrentState() { return (int)this.state.currentState; }

    private int ChangeState()
    {
        if(Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Horizontal") == 1)
        {
            if(Input.GetAxisRaw("Accel") == 1)
            {
                Debug.Log("State : Run");
                return (int)State.MoveState.Run; 
            }
            else if(Input.GetAxisRaw("Decel") == 1)
            {
                Debug.Log("State : BowWalk");
                return (int)State.MoveState.BowWalk;
            }

            Debug.Log("State : Walk");
            return (int)State.MoveState.Walk;
        }
        else if(Input.GetAxisRaw("Attack") == 1)
        {
            Debug.Log("State : Attack");
            return (int)State.BasicState.Attack;
        }
        else if(GetComponentInParent<PlayerInfo>().GetHP() < 0)
        {
            Debug.Log("State : Death");
            return (int)State.BasicState.Death;
        }
        else
        {
            Debug.Log("State : Idle");
            return (int)State.BasicState.Idle;
        }
    }
}
