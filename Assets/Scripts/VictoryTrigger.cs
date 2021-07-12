using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField]
    PauseRestartUIManager gameUI;

    [SerializeField]
    LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameUI.ShowVictoryPanel();
    }


}
