using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Animator[] hearts;

    [SerializeField]
    private CharacterHealth playerHealthInfo;


    private void Awake()
    {
        playerHealthInfo.OnHealthChange += RefreshHeartContainers;
    }

    void RefreshHeartContainers()
    {
        if (playerHealthInfo.CurrentHealth >= 0 && playerHealthInfo.CurrentHealth < hearts.Length)
            hearts[playerHealthInfo.CurrentHealth].SetTrigger("LostHealth");
    }

}
