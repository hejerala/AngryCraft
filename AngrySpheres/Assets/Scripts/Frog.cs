using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour
{
    public float velocityToDie = 5.0f;
    public float delayForDeath = 0.5f;
    public GameObject onDestroySoundPrefab;
    public GameObject onDestroyParticleSystemPrefab;
    public Color markedForDeath = Color.red;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Ctrl + Shift + M to list the mono behavior methods
    void OnCollisionEnter(Collision collisionInfo)
    {
        //If a collision is detected and its string enough the next code runs
        if (collisionInfo.relativeVelocity.magnitude > velocityToDie)
        {
            Destroy(this.gameObject, delayForDeath);//half second delay before destroy
            gameObject.GetComponentInChildren<Renderer>().material.color = markedForDeath;//Changes the color to show its gonna disappear
            Instantiate(onDestroySoundPrefab, transform.position, transform.rotation);//Plays explosion sound
            Instantiate(onDestroyParticleSystemPrefab, transform.position, transform.rotation);//Plays explosion particle effect
        }
    }

}
