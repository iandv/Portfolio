using System;

[Serializable]
public class GameManagerData
{
    public int winScore;
    public int score;

    public GameManagerData(GameManager gm)
    {
        winScore = gm.winScore;
        score = gm.score;
    }
}
