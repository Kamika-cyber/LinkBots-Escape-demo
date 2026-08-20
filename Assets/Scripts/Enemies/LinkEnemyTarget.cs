using UnityEngine;

public class LinkEnemyTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyWholeObject = true;
    [SerializeField] private GameObject objectToDisable;

    public void DieFromLink()
    {
        if (destroyWholeObject)
        {
            Destroy(gameObject);
        }
        else
        {
            if (objectToDisable != null)
                objectToDisable.SetActive(false);
            else
                gameObject.SetActive(false);
        }
    }
}