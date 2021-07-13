using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Grid grid;
    public int currentGridIndex;
    public bool isMoving;
    private Vector3 origPos, targetPos, camOrigPos, camTargetPos;
    private float timeToMove = 0.1f;
    private float timeToStand = 0.1f;
    private float cameraMoveTime = 0.2f;
    public int attack;
    public int moveSpeed;
    public Transform mainCamera;
    public AnimalType animalType;
    [SerializeField] Animator animator;
    [SerializeField] SoundManager soundManager;
    [SerializeField] AnimatorFunction animatorFunction;


    private void Start()
    {
        InitialzeSprite(currentGridIndex);
    }
    public void InitialzeSprite(int point)
    {
        currentGridIndex = point;
        targetPos = grid.blocks[currentGridIndex].transform.position;
        transform.position = targetPos;

    }
    private IEnumerator MoveCamera()
    {
        float elapsedTime = 0;
        camOrigPos = mainCamera.position;
        while (elapsedTime < cameraMoveTime)
        {
            camTargetPos = new Vector3(0, transform.position.y, -10);
            mainCamera.position = Vector3.Lerp(camOrigPos, camTargetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.position = new Vector3(0, transform.position.y, -10);
    }

    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Game)
        {
            if (!isMoving && (currentGridIndex + 6 < grid.blocks.Count))
            {
                if (grid.GapUnder(currentGridIndex))
                {
                    print("FALL");
                    //StartCoroutine(MovePlayer(6));
                    StartCoroutine(Falling(6));
                }
            }
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
        }

    }


    IEnumerator MovePlayer(int direction)
    {
        int speed = moveSpeed;

        if (grid.GetMovable(currentGridIndex, direction, speed) == 1)
        {
            float animTime = 0;
            animator.SetBool("Attacking", true);

            while (animTime < timeToStand)
            {
                animTime += Time.deltaTime;
                yield return null;
            }
            AttackBlock(direction, attack, speed);
            animator.SetBool("Attacking", false);

            yield break;
        }
        if (grid.GetMovable(currentGridIndex, direction, speed) == 2)
        {
            if (animalType == AnimalType.Horse)
            {
                speed = grid.CheckTargetAccessible(currentGridIndex, direction, speed);
            }
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
            animator.SetBool("Moving", false);
            transform.position = targetPos;
            currentGridIndex += direction * speed;
            isMoving = false;
        }
    }
    IEnumerator Falling(int direction)
    {
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = grid.GetTargetTransform(currentGridIndex, direction, 1).position;
        isMoving = true;
        animator.SetBool("Moving", true);
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("Moving", false);
        transform.position = targetPos;
        currentGridIndex += direction * 1;
        isMoving = false;
    }
    public void AttackBlock(int direction, int attack, int speed)
    {
        print("ATTACKED");
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
