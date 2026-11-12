using Unity.VisualScripting;
using UnityEngine;

public class PlaySoundWhileInHB : MonoBehaviour
{
    /*Audio emiter should be on:
    loop = true
    mute = true
    at default.

    Important, make sure this object only has the hitbox that matters to this script
    */
    [SerializeField] AudioSource audio;
    [SerializeField] bool playOnlyOnEnter; //Do not have loop on when this is true!

    [SerializeField] bool removeHoldFromDistanceDestroy;
    [SerializeField] bool targetParentObj;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audio.mute = false;
            if (playOnlyOnEnter)
            {
                audio.Play();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playOnlyOnEnter)
        {
            audio.mute = true;

            if (removeHoldFromDistanceDestroy)
            {
                if (targetParentObj)
                {
                    GetComponentInParent<DestroySelfWhenFarFromPlayer>().hold = false;
                }
                else
                {
                    GetComponent<DestroySelfWhenFarFromPlayer>().hold = false;
                }
            }
        }
    }
}
