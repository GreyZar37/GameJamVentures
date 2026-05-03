[System.Serializable]
public class Gambler
{
    public int health;
    public int points;
    public int turnsPlayed;
    public bool isSubtracting;
    public bool isFinished;
    public GamblerType gamblerType;

    public Gambler(int startHealth, GamblerType gamblerType)
    {
        health = startHealth;
        points = 0;
        turnsPlayed = 0;
        isSubtracting = false;
        isFinished = false;
        this.gamblerType = gamblerType;
    }

    public void ResetRound()
    {
        points = 0;
        turnsPlayed = 0;
        isFinished = false;
        isSubtracting = false;
    }
}

public enum GamblerType
{
    Player, Opponent
}