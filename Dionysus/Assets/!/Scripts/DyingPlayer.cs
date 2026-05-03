using UnityEngine;
using UnityEngine.SceneManagement;

public class DyingPlayer : MonoBehaviour
{
    public void Die()
    {
        SceneManager.LoadScene(0);
    }
}