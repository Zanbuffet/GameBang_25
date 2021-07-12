using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int health;
    public bool death;
    public float staminaRecover;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //GameManger.Instance.AddStamina(staminaRecover)
            if(death)
            {
                //death effect
                GameManager.Instance.GameOver();
            }
            Destroy(gameObject);

        }
    }
}
