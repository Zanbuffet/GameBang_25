using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpeningController : MonoBehaviour
{
    [SerializeField] GameObject comic;
    [SerializeField] Transform playerMap;
    [SerializeField] Transform playerTarget;
    [SerializeField] Vector3 targetPos, origPos;
    [SerializeField] float speed;
    [SerializeField] float timeToMove;

    private void Start()
    {
        StartCoroutine(CameraMoving());
    }

    IEnumerator CameraMoving()
    {
        yield return new WaitForSeconds(15f);
        float elapsedTime = 0;
        origPos = Camera.main.transform.position;
        targetPos = new Vector3(0, -21.09f, -10f);

        while (elapsedTime < timeToMove)
        {
            Camera.main.transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = targetPos;

        GameManager.Instance.gameState = GameState.Game;
    }
}
