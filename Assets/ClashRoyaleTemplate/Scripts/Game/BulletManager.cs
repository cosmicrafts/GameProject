using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : MonoBehaviour
{
    
    public Action<BulletManager> OnBulletImpact;
    public int AttackDamage { get; private set; }
    public GameObjectManager TargetObject { get; private set; }
    [SerializeField] private GameObject explosionImpact;

    public bool IsPaused = false;
    
    public void Shot(float velocity, int attackDamage, GameObjectManager gameObjectManager)
    {
        AttackDamage = attackDamage;
        TargetObject = gameObjectManager;
        StartCoroutine(OnShot(velocity, gameObjectManager));
    }
    
    private IEnumerator OnShot(float velocity, GameObjectManager gameObjectManager)
    {
        float distance = Vector3.Distance(gameObjectManager.transform.position, transform.position);
        float shotTime = distance / velocity;
        float time = 0.0f;
        Vector3 startPosition = transform.position;
        
        while(gameObjectManager && time < shotTime)
        {
            if (!IsPaused)
            {
                time += Time.deltaTime * velocity;
                transform.position = Vector3.up + Vector3.Lerp(startPosition, gameObjectManager.transform.position, time / shotTime);
            }
            yield return new WaitForEndOfFrame();
        }
        
        OnBulletImpact?.Invoke(this);
        GameObject explosion = Instantiate(explosionImpact, transform.position, transform.rotation);
        Destroy(explosion, 2); 
        Destroy(this.gameObject);
        yield return new WaitForSeconds(0.1f);
    }
    
    
}
