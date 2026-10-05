using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;

public class GameManager : BaseMono
{
    [SerializeField] private EventNoParam onGameWin;
    [SerializeField] private EventNoParam onGameLose;
    [SerializeField] private WinPanel winPanel;
    [SerializeField] private LosePanel losePanel;
    public GameState State;

    public override void OnEnable()
    {
        base.OnEnable();
        onGameWin.AddListener(WinResult);
        onGameLose.AddListener(LoseResult);
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }

    private void WinResult()
    {
        Debug.Log("Thắng");
    }
    private void LoseResult()
    {
        Debug.Log("Thua");
    }
}
[Serializable]
public enum GameState
{
    Playing,
    EndGame,
    Pause
}