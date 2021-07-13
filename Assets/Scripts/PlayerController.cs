using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Grid grid;
    public int currentGridIndex;
    private bool isMoving;
    private Vector3 origPos, targetPos, camOrigPos, camTargetPos;
    private float timeToMove = 0.1f;
    private float cameraMoveTime = 0.1f;
    public int attack;
    public int moveSpeed;
    public Transform cameraTransform;
    public Transform mainCamera;
    public AnimalType animalType;
    [SerializeField] Animator animator;
    [SerializeField] SoundManager soundManager;
    [SerializeField] AnimatorFunction animatorFunction;


    private IEnumerator MoveCamera()
    {
        float elapsedTime = 0;
        camOrigPos = mainCamera.position;
        while (elapsedTime < cameraMoveTime)
        {
            //transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            camTargetPos = new Vector3(0, cameraTransform.position.y, cameraTransform.position.z);
            Debug.Log(cameraTransform.position);
            mainCamera.position = Vector3.Lerp(camOrigPos, camTargetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //transform.position = targetPos;
        mainCamera.position = new Vector3(0, cameraTransform.position.y, cameraTransform.position.z);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(-1));
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            StartCoroutine(MovePlayer(1));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(6));
            StartCoroutine(MoveCamera());
        }
        if (!isMoving && (currentGridIndex + 6 < grid.blocks.Count))
        {
            if (grid.GapUnder(currentGridIndex))
            {
                StartCoroutine(MovePlayer(6));
            }
        }
    }


    IEnumerator MovePlayer(int direction)
    {
        int speed = moveSpeed;
        if (animalType == AnimalType.Horse)
        {
            speed = grid.CheckTargetAccessible(currentGridIndex, direction, speed);

        }
        if (grid.GetMovable(currentGridIndex, direction, speed) == 1)
        {
            AttackBlock(direction, attack, speed);
        }
        if (grid.GetMovable(currentGridIndex, direction, speed) == 2)
        {
            float elapsedTime = 0;
            origPos = transform.position;
            targetPos = grid.GetTargetTransform(currentGridIndex, direction, speed).position;
            isMoving = true;
            animator.SetBool("Moving", true);
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
            currentGridIndex += direction * speed;
            isMoving = false;
            animator.SetBool("Moving", false);
        }
    }

    public void AttackBlock(int direction, int attack, int speed)
    {
        grid.blocks[currentGridIndex + direction * speed].GetComponentInChildren<Block>().TakeDamage(attack);
        if (animalType == AnimalType.Pig)
        {
            PigSkill(direction, attack);
        }
        if (animalType == AnimalType.Elephant)
        {
            ElephantSkill(direction, attack);
        }
        if (animalType == AnimalType.Lion)
        {
            LionSkill(direction, attack);
        }
        grid.ResetTarget();
        //check down block;
    }

    private void PigSkill(int direction, int attack)
    {
        if (direction == 6)
        {
            if ((currentGridIndex + direction + 1) % 6 != 0)
            {
                Block bl = grid.blocks[currentGridIndex + direction + 1].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
            if (currentGridIndex % 6 != 0 && currentGridIndex != 0)
            {
                Block bl = grid.blocks[currentGridIndex + direction - 1].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
        }
        if (direction == -1) //left
        {
            Block bl = grid.blocks[currentGridIndex + grid.mapWidth - 1].GetComponentInChildren<Block>();
            if (bl != null)
                bl.TakeDamage(attack);
            if (currentGridIndex - grid.mapWidth - 1 >= 0)
            {
                Block b2l = grid.blocks[currentGridIndex - grid.mapWidth - 1].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
        if (direction == 1) //right
        {
            Block bl = grid.blocks[currentGridIndex + grid.mapWidth + 1].GetComponentInChildren<Block>();
            if (bl != null)
                bl.TakeDamage(attack);
            {
                Block b2l = grid.blocks[currentGridIndex - grid.mapWidth + 1].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
    }
    private void ElephantSkill(int direction, int attack)
    {
        if (direction == 6) //down
        {
            if ((currentGridIndex + direction) < grid.blocks.Count)
            {
                Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
            if (currentGridIndex + direction * 2 < grid.blocks.Count)
            {
                Block bl = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
        }
        if (direction == -1) //left
        {
            Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
            if (bl != null)
            {
                bl.TakeDamage(attack);
            }
            if ((currentGridIndex + direction * 2) % grid.mapWidth < currentGridIndex % grid.mapWidth)
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
        if (direction == 1) //right
        {
            Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
            if (bl != null)
                bl.TakeDamage(attack);
            if (currentGridIndex % grid.mapWidth < ((currentGridIndex + direction * 2) % grid.mapWidth))
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
    }
    private void LionSkill(int direction, int attack)
    {
        if (direction == 6) //down
        {
            if ((currentGridIndex + direction) < grid.blocks.Count)
            {
                Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
            if (currentGridIndex + direction * 2 < grid.blocks.Count)
            {
                Block bl = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
            if (currentGridIndex + direction * 3 < grid.blocks.Count)
            {
                Block bl = grid.blocks[currentGridIndex + direction * 3].GetComponentInChildren<Block>();
                if (bl != null)
                    bl.TakeDamage(attack);
            }
        }
        if (direction == -1) //left
        {
            Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
            if (bl != null)
            {
                bl.TakeDamage(attack);
            }
            if ((currentGridIndex + direction * 2) % grid.mapWidth < currentGridIndex % grid.mapWidth)
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
            if ((currentGridIndex + direction * 3) % grid.mapWidth < currentGridIndex % grid.mapWidth)
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 3].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
        if (direction == 1) //right
        {
            Block bl = grid.blocks[currentGridIndex + direction].GetComponentInChildren<Block>();
            if (bl != null)
                bl.TakeDamage(attack);
            if (currentGridIndex % grid.mapWidth < ((currentGridIndex + direction * 2) % grid.mapWidth))
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 2].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
            if (currentGridIndex % grid.mapWidth < ((currentGridIndex + direction * 3) % grid.mapWidth))
            {
                Block b2l = grid.blocks[currentGridIndex + direction * 3].GetComponentInChildren<Block>();
                if (b2l != null)
                    b2l.TakeDamage(attack);
            }
        }
    }
}


public enum AnimalType
{
    Human,
    Pig,
    Horse,
    Peguin,
    Elephant,
    Lion
}
