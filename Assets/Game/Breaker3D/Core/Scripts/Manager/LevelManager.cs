using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Audio;
using VirtueSky.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EventNoParam onGameLose;
    [SerializeField] private EventNoParam onGameWin;
    [SerializeField] private EventNoParam onLevelClear;

    [SerializeField] private WinPanel winPanel;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private PlayMusicEvent playMusicEvent;
    [SerializeField] private SoundData soundTheme;

    [SerializeField] private Level level;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform ballInit;

    public Level currentLevel { get; private set; }
    public List<Ball> Balls = new List<Ball>();
    public GameState State { get; private set; } = GameState.Playing;

    private void OnEnable()
    {
        if (onGameWin != null)
            onGameWin.AddListener(HandleGameWin);
        if (onLevelClear != null)
            onLevelClear.AddListener(HandleGameWin);
        if (onGameLose != null)
            onGameLose.AddListener(HandleGameLose);

        foreach (Ball currentBall in Balls)
            currentBall.OnOut += HandleBallOut;
    }

    private void OnDisable()
    {
        if (onGameWin != null)
            onGameWin.RemoveListener(HandleGameWin);
        if (onLevelClear != null)
            onLevelClear.RemoveListener(HandleGameWin);
        if (onGameLose != null)
            onGameLose.RemoveListener(HandleGameLose);

        foreach (Ball currentBall in Balls)
            currentBall.OnOut -= HandleBallOut;

        Time.timeScale = 1f;
    }

    private void Awake()
    {
        State = GameState.Playing;
        Time.timeScale = 1f;

        if (winPanel != null)
            winPanel.gameObject.SetActive(false);
        if (losePanel != null)
            losePanel.gameObject.SetActive(false);

        currentLevel = Instantiate(level, transform);

        Ball currentBall = Instantiate(ball, ballInit.position, Quaternion.identity, transform);
        AddBall(currentBall);
    }

    private void Start()
    {
        if (playMusicEvent != null && soundTheme != null)
            playMusicEvent.Raise(soundTheme);
    }

    public void PauseGame()
    {
        if (State != GameState.Playing)
            return;

        State = GameState.Pause;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (State != GameState.Pause)
            return;

        State = GameState.Playing;
        Time.timeScale = 1f;
    }

    private void HandleGameWin()
    {
        if (State != GameState.Playing)
            return;

        State = GameState.EndGame;
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.gameObject.SetActive(true);
    }

    private void HandleGameLose()
    {
        if (State != GameState.Playing)
            return;

        State = GameState.EndGame;
        Time.timeScale = 0f;

        if (losePanel != null)
            losePanel.gameObject.SetActive(true);
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

        if (Balls.Count == 0 && onGameLose != null)
            onGameLose.Raise();
    }
}

public enum GameState
{
    Playing,
    EndGame,
    Pause
}
