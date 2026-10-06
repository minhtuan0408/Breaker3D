using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EventNoParam onGameLose;
    [SerializeField] private EventNoParam onGameWin;

    [SerializeField] private Level level;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform ballInit;
    public Level currentLevel { get; private set; }
    public List<Ball> Balls = new List<Ball>();
    private void Awake()
    {
        currentLevel = Instantiate(level, transform);

        Ball currentBall = Instantiate(
            ball, ballInit.position, Quaternion.identity, transform);

        AddBall(currentBall);
    }

    private void OnEnable()
    {
        foreach (Ball currentBall in Balls)
            currentBall.OnOut += HandleBallOut;
    }

    private void OnDisable()
    {
        foreach (Ball currentBall in Balls)
            currentBall.OnOut -= HandleBallOut;
    }

    public void AddBall(Ball newBall)
    {
        Balls.Add(newBall);

        if (isActiveAndEnabled)
            newBall.OnOut += HandleBallOut;
    }

    public void InitBalls(int multiplier)
    {
        if (multiplier < 2)
            return;

        Ball[] currentBalls = Balls.ToArray();
        foreach (Ball sourceBall in currentBalls)
        {
            for (int i = 1; i < multiplier; i++)
            {
                float angle = i % 2 == 1 ? -15f * ((i + 1) / 2) : 15f * (i / 2);
                Ball newBall = Instantiate(ball, sourceBall.transform.position, Quaternion.identity, transform);
                newBall.m_MoveDirection = Quaternion.Euler(0f, angle, 0f) * sourceBall.m_MoveDirection;
                newBall.m_MoveDirection.Normalize();
                newBall.m_IsStoped = sourceBall.m_IsStoped;
                AddBall(newBall);
            }
        }
    }
    private void HandleBallOut(Ball ballThatFell)
    {
        ballThatFell.OnOut -= HandleBallOut;
        Balls.Remove(ballThatFell);

        if (Balls.Count == 0)
        {
            Debug.Log("Báo thua");
            onGameLose.Raise();
        }
    }
}
