using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PauseScreen;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyPause();
        }
    }
    public void ApplyPause()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
