using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Grid grid;
    Block currentGrid;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.1f;
    public LayerMask block;

    public int attack;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(-1));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(1));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(6));
        }
        if (!isMoving && (grid.index + 6 < grid.blocks.Count))
        {
            if (grid.GapUnder())
            {
                StartCoroutine(MovePlayer(6));
            }
        }
    }


    IEnumerator MovePlayer(int direction)
    {
        if (grid.GetMovable(direction) == 1)
        {
            AttackBlock(attack);
        }
        if (grid.GetMovable(direction) == 2)
        {
            float elapsedTime = 0;
            origPos = transform.position;
            targetPos = grid.GetTargetTransform(direction).position;
            isMoving = true;
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
            isMoving = false;
        }
    }

    public void AttackBlock(int attack)
    {
        Debug.Log("SSSSSS");
        grid.targetBlockGameObject.GetComponentInChildren<Block>().TakeDamage(attack);
        grid.ResetTarget();
        //check down block;
    }
}
