using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GamblingManager2 : Singleton<GamblingManager2>
{
    [Header("Gambling Properties")]
    [field: SerializeField] public GameState GameState {  get; private set; }
    [field: SerializeField] public Gambler Player {  get; private set; }
    [field: SerializeField] public Gambler Opponent { get; private set; }
    [SerializeField] private int targetScore = 10;
    [SerializeField] private int maxRounds = 3;


    [Header("Other Properties")]
    [SerializeField] private SubtractionSelectionManager subtractionManager;
    [SerializeField] private Animator bellAnimator;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private GameObject dicePoolPrefab;
    [SerializeField] private Vector3[] dicePositions = new Vector3[2];

    private TMP_Text scoreText;
    private DicePool currentPool;

    public event Action OnGamblingStart;
    public event Action OnGamblingEnd;
    public event Action<GamblerType> OnRoundFinished; //GamblerType represents who won that round

    private void Awake()
    {
        scoreText = scoreTextAnimator.GetComponent<TMP_Text>();
    }

    public void SetupForGambling()
    {
        bellAnimator.SetBool("isAvailable", true);
    }

    public void StartGambling()
    {
        Player = new Gambler(PlayerManager.Instance.PlayerHealth, GamblerType.Player);
        Opponent = new Gambler(1, GamblerType.Opponent); //Change argument (health) based on opponent difficulty!

        GameState = (GameState)Random.Range(0, 2); //Randomize who starts
        ChangeGamblerTurn(GameState);
        OnGamblingStart?.Invoke();
    }

    private void ChangeGamblerTurn(GameState turn)
    {
        GameState = turn;
        if (GameState == GameState.PLAYER_TURN) StartCoroutine(RunGamblerTurn(Player));
        else if (GameState == GameState.OPPONENT_TURN) StartCoroutine(RunGamblerTurn(Opponent));
    }

    private IEnumerator RunGamblerTurn(Gambler gambler)
    {
        yield return new WaitForSeconds(2f);
        if (currentPool != null) Destroy(currentPool.gameObject);
        //If not player's first turn, select a subtraction selection:
        if(gambler.turnsPlayed > 0 && gambler.gamblerType == GamblerType.Player)
        {
            subtractionManager.SetupSelection((selection) =>
            {
                gambler.isSubtracting = selection == SubtractionSelection.Subtract;
                if (selection == SubtractionSelection.Stop)
                {
                    gambler.isFinished = true;
                    if(!CheckIfGamblersAreFinished())
                    {
                        ChangeGamblerTurn(GameState == GameState.PLAYER_TURN ? GameState.OPPONENT_TURN : GameState.PLAYER_TURN);
                    }
                    return; //Player stopped, don't roll.
                }
                RollDices();
            });
            yield break; //This returns makes sures player selects a choice before continue to roll a dice
        }
        else if(gambler.turnsPlayed > 0 && gambler.gamblerType == GamblerType.Opponent) //If not opponent's first turn, opponent calculates a subtraction choice
        {
            SubtractionSelection desiredSelection = CalculateOpponentChoice();
            gambler.isSubtracting = desiredSelection == SubtractionSelection.Subtract;
            if(desiredSelection == SubtractionSelection.Stop)
            {
                gambler.isFinished = true;
                if (!CheckIfGamblersAreFinished())
                {
                    ChangeGamblerTurn(GameState == GameState.PLAYER_TURN ? GameState.OPPONENT_TURN : GameState.PLAYER_TURN);
                }
                yield break; //Opponent stopped, don't roll.
            }
        }

        //Calls local function RollDices():
        RollDices();

        void RollDices()
        {
            gambler.turnsPlayed++;
            currentPool = Instantiate(dicePoolPrefab, dicePositions[gambler.gamblerType == GamblerType.Player ? 0 : 1], Quaternion.identity).GetComponent<DicePool>();
            currentPool.OnRollDone += OnDiceRollDone;
            //Local function is called when dices are finished rolling:
            void OnDiceRollDone(int sum)
            {
                gambler.points += gambler.isSubtracting ? -sum : sum;
                scoreText.text = $"{sum} !";
                scoreTextAnimator.SetTrigger("Show Score");

                if (gambler.turnsPlayed >= maxRounds)
                {
                    gambler.isFinished = true;
                    bool finished = CheckIfGamblersAreFinished();
                    if (finished) return;
                }
                ChangeGamblerTurn(GameState == GameState.PLAYER_TURN ? GameState.OPPONENT_TURN : GameState.PLAYER_TURN);
            }
        }
    }

    private bool CheckIfGamblersAreFinished()
    {
        bool allFinished = Player.isFinished && Opponent.isFinished;
        if (allFinished) StartCoroutine(FinishRound());
        return allFinished;
    }

    private IEnumerator FinishRound()
    {
        yield return new WaitForSeconds(2);
        int playerDiff = Mathf.Abs(Player.points - targetScore);
        int opponentDiff = Mathf.Abs(Opponent.points - targetScore);

        if (playerDiff >= opponentDiff) // Player Loses
        {
            Player.health--;
            PlayerManager.Instance.SetPlayerHealth(Player.health);
            if (Player.health <= 0)
            {
                EndGambling(GameState.LOSE); yield break;
            }
            OnRoundFinished?.Invoke(GamblerType.Opponent);
        }
        else // Player Wins
        {
            Opponent.health--;
            if (Opponent.health <= 0)
            {
                EndGambling(GameState.WIN); yield break;
            }
            OnRoundFinished?.Invoke(GamblerType.Player);
        }

        Player.ResetRound();
        Opponent.ResetRound();
        ChangeGamblerTurn(GameState == GameState.PLAYER_TURN ? GameState.OPPONENT_TURN : GameState.PLAYER_TURN);
    }

    private void EndGambling(GameState endState)
    {
        GameState = endState;
        if(GameState == GameState.WIN)
        {
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.Sitting);
        }
        else if(GameState == GameState.LOSE)
        {
            //Play Death Animation Falling to The Ground And Reset Game
            SceneManager.LoadScene(0);
        }
        OnGamblingEnd?.Invoke();
    }

    private SubtractionSelection CalculateOpponentChoice()
    {
        if (Mathf.Abs(Opponent.points - targetScore) <= 3) return SubtractionSelection.Stop;
        else if (Opponent.points > targetScore) return SubtractionSelection.Subtract;
        else return SubtractionSelection.Add;
    }
}

public enum GameState
{
    PLAYER_TURN, OPPONENT_TURN, WIN, LOSE
}