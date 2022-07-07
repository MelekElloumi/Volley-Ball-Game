 using UnityEngine;
 using System.Collections;
 
  //Add this Script Directly to The Death Zone
 public class volleyballaudio : MonoBehaviour
{
    public AudioClip[] audioSources;
    public AudioClip audio2;
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
    }

    void OnCollisionEnter(Collision collinfo)  //Plays Sound Whenever collision detected
    {


        if (collinfo.collider.CompareTag("blueAgent") || collinfo.collider.CompareTag("purpleAgent"))
        {
            GetComponent<AudioSource>().clip = audioSources[Random.Range(0, audioSources.Length)];
            GetComponent<AudioSource>().Play();
        }
        if (collinfo.collider.CompareTag("boundary") || collinfo.collider.CompareTag("purpleBoundary") || collinfo.collider.CompareTag("blueBoundary") || collinfo.collider.CompareTag("wall"))
        {
            GetComponent<AudioSource>().clip = audio2;
            GetComponent<AudioSource>().Play();
        }
    }
    // Make sure that deathzone has a collider, box, or mesh.. ect..,
    // Make sure to turn "off" collider trigger for your deathzone Area;
    // Make sure That anything that collides into deathzone, is rigidbody;
}
