using UnityEngine;

public class WeaponPos : MonoBehaviour
{
    public Transform Pos;

    // Update is called once per frame
    private void Update()
    {
        transform.position = Pos.position;
    }

}

