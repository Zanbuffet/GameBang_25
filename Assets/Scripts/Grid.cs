using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<GameObject> blocks;
    public int mapWidth, mapHeight;
    public int startIndex;
    public GameObject targetBlockGameObject;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            blocks.Add(transform.GetChild(i).gameObject);
        }
    }

    public bool GapUnder(int currentIndex)
    {
        int target = currentIndex + mapWidth;
        Block targetBlock = blocks[target].GetComponentInChildren<Block>();
        if (targetBlock != null)
            return false;
        else
            return true;
    }

    public int GetMovable(int currentIndex, int direction, int speed)
    {
        int target = currentIndex + direction * speed;
        if (target >= 0 && target < mapHeight * mapWidth)
        {
            if (currentIndex % 6 == 0 && direction == -1)//left
                return 0;
            if ((currentIndex + 1) % 6 == 0 && direction == 1) //right
                return 0;
            Block targetBlock = blocks[target].GetComponentInChildren<Block>();
            if (targetBlock != null)
            {
                targetBlockGameObject = blocks[target];
                return 1;
            }
            else
                return 2;
        }
        else
            return 0;
    }

    public bool GetAttackTarget(int currentIndex, int direction)
    {
        int target = currentIndex + direction;
        Block targetBlock = blocks[target].GetComponentInChildren<Block>();
        if (targetBlock != null)
        {
            targetBlockGameObject = blocks[target];
            return true;
        }
        return false;
    }
    public Transform GetTargetTransform(int currentIndex, int direction, int speed)
    {
        //index += direction;
        return blocks[currentIndex + direction * speed].transform;
    }

    public int CheckTargetAccessible(int currentGridIndex, int direction, int speed)
    {
        if (currentGridIndex + direction * speed < blocks.Count)
        {
            Block bl = blocks[currentGridIndex + direction * speed].GetComponentInChildren<Block>();
            if (bl != null && bl.ice)
            {
                return 1;
            }
        }
        if (direction == -1) //move left
        {
            if ((currentGridIndex - speed) % 6 > currentGridIndex % 6)
            {
                return 1;
            }
        }
        if (direction == 1) //move right
        {
            if ((currentGridIndex + speed) % 6 < currentGridIndex % 6)
            {
                return 1;
            }
        }
        if (direction == 6) //move down
        {
            if ((currentGridIndex + direction * speed) >= blocks.Count)
            {
                return 1;
            }
        }
        return speed;
    }
    public void ResetTarget()
    {
        targetBlockGameObject = null;
    }
}
