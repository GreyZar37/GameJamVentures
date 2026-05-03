using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private PlayDice playDice;
    [SerializeField] private Animator bellAnimator;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private Animator gamblingViewPlayerAnimator;
    [SerializeField] private TMP_Text playerScoreText;
    [SerializeField] private TMP_Text opponentScoreText;
    [SerializeField] private Image arrowImage;
    [SerializeField] private GameObject dicePoolPrefab;
    [SerializeField] private Transform[] dicePositions;
    [SerializeField] private GameObject[] playerGoblets;
    [SerializeField] private GameObject[] opponentsGoblets;

    private TMP_Text scoreText;
    private DicePool currentPool;

    public event Action OnGamblingStart;
    public event Action OnGamblingEnd;
    public event Action<GamblerType> OnRoundFinished; //GamblerType represents who won that round

    private void Awake()
    {
        scoreText = scoreTextAnimator.GetComponent<TMP_Text>();
        HideGoblets();
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

        UpdateScoreText();
        UpdateGobletsHealth();

        OnGamblingStart?.Invoke();
    }

    private void ChangeGamblerTurn(GameState turn)
    {
        GameState = turn;
        if (GameState == GameState.PLAYER_TURN && !Player.isFinished)
        {
            StartCoroutine(RunGamblerTurn(Player));
        }
        else if (GameState == GameState.OPPONENT_TURN && !Opponent.isFinished)
        {
            StartCoroutine(RunGamblerTurn(Opponent));
        }
        else if (GameState == GameState.OPPONENT_TURN && Opponent.isFinished && !Player.isFinished)
        {
            ChangeGamblerTurn(GameState.PLAYER_TURN);
            return;
        }
        else if (GameState == GameState.PLAYER_TURN && Player.isFinished && !Opponent.isFinished)
        {
            ChangeGamblerTurn(GameState.OPPONENT_TURN);
            return;
        }
        else
        {
            CheckIfGamblersAreFinished();
        }

        Quaternion targetRot = turn == GameState.PLAYER_TURN ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.identity;
        StartCoroutine(SmoothlyRotateArrow(targetRot));

        IEnumerator SmoothlyRotateArrow(Quaternion targetRot)
        {
            float progress = 0f;
            while(progress < 1f)
            {
                arrowImage.rectTransform.rotation = Quaternion.Lerp(arrowImage.rectTransform.rotation, targetRot, progress);
                progress += Time.deltaTime / 4f;
                yield return null;
            }
            arrowImage.rectTransform.rotation = targetRot;
        }
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
        else if(gambler.turnsPlayed == 0 && gambler.gamblerType == GamblerType.Player)
        {
            Debug.Log("Players first turn, show dice");
            playDice.ShowDice(() =>
            {
                RollDices();
            });
            yield break;
        }


        //Calls local function RollDices():
        Debug.Log("Enemy's First Turn, Just roll dice");
        RollDices();

        void RollDices()
        {
            gambler.turnsPlayed++;
            currentPool = Instantiate(dicePoolPrefab, dicePositions[gambler.gamblerType == GamblerType.Player ? 0 : 1].position, Quaternion.identity).GetComponent<DicePool>();
            currentPool.OnRollDone += OnDiceRollDone;
            //Local function is called when dices are finished rolling:
            void OnDiceRollDone(int sum)
            {
                gambler.points += gambler.isSubtracting ? -sum : sum;
                if (gambler.points < 0) gambler.points = 0;

                scoreText.text = $"{sum} !";
                scoreTextAnimator.SetTrigger("Show Score");
                UpdateScoreText();

                if (gambler.points == targetScore)
                {
                    gambler.isFinished = true;
                    bool finished = CheckIfGamblersAreFinished();
                    if (finished) return;
                }
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
            gamblingViewPlayerAnimator.SetTrigger("Drink");
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

        UpdateGobletsHealth();

        Player.ResetRound();
        Opponent.ResetRound();
        UpdateScoreText();
        ChangeGamblerTurn(GameState == GameState.PLAYER_TURN ? GameState.OPPONENT_TURN : GameState.PLAYER_TURN);
    }

    private void EndGambling(GameState endState)
    {
        GameState = endState;
        if(GameState == GameState.WIN)
        {
            Debug.Log("Congratulations!! You won!!");
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.Sitting);
            TableMovement.Instance.currentRoom.enemyBeaten = true;
        }
        else if(GameState == GameState.LOSE)
        {
            PlayerControllerManager.Instance.ChangePlayerController(PlayerControllerType.Dying);
            Debug.Log("Sorry to break it to you, but you lost");
        }
        HideGoblets();
        OnGamblingEnd?.Invoke();
        OnGamblingEnd -= OnGamblingEnd;
    }

    private SubtractionSelection CalculateOpponentChoice()
    {
        if (Mathf.Abs(Opponent.points - targetScore) <= 3) return SubtractionSelection.Stop;
        else if (Opponent.points > targetScore) return SubtractionSelection.Subtract;
        else return SubtractionSelection.Add;
    }

    private void UpdateScoreText()
    {
        playerScoreText.text = $"{Player.points}/{targetScore}";
        opponentScoreText.text = $"{Opponent.points}/{targetScore}";
    }

    private void UpdateGobletsHealth()
    {
        HideGoblets();
        for (int i = 0; i < Player.health; i++)
        {
            playerGoblets[i].SetActive(true);
        }
        for (int i = 0; i < Opponent.health; i++)
        {
            opponentsGoblets[i].SetActive(true);
        }
    }
    private void HideGoblets()
    {
        foreach (GameObject goblet in playerGoblets) goblet.SetActive(false);
        foreach (GameObject goblet in opponentsGoblets) goblet.SetActive(false);
    }
}

public enum GameState
{
    PLAYER_TURN, OPPONENT_TURN, WIN, LOSE
}