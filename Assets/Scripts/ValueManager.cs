using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ValueManager : MonoBehaviour
{
    [SerializeField] int initialHealth;
    public int currentHealth;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] int initialMoney;
    public int currentMoney;
    [SerializeField] TextMeshProUGUI moneyText;


    public static ValueManager _instance;
    public static ValueManager Instance {get{return _instance;}}
    private void Awake(){
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance =  this;
        }
        currentHealth = initialHealth;
        currentMoney = initialMoney;
        updateHealth(0);
        updateMoney(0);
    }

    public void updateHealth(int amount){
        currentHealth += amount;
        healthText.text = currentHealth.ToString();
        if(currentHealth<1){
            SceneManager.LoadScene("EndScene");
        }
    }
    public bool updateMoney(int amount){
        int check = currentMoney+amount;
        if(check<0){
            return false;
        }

        currentMoney = check;
        moneyText.text = currentMoney.ToString();
        return true;
    }
}
