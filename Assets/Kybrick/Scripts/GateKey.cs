using UnityEngine;

public class GateKey : MonoBehaviour
{
    [SerializeField] public bool Active;
    [SerializeReference] private Sprite BInactive;
    [SerializeReference] private Sprite BActive;
    [SerializeReference] private SpriteRenderer Render;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet"))
        {
            if (Active)
            {
                Active = false;
            }
            else
            {
                Active = true;
            }
        }
    }
    public void Update()
    {
        if (Active)
        {
            Render.sprite = BActive;
        }
        else
        {
            Render.sprite = BInactive;
        }
    }
}
