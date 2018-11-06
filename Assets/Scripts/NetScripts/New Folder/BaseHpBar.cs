using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPlayer
{
    
}
public class BaseHpBar : MonoBehaviour
{

    public Slider HpBar;
    public NewGameUnit unit;


    // Use this for initialization
    void Start()
    {
        unit = GetComponent<NewGameUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < 0)
        {
            HpBar = GameObject.FindGameObjectWithTag("PlayerHp").GetComponent<Slider>();
        }
        if (transform.position.z > 0)
        {
            HpBar = GameObject.FindGameObjectWithTag("EnemyHp").GetComponent<Slider>();
        }
        HpBar.value = (float)unit.properties.currentHealth / unit.properties.maxHealth;
    }
}

