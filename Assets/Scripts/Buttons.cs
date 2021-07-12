using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] ButtonController buttonController;
    [SerializeField] Animator animator;
    [SerializeField] int index;
    private bool changingScene;
    private float timer;
    [SerializeField] GameObject changingCover;

    // Update is called once per frame
    void Update()
    {
        if (buttonController.cur_index == index)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1)
            {
                animator.SetBool("press", true);
            }
            else if (animator.GetBool("press"))
            {
                animator.SetBool("press", false);
                Instantiate(changingCover, GameObject.Find("Canvas").transform);
                changingScene = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
        if (changingScene)
        {
            timer += Time.deltaTime;
            if (timer >= 1.5)
            {
                if (buttonController.cur_index==buttonController.max_index)
                Application.Quit();
                else
                SceneManager.LoadScene(1);
            }
        }
    }
}
