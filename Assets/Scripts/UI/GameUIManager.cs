using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private Animator[] hearts;

    [SerializeField]
    private Animator coin;

    [SerializeField]
    private Text coinAmountTxt;

    [SerializeField]
    private CharacterHealth playerHealthInfo;

    [SerializeField]
    private CoinCollecting coinCollecting;

    private void Awake()
    {
        playerHealthInfo.OnHealthChange += RefreshHeartContainers;
        coinCollecting.OnCoinChange += RefreshCoinAmount;
    }
    
    void RefreshHeartContainers()
    {
        if (playerHealthInfo.CurrentHealth >= 0 && playerHealthInfo.CurrentHealth < hearts.Length)
            hearts[playerHealthInfo.CurrentHealth].SetTrigger("LostHealth");
    }

    void RefreshCoinAmount()
    {
        coin.SetTrigger("gotNewCoin");
        coinAmountTxt.text = "x " + coinCollecting.CoinAmount;
    }

}
