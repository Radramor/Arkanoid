using UnityEngine;

public class Play : MonoBehaviour
{
    public GameObject PauseScreen;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyPlay();
        }
    }
    public void ApplyPlay()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
