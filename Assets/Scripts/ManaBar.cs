using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public GameObject manaBarUI;

    public void manaAppear(){
        manaBarUI.SetActive(true);
    }

    public void SetMaxMana(int mana){
        slider.maxValue = mana;
        slider.value = mana;
    }

    public void SetMana(int mana){
        slider.value = mana;
    }

}
