using System;
using DG.Tweening;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed;
    public float fadeOutAfter;
    public float timer;
    public SpriteRenderer sr;

    public bool fadeCalled;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.DOFade(1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(0, speed, 0);

        if (timer > fadeOutAfter && !fadeCalled)
        {
            sr.DOFade(0f, 1f);
            fadeCalled = true;
        }
    }
}
