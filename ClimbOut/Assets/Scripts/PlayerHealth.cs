using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;

    void Update()
    {
        if (healthAmount <= 0)
        {
            // Optionally disable controls here
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PlayerTakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Max(healthAmount, 0f); // Clamp to zero
        healthBar.fillAmount = healthAmount / 100f;
    }
}