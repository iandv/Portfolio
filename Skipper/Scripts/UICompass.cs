using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICompass : MonoBehaviour
{
    public GameObject iconPrefab;
    List<UIQuestMarker> questmarkers = new List<UIQuestMarker>();
    List<GameObject> images = new List<GameObject>();
    public RawImage compassImage;
    public Transform player;

    public float maxDistance = 1000f;

    float compassUnit;


    public static UICompass Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Duplicated UICompass found in GameObject: {gameObject.name}");
            Destroy(this);
        }
    }

    private void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (UIQuestMarker marker in questmarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

            float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
            float scale = 0f;

            if (dst < maxDistance)
            {
                scale = 1f - (dst / maxDistance);
            }

            marker.image.rectTransform.localScale = Vector3.one * scale;
        }
    }

    public void AddQuestMarker (UIQuestMarker marker)
    {
        GameObject newMarker = Instantiate(iconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        images.Add(newMarker);
        questmarkers.Add(marker);
    }

    public void RemoveQuestMarket (UIQuestMarker marker, GameObject image)
    {
        images.Remove(image);
        Destroy(image);
        questmarkers.Remove(marker);
    }

    Vector2 GetPosOnCompass (UIQuestMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
