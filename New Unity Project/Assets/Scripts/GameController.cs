using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const int INIT_PLAYER_HP = 10;

    private static Vector3 finish;
    private static int playerHealth;
    private static TextMeshProUGUI hpText;
    private GameObject startButton;
    private TextMeshProUGUI turnText;
    private UIController uIController;
    private BuildingController buildingController;
    private int turn = 0;
    private Respawn respawn;
    private int money = 50;
    private bool gameOver = false;
    
    public static Vector3 Finish { get => finish; set => finish = value; }
    public static int PlayerHealth
    {
        get => playerHealth;

        set
        {
            playerHealth = value;
            hpText.text = playerHealth.ToString();
        }
    }

    public int Money
    {
        get => money;
        set
        {
            money = value;
            uIController.SetMoneyText(value);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        playerHealth = INIT_PLAYER_HP;

        respawn = FindObjectOfType<Respawn>();
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Transform>().position;

        hpText = GameObject.Find("HPText").GetComponent<TextMeshProUGUI>();
        hpText.text = playerHealth.ToString();

        turnText = GameObject.Find("TurnText").GetComponent<TextMeshProUGUI>();
        turnText.text = "Turn " + turn;

        buildingController = FindObjectOfType<BuildingController>();
        uIController = FindObjectOfType<UIController>();
        startButton = GameObject.Find("StartButton");

        uIController.SetMoneyText(money);
    }

    private void Update()
    {
        HandleInput();

        if (PlayerHealth <= 0 && !gameOver) GameOver();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                GameObject selectedObject = hit.transform.gameObject;
                if (selectedObject.CompareTag("BuildArea") && !EventSystem.current.IsPointerOverGameObject())
                {
                    BuildArea buildArea = selectedObject.GetComponent<BuildArea>();
                    buildingController.BuildArea = buildArea;

                    if(buildArea.Occupied)
                    {
                        uIController.ToggleDestroyMenu();
                    }
                    else
                    {
                        uIController.ToggleBuildingsMenu();
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        turn++;
        turnText.text = "Turn " + turn;

        respawn.CubesNumber = turn;
        respawn.SmallCubesNumber = turn;

        if (turn > 3)
        {
            respawn.RespawnTime = 0.5f;
            respawn.BigCubesNumber = turn - 3;
        }
        if (turn > 7)
        {
            respawn.RespawnTime = 0.25f;
            respawn.BigCubesNumber = turn - 7;
            respawn.BossCubesNumber = turn - 7;
        }
        if (turn > 13)
        {
            respawn.CubesNumber = turn - 7;
            respawn.SmallCubesNumber = turn - 7;
            respawn.BigCubesNumber = turn - 13;
            respawn.BossCubesNumber = turn - 13;
            respawn.SmallBossCubesNumber = turn - 13;
        }
        if (turn > 17)
        {
            respawn.CubesNumber = turn - 13;
            respawn.SmallCubesNumber = turn - 13;
            respawn.BigCubesNumber = turn - 17;
            respawn.BossCubesNumber = turn - 17;
            respawn.SmallBossCubesNumber = turn - 17;
            respawn.SplitEnemyNumber = turn - 17;
        }
        if(turn > 23)
        {
            respawn.CubesNumber = turn - 17;
            respawn.SmallCubesNumber = turn - 17;
            respawn.BigCubesNumber = turn - 23;
            respawn.BossCubesNumber = turn - 23;
            respawn.SmallBossCubesNumber = turn - 23;
            respawn.SplitEnemyNumber = turn - 23;
            respawn.BigSplitEnemyNumber = turn - 23;
        }
        respawn.StartTurn();
    }

    public void NextTurn()
    {
        if(!gameOver) startButton.SetActive(true);
    }

    public void GameOver()
    {
        gameOver = true;
        startButton.SetActive(false);
        uIController.ShowLostMenu();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
