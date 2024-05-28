using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] GameObject storePanel;
    public static StoreManager _instance;
    public static StoreManager Instance {get{return _instance;}}
    private void Awake(){
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance =  this;
        }
    }

    public void SetStorePanel(bool state){
        storePanel.SetActive(state);
    }
}
