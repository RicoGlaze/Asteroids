using Unity.VisualScripting;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 20f)]
    public float scrollSpeed = 0.5f;

    private float maxSpeed = 2.5f;
    private float offset;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

    public void IncreaseSpeed()
    {
        if(scrollSpeed < maxSpeed)
        {
            scrollSpeed += 0.1f;
        }
    }

    public void ResetSpeed()
    {
        scrollSpeed = 0.5f;
    }
}