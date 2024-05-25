using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] GameObject sprite;
    SpriteRenderer sr;
    bool validPlacement = false;
    GameObject currentSpace = null;
    Vector3 originalPos;
    Vector3 offset;

    List<GameObject> validSpaces=new List<GameObject>();

    void Start(){
        sr = sprite.GetComponent<SpriteRenderer>();
        SetTowerAlpha(0.5f);
    }


    void Update(){
        UpdateSpriteLocation();
    }
    Vector3 MouseWorldPosition(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void SetTowerAlpha(float alpha){
        Color color = sr.color;
        color.a = alpha;
        sr.color = color;
    }


    //Mouse Controls
    void SetMouseOverState(bool state){
        if(state){
            SetTowerAlpha(1f);
        }
        else{
            SetTowerAlpha(.5f);
        }
    }
    void OnMouseEnter(){
        if(validPlacement){return;}
        SetMouseOverState(true);
    }
    void OnMouseExit(){
        if(validPlacement){return;}
        SetMouseOverState(false);
    }
    void OnMouseDown(){
        if(validPlacement){return;}
        originalPos=transform.position;
        offset = transform.position-MouseWorldPosition();
    }

    void OnMouseUp(){
        if(validPlacement){return;}
        if(currentSpace!=null){
            transform.position=sprite.transform.position;
        }
    }

    void OnMouseDrag(){
        if(validPlacement){return;}
        transform.position = MouseWorldPosition()+offset;
    }

    //Placement
    void UpdateSpriteLocation(){
        if(validSpaces.Count==0){
            sprite.transform.position=transform.position;
            currentSpace = null;
        }
        else{
            GameObject closestSpace = validSpaces.OrderBy(space => Vector3.Distance(space.transform.position, transform.position)).First();
            sprite.transform.position=closestSpace.transform.position;
            currentSpace = closestSpace;
        }
    }
    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("triggerEnterUpdate");
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
