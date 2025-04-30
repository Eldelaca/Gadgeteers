using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject gadgetUpgradeCollectText;
    private void OnTriggerEnter(Collider other)
    {
        gadgetUpgradeCollectText.SetActive(true);
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(3f);
        gadgetUpgradeCollectText.SetActive(false);
        Destroy(gameObject);
    }
}

