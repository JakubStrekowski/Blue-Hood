using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutOnCollision : MonoBehaviour
{
    [SerializeField]
    private LayerMask layertToTrigger;

    SpriteRenderer _sr;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("FadeOut");
    }

    public float fadeSpeed = .003f;
    float lerpAmount;


    IEnumerator FadeOut()
    {
        lerpAmount = 0;

        while (lerpAmount < 1)
        {
            Color c = _sr.color;
            c.a = Mathf.Lerp(1, 0, lerpAmount);
            _sr.color = c;
            lerpAmount += fadeSpeed;

            yield return null;
        }
    }
}
