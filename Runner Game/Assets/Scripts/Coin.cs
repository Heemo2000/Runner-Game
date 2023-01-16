using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PoolObject
{
    [SerializeField]private float rotatingSpeed = 25f;
    [SerializeField]private ParticleSystem coinPickEffect;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotatingSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if(player != null)
        {
            GameManager.Instance.OnScoreIncrease?.Invoke();
            SoundManager.Instance?.PlaySFX(SoundType.CoinPickup);
            Instantiate(coinPickEffect,transform.position,Quaternion.identity);
        }
        base.Destroy();    
    }
}
