using System;
using System.IO;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultLeftClamp = -7.53f;
    private float defaultRightClamp = 7.56f;
    private float speed = 0.04f;
    private Vector2 moveDelta;
    private SpriteRenderer sr;
    [SerializeField] private GameObject PauseScreen;
    private void Start()
    {
        Load.LoadGame();
        transform.position = Save.data.PaddlePosition;
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = transform.position.y;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        moveDelta.x = Input.GetAxisRaw("Horizontal");
        PaddleMovement();

    }
    // ?????????? ????????? ????????? ??? ?????? ?? ????
    private void OnApplicationQuit()
    {
        Save.data.PaddlePosition = transform.position;
        Save.SaveGame();
    }
    // ???????? ????????? ? ??????? ??????
    private void PaddleMovement()
    {
        float leftClamp = defaultLeftClamp;
        float rightClamp = defaultRightClamp;

        var position = transform.position;
        switch (moveDelta.x)
        {
            case > 0:
                position.x += speed;
                break;
            case < 0:
                position.x -= speed;
                break;
        }

        float mousePositionPixels = Mathf.Clamp(position.x, leftClamp, rightClamp);
        if (PauseScreen.activeSelf)
            transform.position = new Vector3(transform.position.x, paddleInitialY, 0);
        else
            transform.position = new Vector3(mousePositionPixels, paddleInitialY, 0);
    }
    //????????? ???????? ?????? ??? ??????? ?? ?????? ????? ?????????
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 300)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 300)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
