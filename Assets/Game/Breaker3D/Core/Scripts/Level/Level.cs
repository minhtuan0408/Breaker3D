using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private EventNoParam onLevelClear;
    [SerializeField] private EventNoParam onLevelFail;

    [SerializeField] private List<BlockBase> blocks;
    //[SerializeField] private List<Ball> balls;

    private int aliveBlockCount;
    private int aliveBallCount;

    private bool isFinished;

    private void Awake()
    {
        aliveBlockCount = blocks.Count;
        //aliveBallCount = balls.Count;

        foreach (var block in blocks)
            block.OnBreak += HandleBlockDead;

        //foreach (var ball in balls)
        //    ball.OnOut += HandleBallDead;
    }

    private void HandleBlockDead()
    {
        aliveBlockCount--;

        if (aliveBlockCount <= 0)
            LevelClear();
    }

    private void HandleBallDead()
    {
        aliveBallCount--;

        if (aliveBallCount <= 0)
            LevelFail();
    }

    private void LevelClear()
    {
        if (isFinished)
            return;

        isFinished = true;
        onLevelClear.Raise();
    }

    private void LevelFail()
    {
        if (isFinished)
            return;

        isFinished = true;
        onLevelFail.Raise();
    }
}