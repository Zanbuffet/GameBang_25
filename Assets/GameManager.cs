using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public float maxStamina = 10;
    public float curStamina = 10;
    public GameObject staminaBar = null;
    public GameState gameState;
    public GameObject failPanel;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void GameOver()
    {
        failPanel.SetActive(true);
        gameState = GameState.Dead;
        //SceneManager.LoadScene(0);
    }

    public void SetStaminaBar()
    {
        if (staminaBar == null)
        {
            staminaBar = GameObject.FindGameObjectWithTag("Stamina");
        }
        if (curStamina >= maxStamina + 1)
        {
            curStamina = maxStamina + 1;
        }
        staminaBar.GetComponent<Image>().fillAmount = curStamina / maxStamina;
        staminaBar.GetComponent<Image>().color = new Color((230 / 255f), ((curStamina / maxStamina)), (245 / 255f), (255 / 255f));
        if (curStamina <= 0)
        {
            GameOver();
        }
    }
    public void ReturnTittle()
    {
        SceneManager.LoadScene(0);
    }
    public void NewGame()
    {
        //temp change to scene without opening
        SceneManager.LoadScene(2);
    }

    public void WinScene()
    {
        SceneManager.LoadScene(3);
    }
    private void Update()
    {
        if (gameState == GameState.Game)
        {
            curStamina -= Time.deltaTime;
            SetStaminaBar();
        }
    }
}
public enum GameState
{
    Tittle,
    Cg,
    Game,
    Dead,
    Win
}