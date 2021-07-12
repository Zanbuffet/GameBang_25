using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<GameObject> blocks;
    public int mapWidth, mapHeight;
    public int index;
    public GameObject targetBlockGameObject;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            blocks.Add(transform.GetChild(i).gameObject);
        }
    }

    public bool GapUnder()
    {
        int target = index + 6;
        Block targetBlock = blocks[target].GetComponentInChildren<Block>();
        if (targetBlock != null)
            return false;
        else
            return true;
    }

    public int GetMovable(int direction)
    {
        int target = index + direction;
        if (target >= 0 && target < mapHeight * mapWidth)
        {
            Block targetBlock = blocks[target].GetComponentInChildren<Block>();
            if (targetBlock != null)
            {
                targetBlockGameObject = blocks[target];
                return 1;
            }
            if (index % 6 == 0 && direction == -1)//left
                return 0;
            if ((index + 1) % 6 == 0 && direction == 1)
                return 0;
            else
                return 2;
        }
        else
            return 0;
    }

    public bool GetAttackTarget(int direction)
    {
        int target = index + direction;
        Block targetBlock = blocks[target].GetComponentInChildren<Block>();
        if (targetBlock != null)
        {
            targetBlockGameObject = blocks[target];
            return true;
        }
        return false;
    }
    public Transform GetTargetTransform(int direction)
    {
        index += direction;
        return blocks[index].transform;
    }

    public void ResetTarget()
    {
        targetBlockGameObject = null;
    }
}
