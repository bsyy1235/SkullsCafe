// FloatingAnimation.cs
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float floatHeight = 20f;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float scaleAmount = 0.1f;

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        float newScale = 1f + Mathf.Sin(Time.time * floatSpeed) * scaleAmount;

        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.localScale = startScale * newScale;
    }
}