using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalForm : MonoBehaviour
{
    public float duration;
    public AnimalType animalType;
    float timer;
    bool startTimer;
    public void SwitchToAnimal()
    {
        startTimer = true;
    }
    private void Update()
    {
        if (startTimer)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                AnimalBar.Instance.UpdateUI(timer / duration);
            }
            else
            {
                Debug.Log("CHANGEBACK");
                timer = 0;
                startTimer = false;
                AnimalBar.Instance.ChangeForm(0, animalType);
                AnimalBar.Instance.UpdateUI(0);

            }
        }
    }
}
