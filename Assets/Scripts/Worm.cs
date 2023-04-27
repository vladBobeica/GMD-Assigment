using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonstersBehavior
{
    private void Start()
    {
        lives = 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject == Mikkel.Instance.gameObject)
    {
        Mikkel.Instance.GetDamage();
        
    }

}
}
