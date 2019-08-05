using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }

    private void UpdateHealthBar(float amount)
    {
        healthBar.fillAmount = amount;
    }
}
