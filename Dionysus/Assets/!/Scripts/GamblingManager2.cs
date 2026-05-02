using UnityEngine;

public class GamblingManager2 : MonoBehaviour
{
    [field: SerializeField] public GameState GameState {  get; private set; }
    [field: SerializeField] public Gambler Player {  get; private set; }
    [field: SerializeField] public Gambler Opponent { get; private set; }

    public void StartGambling()
    {
        Player = new Gambler(3);
        Opponent = new Gambler(1);

        GameState = (GameState)Random.Range(0, 2); //Randomize who starts
        if (GameState == GameState.PLAYER_TURN) RunGamblerTurn(Player);
        else if(GameState == GameState.OPPONENT_TURN) RunGamblerTurn(Opponent);
    }

    private void RunGamblerTurn(Gambler gambler)
    {
        gambler.turnsPlayed++;
    }
}

public enum GameState
{
    PLAYER_TURN, OPPONENT_TURN, WIN, LOSE
}