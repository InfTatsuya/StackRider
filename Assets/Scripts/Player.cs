using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Joystick joystick;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Rigidbody rb;

    [SerializeField] GameObject playerVisual;
    [SerializeField] GameObject playerModel;
    [SerializeField] Animator anim;
    [SerializeField] Transform ballParent;
    [SerializeField] float ballOffset = 1f;
    [SerializeField] Ball ballPrefab;

    private Vector2 moveDirection;
    private List<Ball> collectedBall = new List<Ball>();

    private int ballCount;
    public int BallCount
    {
        get => ballCount;
        private set
        {
            ballCount = value;
            if(ballCount <= 0 && GameManager.Instance.CurrentGameState == GameManager.GameState.Playing)
            {
                GameManager.Instance.CurrentGameState = GameManager.GameState.Lose;
            }
        }
    }

    private bool isOnLava;
    private float meltBallDuration;
    private float lavaTimer;

    private WaitForSeconds countBallDelay = new WaitForSeconds(0.5f);

    private bool needAccelerated;
    private bool speedUp;
    private float timer;

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;

        moveDirection = joystick.Direction;

        rb.velocity = new Vector3(moveDirection.x * moveSpeed * 0.5f, rb.velocity.y, 1f * moveSpeed) ;

        if(isOnLava)
        {
            lavaTimer -= Time.deltaTime;
            playerVisual.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down, (meltBallDuration - lavaTimer) / meltBallDuration);

            if(lavaTimer < 0f)
            {
                lavaTimer = meltBallDuration;

                DestroyLowestBall();
            }
        }

        if (needAccelerated)
        {
            timer += Time.deltaTime;
            CalculateSpeed();
        }
    }

    private void CalculateSpeed()
    {
        if (speedUp)
        {
            moveSpeed = moveSpeed + 0.5f * timer;
        }
        else
        {
            moveSpeed = moveSpeed - 0.5f * timer;
        }

        moveSpeed = Mathf.Clamp(moveSpeed, 6f, 20f);
    }

    public void OnNewGame()
    {
        anim.SetTrigger(StringCollection.runAnim);

        moveDirection = Vector2.zero;

        moveSpeed = 10f;

        Ball newBall = Instantiate(ballPrefab);
        newBall.TriggerSpinAnim(true);
        AddBall(newBall);
    }


    private void ClearCollectedBall()
    {
        if(collectedBall.Count <= 0) return;

        foreach (var ball in collectedBall)
        {
            Destroy(ball.gameObject);
        }
        collectedBall.Clear();

        ballCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringCollection.ballTag))
        {
            Ball ball = other.GetComponent<Ball>();
            AddBall(ball);
        }
        else if (other.CompareTag(StringCollection.coinTag))
        {
            AddCoin();
        }
        else if (other.CompareTag(StringCollection.wallTag))
        {
            Wall wall = other.GetComponent<Wall>();
            if (!TryPassWall(wall))
            {
                GameManager.Instance.CurrentGameState = GameManager.GameState.Lose;
            }
        }
        else if (other.CompareTag(StringCollection.lavaTag))
        {
            EnterLava();
        }
        else if (other.CompareTag(StringCollection.desinationTag))
        {
            GameManager.Instance.CurrentGameState = GameManager.GameState.Win;
        }
        else if (other.CompareTag(StringCollection.stepUpTag))
        {
            needAccelerated = true;
            speedUp = false;
            timer = 0f;
        }
        else if (other.CompareTag(StringCollection.stepDownTag))
        {
            needAccelerated = true;
            speedUp = true;
            timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringCollection.lavaTag))
        {
            ExitLava();
        }
        else if (other.CompareTag(StringCollection.stepUpTag))
        {
            needAccelerated = false;
        }
        else if (other.CompareTag(StringCollection.stepDownTag))
        {
            needAccelerated = false;
            speedUp = true;
        }
    }

    private void EnterLava()
    {
        isOnLava = true;
        meltBallDuration = 3.14f / moveSpeed;
        lavaTimer = meltBallDuration;
    }

    private void ExitLava()
    {
        isOnLava = false;

        if(lavaTimer > 0f)
        {
            DestroyLowestBall();
        }
    }

    private void DestroyLowestBall()
    {
        if (collectedBall.Count <= 0) return;
        Destroy(collectedBall[collectedBall.Count - 1].gameObject);
        collectedBall.RemoveAt(collectedBall.Count - 1);
        BallCount--;

        playerVisual.transform.localPosition = Vector3.zero;
        UpdateBallStackVisual();
    }

    private bool TryPassWall(Wall newWall)
    {
        int amt = newWall.HeighStep;

        if(ballCount > amt)
        {
            BallCount -= amt;
            
            while(amt > 0)
            {
                collectedBall[collectedBall.Count - 1].transform.SetParent(null);
                Destroy(collectedBall[collectedBall.Count - 1].gameObject, 3f);
                collectedBall.RemoveAt(collectedBall.Count - 1);

                amt--;
            }
            UpdateBallStackVisual();

            if(newWall.IsPushPlayerUp)
            {
                rb.isKinematic = true;
                transform.position = new Vector3(transform.position.x, newWall.HeighStep + 0.5f, transform.position.z);
                rb.isKinematic = false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddCoin()
    {
        GameManager.Instance.AddCoin();
    }

    private void AddBall(Ball newBall)
    {
        BallCount++;
        collectedBall.Add(newBall);
        newBall.transform.SetParent(ballParent);
        newBall.transform.localPosition = Vector3.zero;
        newBall.OnBallCollected();

        UpdateBallStackVisual();

        UIManager.Instance.ShowScoreByCollectBall();
    }

    private void UpdateBallStackVisual()
    {
        for(int i = 0; i < collectedBall.Count; i++)
        {
            collectedBall[i].transform.localPosition = new Vector3(0f, ballOffset * (collectedBall.Count - i - 1), 0f);
        }

        playerModel.transform.localPosition = new Vector3(0f, ballOffset * ballCount, 0f);
    }

    public void OnWin()
    {
        anim.SetBool(StringCollection.winAnim, true);
        Debug.Log("win");
        rb.velocity = Vector3.zero;
        //GameManager.Instance.AddCoinWhenWin(BallCount);

        StartCoroutine(PlayWinCoroutine());
    }

    private IEnumerator PlayWinCoroutine()
    {
        yield return countBallDelay;

        int index = 1;
        while (BallCount > 0)
        {
            DestroyLowestBall();
            UIManager.Instance.ShowScoreByBall_Win(index);
            GameManager.Instance.AddCoin(index * 5);
            index++;
            yield return countBallDelay;
        }

    }

    public void OnLose()
    {
        anim.SetBool(StringCollection.loseAnim, true);
        rb.velocity = Vector3.zero;
    }

    public void ResetLevel()
    {
        ClearCollectedBall();
        anim.SetBool(StringCollection.winAnim, false);
        anim.SetBool(StringCollection.loseAnim, false);
        transform.position = Vector3.zero;
    }
}
