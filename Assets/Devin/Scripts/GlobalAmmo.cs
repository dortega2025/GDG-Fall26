using Unity.VisualScripting;
using UnityEngine;

public class GlobalAmmo : MonoBehaviour
{
    public static int handgunAmmoCount = 10;
    public static int handgunAmmoCountMax = 10;
    [SerializeField] GameObject ammoDisplay;
    void FixedUpdate()
    {
        ammoDisplay.GetComponent<TMPro.TMP_Text>().text = "" + handgunAmmoCount;

    }
}
