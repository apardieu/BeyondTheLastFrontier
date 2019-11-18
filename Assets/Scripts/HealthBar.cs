using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    /*static Image Barre;

    public float max { get; set; }

    private float Valeur;
    public float valeur
    {
        get { return Valeur; }

        set
        {
            Valeur = Mathf.Clamp(value, 0, max);
            Barre.fillAmount = (1 / max) * Valeur;

        }
    }



    void Start()
    {
        Barre = GetComponent<Image>();
    }
    */
    [SerializeField]
    private int hp;
    [SerializeField]
    private int hpmax;
    private Image healthbar;

    void Start()
    {
        healthbar = GetComponent<Image>();
    }

    public void setHpMax(int hpSet)
    {
        hpmax=hpSet;
        hp = hpmax;
    }

    // Inflige des dégâts
    public void TakeDamage(int damages)
    {
        hp -= damages;
        UpdateHealth();
    }
    // Soigne le joueur
    public void Heal(int heal)
    {
        hp += heal;
        UpdateHealth();
    }

    // Remet au max
    public void HealMax()
    {
        hp = hpmax;
        UpdateHealth();
    }

    // Actualise les points de vie pour rester entre 0 et hpmax
    private void UpdateHealth()
    {
        hp = Mathf.Clamp(hp, 0, hpmax);
        float amount = (float)hp / hpmax;
        healthbar.fillAmount = amount;
    }

}
