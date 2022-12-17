using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHP : MonoBehaviour
{
    [SerializeField] float HP;
    [SerializeField] Text showHP;

    void Start()
    {
        HP = GameObject.Find("Player (1)").GetComponent<Target>().health;
    }

    void Update()
    {
        HP = GameObject.Find("Player (1)").GetComponent<Target>().health;
        showHP.text = HP.ToString();
    }
}
