using System.Linq;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeReference] private SpriteRenderer Render;
    [SerializeReference] private BoxCollider2D col;
    [SerializeReference] private Sprite InactiveSpr;
    [SerializeReference] private Sprite ActiveSpr;
    [SerializeReference] private GameObject Placeholder;
    [SerializeReference] private GameObject[] Keys;
    [SerializeReference] private GameObject Check;
    [SerializeField] private bool active;
    [SerializeReference] private GateKey Key;
    public bool BCheck;
    public float B;
    public int I;


    void Start()
    {
        for(int i =0; i == Keys.Length; i+=1)
        {
            Key = Keys[i].GetComponent<GateKey>();
            Key.Active = true;
        }
    }
    void FixedUpdate()
    {
        if (active)
        {
            col.isTrigger = false;
            Render.sprite = ActiveSpr;
        }
        else
        {
            col.isTrigger = true;
            Render.sprite = InactiveSpr;
        }
        I = 0;
        B = 0F;
        while(I<=Keys.Length-1)
        {
            Key = Keys[I].GetComponent<GateKey>();
            if (Key.Active)
            {
                BCheck = true;
                B += 1;
            }
            else
            {
                BCheck = false;
            }
            I += 1;
            if (B == 0)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }
 
    }
}
