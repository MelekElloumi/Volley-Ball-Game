using UnityEngine;
using UnityEngine.SceneManagement;
public class menucontroller : MonoBehaviour
{
    public GameObject instructions;
    public void Startgame()
    {
        SceneManager.LoadScene("Volleyball");
    }
    public void Startgame2()
    {
        SceneManager.LoadScene("Volleyball2");
    }
    public void Showinstructions(bool x)
    {
        instructions.SetActive(x);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
