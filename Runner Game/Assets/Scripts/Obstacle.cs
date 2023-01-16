using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : PoolObject
{
    
    private void OnTriggerEnter(Collider other) 
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if(player != null)
        {
            player.gameObject.SetActive(false);
            GameManager.Instance.OnGameOver?.Invoke();
            Debug.Log("GAME OVER!!");
        }    
    }
}
