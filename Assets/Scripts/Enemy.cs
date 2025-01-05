using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
