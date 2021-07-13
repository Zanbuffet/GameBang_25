using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int health;
    public bool death;
    public bool ice;
    public float staminaRecover;
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //GameManger.Instance.AddStamina(staminaRecover)
            if (death)
            {
                if (damage < 11000)
                    //death effect
                    GameManager.Instance.GameOver();
            }
            Destroy(gameObject);
            GameManager.Instance.curStamina += 0.5f;
        }
    }
}
