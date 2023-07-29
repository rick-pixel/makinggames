using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int Health;
    int maxHealth = 100;

    void Start(){
        Health = maxHealth;
    }

    void Update(){
        if(Health == 0){
            Destroy(this.gameObject);
        }
    }
    
    public void TakeDamage(int attackDamage){
        Health -= attackDamage;
        Debug.Log("Player's health: " + Health);
    }
}
