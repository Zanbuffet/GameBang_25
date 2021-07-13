using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBar : MonoBehaviour
{
    private static AnimalBar _instance;
    public static AnimalBar Instance { get { return _instance; } }
    public List<AnimalType> currentList = new List<AnimalType>();
    public GameObject player;
    public List<GameObject> animalUI = new List<GameObject>();
    public Transform uiPanel;
    public Image henshenUI;
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

    public void AddToAnimalList(AnimalType animal)
    {
        if (currentList.Count == 0)
        {
            currentList.Add(animal);
            AddBarUI(animal);
            return;
        }
        if (!animal.Equals(currentList[0]))
        {
            currentList.Clear();
            ClearBarUI();
        }
        currentList.Add(animal);
        AddBarUI(animal);
        if (currentList.Count == 3)
        {
            Debug.Log("CHANGE FORM");
            ChangeForm(animal, 0);
            currentList.Clear();
            ClearBarUI();
        }
    }

    void AddBarUI(AnimalType animalType)
    {
        //temp
        Instantiate(animalUI[(int)animalType - 1], uiPanel);
    }
    void ClearBarUI()
    {
        foreach (Transform child in uiPanel)
        {
            Destroy(child.gameObject);
        }
    }
    public void UpdateUI(float amount)
    {
        henshenUI.fillAmount = 1 - amount;
    }
    public void ChangeForm(AnimalType tarAnimal, AnimalType originAnimal)
    {
        player.transform.GetChild((int)originAnimal).gameObject.SetActive(false);
        GameObject targetAnimal = player.transform.GetChild(((int)tarAnimal)).gameObject;
        targetAnimal.SetActive(true);

        int currentPoint = player.transform.GetChild((int)originAnimal).gameObject.GetComponent<PlayerController>().currentGridIndex;
        targetAnimal.GetComponent<PlayerController>().InitialzeSprite(currentPoint);
        AnimalForm af = targetAnimal.GetComponent<AnimalForm>();
        if (af != null)
            targetAnimal.GetComponent<AnimalForm>().SwitchToAnimal();
    }
}
