using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FailPanelUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetAxis("Submit") == 1)
        {
            if (EventSystem.current.currentSelectedGameObject.name == "Quit")
            {
                Debug.Log("QUIT");
                GameManager.Instance.ReturnTittle();
            }
            if (EventSystem.current.currentSelectedGameObject.name == "Retry")
            {
                Debug.Log("RETRY");
                GameManager.Instance.NewGame();
            }
        }
    }
}
