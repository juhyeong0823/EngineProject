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

    public Button staminaEnchantBtn; // 스텟 강화
    public Button strenthEnchantBtn; // 스텟 강화
    public Button eventBtn; // 각종 이벤트들
    public Button coinBtn; // 아령모습


    public Text eventBtnText;  
    public Text dateText; 
    public Text weightText;  // 몸무게
    public Text staminaText; // 체력
    public Text strenthText; // 근력
    public Text coinText;

    public Image eventImage;
    public Image staminaImage;

    public void StaminaEnchant(int value)
    {
        playerStat.StaminaUp(value);
        staminaText.text = "체력 : " + playerStat.stamina;
    }

    public void StrenthEnchant(int value)
    {
        playerStat.StrengthUp(value);
        strenthText.text = "근력 : " + playerStat.strength;
    }

    public void EventHappen() // 일정 거리마다 
    {
        eventBtnText.text = "이벤트에 대한 설명";
        eventImage.sprite = null; // 여기에는 이벤트 그림
    }

    public void TouchCoin(int value)// 코인을 터치해서 먹으며
    {
        playerStat.coin += value;
        coinText.text = "돈 : " + playerStat.coin;
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
