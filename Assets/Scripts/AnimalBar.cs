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
    public Image henshinImage;
    public GameObject henshinGameObject;
    public AudioClip ding1;
    public AudioClip ding2;
    public AudioClip ding3;
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

    private void Update()
    {

    }
    public void AddToAnimalList(AnimalType animal)
    {
        if (currentList.Count == 0)
        {
            currentList.Add(animal);
            AddBarUI(animal);
            SoundManager.instance.AudioPlay(ding1);
            return;
        }
        if (!animal.Equals(currentList[0]))
        {
            currentList.Clear();
            ClearBarUI();
            SoundManager.instance.AudioPlay(ding1);
        }
        if (currentList.Count == 1)
        {
            SoundManager.instance.AudioPlay(ding2);
        }
        if (currentList.Count == 2)
        {
            SoundManager.instance.AudioPlay(ding3);
        }
        currentList.Add(animal);
        AddBarUI(animal);
        if (currentList.Count == 3)
        {
            StartCoroutine(StartChanging(animal));
        }
    }

    IEnumerator StartChanging(AnimalType animal)
    {

        ChangeForm(animal, 0);
        currentList.Clear();
        yield return new WaitForSeconds(0.5f);
        ClearBarUI();
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
        henshinImage.fillAmount = 1 - amount;
    }
    public void ChangeForm(AnimalType tarAnimal, AnimalType originAnimal)
    {
        if (tarAnimal != 0)
            henshinGameObject.SetActive(true);
        else
            henshinGameObject.SetActive(false);
        player.transform.GetChild((int)originAnimal).gameObject.SetActive(false);
        GameObject targetAnimal = player.transform.GetChild(((int)tarAnimal)).gameObject;
        targetAnimal.SetActive(true);
        henshinGameObject.transform.SetParent(targetAnimal.transform);
        henshinGameObject.transform.position = targetAnimal.transform.position + new Vector3(1.5f, 1.5f, 0);

        int currentPoint = player.transform.GetChild((int)originAnimal).gameObject.GetComponent<PlayerController>().currentGridIndex;
        targetAnimal.GetComponent<PlayerController>().InitialzeSprite(currentPoint);
        AnimalForm af = targetAnimal.GetComponent<AnimalForm>();
        if (af != null)
            targetAnimal.GetComponent<AnimalForm>().SwitchToAnimal();
    }
}
