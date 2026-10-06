using UnityEngine;

public class BallBoosterItem : BaseItem
{
    [SerializeField, Min(2)] private int multiplier = 2;

    //protected override void Init()
    //{
    //    LevelManager levelManager = FindObjectOfType<LevelManager>();
    //    if (levelManager != null)
    //        levelManager.InitBalls(multiplier);
    //}
}
