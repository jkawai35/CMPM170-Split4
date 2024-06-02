using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float distance;
    public float damage;
    public int pierce;
    Vector3 origin;
    void Start(){
        origin=transform.position;
    }
    void Update(){
        if(Vector3.Distance(origin,transform.position)>distance){
            Destroy(gameObject);
        }        
    }
    void OnTriggerEnter2D(Collider2D collider) {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Enemy")) {
            if(pierce>0){
                pierce-=1;
            }
            else{
                ValueManager.Instance.updateMoney(10);
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(other);
                Destroy(gameObject);
            }
        }
    }
}
