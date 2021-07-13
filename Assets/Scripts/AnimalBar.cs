using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBar : MonoBehaviour
{
    private static AnimalBar _instance;
    public static AnimalBar Instance { get { return _instance; } }
    public List<AnimalType> currentList = new List<AnimalType>();
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
            return;
        }
        if (!animal.Equals(currentList[0]))
        {
            currentList.Clear();
            //clear UI
        }
        currentList.Add(animal);
        //Add UI
        if (currentList.Count == 3)
        {
            Debug.Log("CHANGE FORM");
        }
    }

}
