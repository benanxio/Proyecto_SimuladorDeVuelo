using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionMenu : MonoBehaviour
{
[SerializeField] TMP_Dropdown ddCalidad;
[SerializeField] int calidad;
    void Start()
    {
        calidad = PlayerPrefs.GetInt("Calidad",3);
        ddCalidad.value = calidad;
        AjustarCalidad();
    }

    void Update()
    {
        
    }

    public void AjustarCalidad(){
        QualitySettings.SetQualityLevel(ddCalidad.value);
        PlayerPrefs.SetInt("Calidad",ddCalidad.value);
        calidad = ddCalidad.value;
    }
}
