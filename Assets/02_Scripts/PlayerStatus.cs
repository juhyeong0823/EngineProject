using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float runDistance; // � �Ÿ�
    public int stamina; // ������
    public int strength; // �ٷ�
    public int weight;
    public DateTime date;
    public int coin;

    public int nowStamina;

    public void StaminaUp(int value)
    {
        stamina += value;
    }
    public void StrengthUp(int value)
    {
        strength += value;
    }
    public void WeightUp(int value)
    {
        weight += value;
    }

    public void StaminaDown(int value)
    {
        stamina -= value;
    }
    public void StrengthDown(int value)
    {
        strength -= value;
    }
    public void WeightDown(int value)
    {
        weight -= value;
    }
}
