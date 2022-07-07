using UnityEngine;
using UnityEngine.SceneManagement;
public class menucontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public void Startgame()
    {
        SceneManager.LoadScene("Volleyball");
    }
}
