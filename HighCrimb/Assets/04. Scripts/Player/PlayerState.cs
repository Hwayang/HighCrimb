using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    public static PlayerState instance = null;
    BehaviorTree playerState = null;
    string currentState;

    private void Awake()
    {
        StateInit();

        //�̱��� �ν��Ͻ� ����
        if (null != instance)
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

        //BT ��Ʈ ��� ������ Select ��� ����
        playerState.root.children.Add(new ControlFlowNode("Selector"));

        //Select Node ������ �� ���� ����

        

        
    }

    public static PlayerState GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerState();
        }
        return instance;
    }
}
