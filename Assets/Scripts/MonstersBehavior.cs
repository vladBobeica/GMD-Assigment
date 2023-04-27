using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersBehavior : MonoBehaviour
{
    protected int lives;

    public virtual void GetDamage()
    {   
        Debug.Log("Monster got damage");
        lives--;
    
        if (lives <= 0)
        Die();


    }

    public virtual void Die() 
    {
        Destroy(this.gameObject);
    }
}
