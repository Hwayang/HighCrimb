using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    //DB에서 파싱할 데이터
    uint maxHP = 0;
    uint maxStemina = 0;
    uint maxStarve = 0;
    uint maxWater = 0;
    uint maxMoveSpeed = 200;

    //객체에서 다룰 데이터
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
