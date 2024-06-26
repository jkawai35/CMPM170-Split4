using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class TowerBehavior : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject bulletPrefab;
    [Header("Atrributes")]
    [SerializeField] float range;
    [Header("Bullet Atrributes")]
    [SerializeField] float size;
    [SerializeField] float speed;
    [SerializeField] int amount;
    [SerializeField] float delay;
    [SerializeField] float spread;
    public int value;
    [SerializeField] float damage;
    [SerializeField] int pierce;
    SpriteRenderer sr;
    bool validPlacement = false;
    GameObject currentSpace = null;
    bool validOffset=false;
    Vector3 offset;
    GameObject closestEnemy = null;

    List<GameObject> validSpaces=new List<GameObject>();

    void Start(){
        sr = sprite.GetComponent<SpriteRenderer>();
        StartCoroutine(Attack());
    }


    void Update(){
        if(!validPlacement){
            MouseControls();
        }
        else{
            closestEnemy = GetClosestEnemyInRange();
            if(closestEnemy!=null){
                Debug.DrawRay(transform.position,closestEnemy.transform.position-transform.position,Color.blue,0f);
            }
        }
    }
    Vector3 MouseWorldPosition(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void SetTowerAlpha(float alpha){
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }
    //shooting
    IEnumerator Attack(){
        while(true){
            if(closestEnemy==null){
                yield return null;
            }
            else{
                float spreadAmount = 360f*spread;
                for(int i =0;i<amount;i++){
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, quaternion.identity);
                    BulletBehavior bh = bullet.GetComponent<BulletBehavior>();
                    bh.distance = range;
                    bh.damage = damage;
                    bh.pierce = pierce;
                    bullet.transform.localScale=Vector3.one*size;
                    Rigidbody2D br = bullet.GetComponent<Rigidbody2D>();
                    float angle = (i - (amount - 1) / 2.0f) * (spreadAmount / amount);
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    br.velocity=rotation*Vector3.Normalize(closestEnemy.transform.position-transform.position)*speed;
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }

    //targeting
    GameObject GetClosestEnemyInRange(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector3.zero, 0f);
        GameObject closestEnemy = null;
        if (hits.Length > 0){
            RaycastHit2D check = hits
            .Where(hit => hit.collider.gameObject.CompareTag("Enemy"))
            .OrderBy(hit => Vector2.Distance(hit.collider.gameObject.transform.position, transform.position))
            .FirstOrDefault();
            
            if(check!=default(RaycastHit2D)){
                closestEnemy=check.collider.gameObject;
            }
        }
        return closestEnemy;
    }


    //Mouse Controls
    void OnMouseOver(){
        if(!validPlacement){return;}
        if(Input.GetMouseButtonDown(1)){
            ValueManager.Instance.updateMoney(value/2);
            Destroy(gameObject);
        }
    }

    void MouseControls(){
        if(Input.GetMouseButton(0)){
            if(!validOffset){
                offset = transform.position-MouseWorldPosition();
                validOffset=true;
            }
            transform.position = MouseWorldPosition()+offset;
            UpdateSpriteLocation();
        }
        if(Input.GetMouseButtonUp(0)){
            if(currentSpace!=null){
                transform.position=sprite.transform.position;
                sprite.transform.localPosition=Vector3.zero;
                validPlacement=true;
                StoreManager.Instance.SetStorePanel(true);
            }
            else{
                ValueManager.Instance.updateMoney(value);
                Destroy(gameObject);
                StoreManager.Instance.SetStorePanel(true);
            }
        }
    }

    

    //Placement
    void UpdateSpriteLocation(){
        if(validSpaces.Count==0){
            sprite.transform.position=transform.position;
            currentSpace = null;
        }
        else{
            GameObject closestSpace = validSpaces.OrderBy(space => Vector3.Distance(space.transform.position, transform.position)).First();
            sprite.transform.position=closestSpace.transform.position+Vector3.back;
            currentSpace = closestSpace;
        }
    }
    void OnTriggerEnter2D(Collider2D collider) {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Space") && !validSpaces.Contains(other)) {
            validSpaces.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Space") && validSpaces.Contains(other)) {
            validSpaces.Remove(other);
        }
    }
}
