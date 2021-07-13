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
        SceneManager.LoadScene(0);
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
    private void Update()
    {
        //curStamina -= 1.5f * Time.deltaTime;
        SetStaminaBar();
    }
}
