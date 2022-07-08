using UnityEngine;
using UnityEngine.SceneManagement;
public class creditcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Startgame()
    {
        SceneManager.LoadScene("Volleyball");
    }
    public void Startgame2()
    {
        SceneManager.LoadScene("Volleyball2");
    }
}
