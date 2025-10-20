
using UnityEngine;
using UnityEngine.UI;

public class UIQuestMarker : MonoBehaviour
{
    public Sprite icon;
    public Image image;

    private void Start()
    {
        UICompass.Instance.AddQuestMarker(this);
    }

    public Vector2 position
    {
        get 
        { 
            return new Vector2(transform.position.x, transform.position.z); 
        }
    }

    public void RemoveMarker()
    {
        UICompass.Instance.RemoveQuestMarket(this, image.gameObject);
    }
}
