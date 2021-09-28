using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region
    private static UIManager Instance = null;
    public static UIManager instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }
            return Instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public PlayerStatus playerStat;

    public Button staminaEnchantBtn; // ���� ��ȭ
    public Button strenthEnchantBtn; // ���� ��ȭ
    public Button eventBtn; // ���� �̺�Ʈ��
    public Button coinBtn; // �Ʒɸ��


    public Text eventBtnText;  
    public Text dateText; 
    public Text weightText;  // ������
    public Text staminaText; // ü��
    public Text strenthText; // �ٷ�
    public Text coinText;

    public Image eventImage;
    public Image staminaImage;

    public void StaminaEnchant(int value)
    {
        playerStat.StaminaUp(value);
        staminaText.text = "ü�� : " + playerStat.stamina;
    }

    public void StrenthEnchant(int value)
    {
        playerStat.StrengthUp(value);
        strenthText.text = "�ٷ� : " + playerStat.strength;
    }

    public void EventHappen() // ���� �Ÿ����� 
    {
        eventBtnText.text = "�̺�Ʈ�� ���� ����";
        eventImage.sprite = null; // ���⿡�� �̺�Ʈ �׸�
    }

    public void TouchCoin(int value)// ������ ��ġ�ؼ� ������
    {
        playerStat.coin += value;
        coinText.text = "�� : " + playerStat.coin;
    }

    IEnumerator UpdateForSec()
    {
        while(true)
        {
            dateText.text = DateTime.Now.ToString();
            staminaImage.fillAmount = Mathf.Clamp(playerStat.nowStamina/ playerStat.stamina, 0, 1);
            yield return GameManager.instance.sec1;
        }
    }

    

}
