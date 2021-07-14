using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalForm : MonoBehaviour
{
    public float duration;
    public AnimalType animalType;
    float timer;
    bool startTimer;
    [SerializeField] GameObject effect;
    public void SwitchToAnimal()
    {
        startTimer = true;
        GameObject go = Instantiate(effect, transform);
        go.transform.position += Vector3.up;
        Destroy(go, 0.5f);
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

                GameObject go = Instantiate(effect,GameObject.Find("Human").transform);
                go.transform.position += Vector3.up;
                Destroy(go, 0.5f);
            }
        }
    }
}
