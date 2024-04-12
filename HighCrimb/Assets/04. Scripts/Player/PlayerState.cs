using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState instance = null;

    public Dictionary<string, string[]> states;
    string currentState;

    private void Awake()
    {
        StateInit();

        //ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫ ª˝º∫
        if(null != instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void StateInit()
    {
        string[] moveStates = new string[] { "BowWalk", "Run", "Walk" };
        string[] idleStates = new string[] { "Idle" };
        string[] attackStates = new string[] { "Attack" };
        string[] deathStates = new string[] { "Death" };
        string[] hitStates = new string[] { "Hit" };


        states.Add("Move", moveStates);
    }

    public static PlayerState GetInstance()
    {
        if(instance == null)
        {
            instance = new PlayerState();
        }
        return instance;
    }

    private string ChangeState(string message)
    {
        
    }

    public string GetCurrentState(string message)
    {
        currentState = ChangeState(message);
        return currentState;
    }
}
