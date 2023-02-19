using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Transform bar;
    
    public void SetBarSize(float percentage)
    {
        bar.transform.localScale = new Vector3(percentage, 1);
    }
}
