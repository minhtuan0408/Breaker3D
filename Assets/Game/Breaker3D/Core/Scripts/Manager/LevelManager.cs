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
    public Ball currentBall;
    private void Awake()
    {
        currentLevel = Instantiate(level, transform);
        currentBall = Instantiate(ball, ballInit.position, Quaternion.identity,transform);
    }

    private void Start()
    {
        currentBall.OnOut += HandleBallDead;
    }
    private void OnEnable()
    {
        currentBall.OnOut -= HandleBallDead;
    }
    private void HandleBallDead()
    {
        Debug.Log("Báo thua");
        onGameLose.Raise();
    }
}
