using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartController : MonoBehaviour
{
    public float intensity;
    public int id;
    private bool iscollected;
    public Image uiImage;
    public Sprite collectedSprite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!iscollected)
            {
                GameManager.Instance.UpdateCollected(id);
                iscollected = true;
                Destroy(gameObject);
                other.GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", other.GetComponentInChildren<SkinnedMeshRenderer>().material.GetColor("_EmissionColor")*intensity);
                uiImage.sprite = collectedSprite;
                uiImage.transform.localScale = new Vector3(1,1,1);
                other.GetComponent<PlayerController>().life = 3;
            }
            
        }
    }
}
