using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == null) return;
        if (collision.GetComponent<BoxCollider2D>() == collision) return;

        gameObject.GetComponent<Collider2D>().enabled = false;
        collision.gameObject.transform.parent.GetComponent<CoinCollecting>().CollectedCoin();
        GetComponent<Animator>().SetTrigger("isCollected");
    }
}
