using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    CharacterHealth characterInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent == null) return;

        if (((1 << collision.gameObject.layer) & characterInfo.enemyLayer.value) > 0)
        {
            CharacterHealth enemyHealth = collision.gameObject.transform.parent.GetComponent<CharacterHealth>();
            if(enemyHealth != null)
            {
                if(collision == enemyHealth.myHitbox)
                {
                    collision.gameObject.transform.parent.GetComponent<CharacterHealth>().GetAttacked(transform.parent.position);
                }
            }
        }
    }

}
