using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    //DB���� �Ľ��� ������
    uint maxHP = 0;
    uint maxStemina = 0;
    uint maxStarve = 0;
    uint maxWater = 0;
    uint maxMoveSpeed = 200;

    //��ü���� �ٷ� ������
    int HP = 0;
    int stemina = 0;
    int starve = 0;
    int waterValue = 0;
    int moveSpeed = 0;

    private void Awake()
    {
        HP = (int)maxHP;
        stemina = (int)maxStemina;
        starve = (int)maxStarve;
        waterValue = (int)maxWater;
        moveSpeed = (int)maxMoveSpeed;
    }

    public int GetHP() { return HP; }
    public int GetStemina() { return stemina; }
    public int GetStarve() { return starve; }
    public int GetWater() { return waterValue;}
    public int GetMoveSpeed() { return moveSpeed;}

}
