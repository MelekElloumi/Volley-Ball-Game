using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Team
{
    Blue = 0,
    Purple = 1,
    Default = 2
}

public enum Event
{
    HitPurpleGoal = 0,
    HitBlueGoal = 1,
    HitOutOfBounds = 2,
    HitIntoBlueArea = 3,
    HitIntoPurpleArea = 4
}

public class VolleyballEnvController : MonoBehaviour
{
    int ballSpawnSide;
    int agentside;

    VolleyballSettings volleyballSettings;

    public VolleyballAgent blueAgent;
    public VolleyballAgent purpleAgent;
    public string bluename;
    public string purplename;
    public Text bluescore;
    public Text purplescore;
    public GameObject winbox;
    public AudioClip crowdcheer;
    public AudioClip wiiiin;
    public List<VolleyballAgent> AgentsList = new List<VolleyballAgent>();
    List<Renderer> RenderersList = new List<Renderer>();

    Rigidbody blueAgentRb;
    Rigidbody purpleAgentRb;

    public GameObject ball;
    Rigidbody ballRb;

    public GameObject blueGoal;
    public GameObject purpleGoal;

    Renderer blueGoalRenderer;
    int pscore = 0;
    int bscore = 0;
    Renderer purpleGoalRenderer;
    bool proundwin = false;
    bool broundwin = false;
    Team lastHitter;

    //private int resetTimer;
    public int MaxEnvironmentSteps;

    void Start()
    {

        // Used to control agent & ball starting positions
        blueAgentRb = blueAgent.GetComponent<Rigidbody>();
        purpleAgentRb = purpleAgent.GetComponent<Rigidbody>();
        ballRb = ball.GetComponent<Rigidbody>();

        GetComponent<AudioSource>().playOnAwake = false;
        // Starting ball spawn side
        // -1 = spawn blue side, 1 = spawn purple side
        var spawnSideList = new List<int> { -1, 1 };
        ballSpawnSide = spawnSideList[Random.Range(0, 2)];
        agentside = 1;

        // Render ground to visualise which agent scored
        blueGoalRenderer = blueGoal.GetComponent<Renderer>();
        purpleGoalRenderer = purpleGoal.GetComponent<Renderer>();
        RenderersList.Add(blueGoalRenderer);
        RenderersList.Add(purpleGoalRenderer);

        volleyballSettings = FindObjectOfType<VolleyballSettings>();

        ResetScene();
    }

    /// <summary>
    /// Tracks which agent last had control of the ball
    /// </summary>
    public void UpdateLastHitter(Team team)
    {
        lastHitter = team;
    }

    /// <summary>
    /// Resolves scenarios when ball enters a trigger and assigns rewards.
    /// Example reward functions are shown below.
    /// To enable Self-Play: Set either Purple or Blue Agent's Team ID to 1.
    /// </summary>
    public void ResolveEvent(Event triggerEvent)
    {
        switch (triggerEvent)
        {
            case Event.HitOutOfBounds:
                if (lastHitter == Team.Blue)
                {
                    if (proundwin == false)
                    {
                        proundwin = true;
                        GetComponent<AudioSource>().clip = crowdcheer;
                        GetComponent<AudioSource>().Play();
                        pscore++;
                        purplescore.text = pscore.ToString();
                        Invoke("ResetScene", 3);
                    }
                }
                else if (lastHitter == Team.Purple)
                {
                    if (broundwin == false)
                    {
                        broundwin = true;
                        GetComponent<AudioSource>().clip = crowdcheer;
                        GetComponent<AudioSource>().Play();
                        bscore++;
                        bluescore.text = bscore.ToString();
                        Invoke("ResetScene", 3);
                    }
                }

                // end episode
                blueAgent.EndEpisode();
                purpleAgent.EndEpisode();
                break;

            case Event.HitBlueGoal:
                // blue wins
                // blueAgent.AddReward(1f);
                // purpleAgent.AddReward(-1f);
                
                // turn floor blue
                StartCoroutine(GoalScoredSwapGroundMaterial(volleyballSettings.blueGoalMaterial, RenderersList, .5f));

                // end episode
                blueAgent.EndEpisode();
                purpleAgent.EndEpisode();
                if (proundwin == false && broundwin == false)
                {
                    broundwin = true;
                    GetComponent<AudioSource>().clip = crowdcheer;
                    GetComponent<AudioSource>().Play();
                    bscore++;
                    bluescore.text = bscore.ToString();
                    if (bscore >= 5)
                    {
                        win(bluename, Color.blue);
                    }
                    else
                        Invoke("ResetScene", 3);
                }
                break;

            case Event.HitPurpleGoal:
                // purple wins
                // purpleAgent.AddReward(1f);
                // blueAgent.AddReward(-1f);
                
                // turn floor purple
                StartCoroutine(GoalScoredSwapGroundMaterial(volleyballSettings.purpleGoalMaterial, RenderersList, .5f));

                // end episode
                blueAgent.EndEpisode();
                purpleAgent.EndEpisode();
                if (proundwin == false && broundwin == false)
                {
                    proundwin= true;
                    GetComponent<AudioSource>().clip = crowdcheer;
                    GetComponent<AudioSource>().Play();
                    pscore++;
                    purplescore.text = pscore.ToString();
                    if (pscore >= 5){
                        win(purplename,Color.magenta);
                    }
                    else
                        Invoke("ResetScene", 3);
                }
                
                break;

            case Event.HitIntoBlueArea:
                if (lastHitter == Team.Purple)
                {
                    purpleAgent.AddReward(1);
                }
                break;

            case Event.HitIntoPurpleArea:
                if (lastHitter == Team.Blue)
                {
                    blueAgent.AddReward(1);
                }
                break;
        }
    }

    /// <summary>
    /// Changes the color of the ground for a moment.
    /// </summary>
    /// <returns>The Enumerator to be used in a Coroutine.</returns>
    /// <param name="mat">The material to be swapped.</param>
    /// <param name="time">The time the material will remain.</param>
    IEnumerator GoalScoredSwapGroundMaterial(Material mat, List<Renderer> rendererList, float time)
    {
        foreach (var renderer in rendererList)
        {
            renderer.material = mat;
        }

        yield return new WaitForSeconds(time); // wait for 2 sec

        foreach (var renderer in rendererList)
        {
            renderer.material = volleyballSettings.defaultMaterial;
        }

    }


    public void win(string winner, Color c)
    {
        winbox.GetComponentInChildren<Text>().text = winner + " is the winner !!";
        winbox.GetComponentInChildren<Text>().color = c;
        winbox.SetActive(true);
        GetComponent<AudioSource>().clip = wiiiin;
        GetComponent<AudioSource>().Play();
        Invoke("EndGame", 3);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("gameover");
    }
    /// <summary>
    /// Reset agent and ball spawn conditions.
    /// </summary>
    public void ResetScene()
    {
           
        lastHitter = Team.Default; // reset last hitter

        foreach (var agent in AgentsList)
        {
            // randomise starting positions and rotations
            var randomPosX = 0f;// Random.Range(-2f, 2f);
            var randomPosZ = 2f;// Random.Range(-2f, 2f);
            var randomPosY = 1f;// Random.Range(0.5f, 0.6f); // depends on jump height
            var randomRot = 0;

            agent.transform.eulerAngles = new Vector3(0, randomRot, 0);

            agentside = -1 * agentside;

            if (agentside == -1)
            {
                agent.transform.localPosition = new Vector3(randomPosX, randomPosY, randomPosZ);
            }
            else if (agentside == 1)
            {
                agent.transform.localPosition = new Vector3(randomPosX, randomPosY, -1 * randomPosZ);
            }

            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        // reset ball to starting conditions
        ResetBall();
    }

    /// <summary>
    /// Reset ball spawn conditions
    /// </summary>
    void ResetBall()
    {
        var randomPosX = 0f;// Random.Range(-2f, 2f);
        var randomPosZ = 8f;// Random.Range(6f, 10f);
        var randomPosY = 15f;// Random.Range(6f, 8f);

        // alternate ball spawn side
        // -1 = spawn blue side, 1 = spawn purple side
        ballSpawnSide = -1 * ballSpawnSide;

        if (ballSpawnSide == -1)
        {
            ball.transform.localPosition = new Vector3(randomPosX, randomPosY, randomPosZ);
        }
        else if (ballSpawnSide == 1)
        {
            ball.transform.localPosition = new Vector3(randomPosX, randomPosY, -1 * randomPosZ);
        }
        ball.GetComponentInChildren<TrailRenderer>().Clear();
        ballRb.angularVelocity = Vector3.zero;
        ballRb.velocity = Vector3.zero;
        proundwin = false;
        broundwin = false;
    }
}

