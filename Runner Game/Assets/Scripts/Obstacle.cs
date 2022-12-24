using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if(player != null)
        {
            player.gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("GameOver").SetActive(true);
            Debug.Log("GAME OVER!!");
        }    
    }
}
