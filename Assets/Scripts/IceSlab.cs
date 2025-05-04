using UnityEngine;

public class IceSlab : MonoBehaviour
{

    private float lifeTimer;

    void Start()
    {
        lifeTimer = 5f;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (!(lifeTimer <= 0f)) return;
        Destroy(gameObject);
    }

}
