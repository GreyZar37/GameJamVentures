[System.Serializable]
public class Gambler
{
    public int health;
    public int points;
    public int turnsPlayed;
    public bool isFinished;

    public Gambler(int startHealth)
    {
        health = startHealth;
        points = 0;
        turnsPlayed = 0;
        isFinished = false;
    }
}