using System;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class DamageRouter : MonoBehaviour
{
    MonoBehaviour script;

    void Start()
    {
        MonoBehaviour[] scripts;
        scripts = GetComponents<MonoBehaviour>(); 
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i].GetType().Name.Contains("TakeDamage")){
                script = scripts[i];
                break;
            }
        }
    }

    public void TakeDamage(float DMG)
    {
        //damageScript.TakeDamage(1);
    }
}
