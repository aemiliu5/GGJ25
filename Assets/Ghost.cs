using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed;
    public float fadeOutAfter;
    public float timer;
    public SpriteRenderer sr;

    private float alpha;
    boo

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        float alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(0, speed, 0);

    }

    private void FadeOut()
    {
        float alpha = 1;
        sr.color = new Color(1,1,1, alpha -)
    }
}
