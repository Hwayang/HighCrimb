using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public static PlayerState instance;
    BehaviorTree playerState;
    string currentState;

    private void Awake()
    {
        StateInit();

        //싱글톤 인스턴스 생성
        if (instance is not null)
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
        playerState = new BehaviorTree();

        //BT 루트 노드 하위에 Select 노드 생성
        playerState.root.children.Add(new ControlFlowNode("Selector"));

        //Select Node 하위에 각 상태 생성

        

        
    }

    public static PlayerState GetInstance()
    {
        if (instance is null)
        {
            instance = new PlayerState();
        }
        return instance;
    }
}
