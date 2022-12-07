using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    public int Hitpoints = 1;

    public static event Action<Brick> OnBrickDestruction;

    //private void Start()
    //{
    //    this.sr = this.GetComponent<SpriteRenderer>();
    //    this.sr.sprite = BricksManager.Instance.Sprites[this.Hitpoints - 1];
    //}

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }
    private void ApplyCollisionLogic(Ball ball)
    {
        Hitpoints--;
        if (Hitpoints <=0)
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            Destroy(this.gameObject);
        }
        else
        {
            sr.sprite = BricksManager.Instance.Sprites[Hitpoints - 1];
        }
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        transform.SetParent(containerTransform);
        sr.sprite = sprite;
        sr.color = color;
        Hitpoints = hitpoints;
    }
}
