using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] private PlayerStatus requiredPlayerStatus;
    [SerializeField] private GameObject[] objectsToActivate;
    
     private bool isActivated = false;
    

    void Update()
    {
        if (PlayerControllerManager.Instance.currentPlayerStatus == requiredPlayerStatus && !isActivated)
        {
            
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
            
            isActivated = true;
        }

        if (PlayerControllerManager.Instance.currentPlayerStatus != requiredPlayerStatus && isActivated)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(false);
            }
        }
       
    }
}
