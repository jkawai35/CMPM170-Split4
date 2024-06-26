using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.Mathematics;
using TMPro;


public class StoreElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] TextMeshProUGUI valueText;
    int value;

    void Start(){
        value = towerPrefab.GetComponent<TowerBehavior>().value;
        valueText.text = value.ToString();
    }
    void SetMouseOver(bool state){
        if(state){
            transform.localScale = Vector3.one*1.1f;
        }
        else{
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        SetMouseOver(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        SetMouseOver(false);
    }
    public void OnPointerDown(PointerEventData eventData){
        if(ValueManager.Instance.updateMoney(-value)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -1;
            GameObject tower = Instantiate(towerPrefab, mousePos, quaternion.identity);
            StoreManager.Instance.SetStorePanel(false);
        }
    }
}
