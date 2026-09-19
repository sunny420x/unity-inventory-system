using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Header("Settings")]
    public AudioSource Player_Audio;
    public float stepRate = 0.6f;
    public float stepCoolDown = 0.5f;

    [Header("Sound Clips")]
    public AudioClip footstepIndoor;
    public AudioClip footstepOutdoor;

    void Start()
    {
        Debug.Log("Footsteps Script is working.");
    }


    void Update()
    {
        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f)
        {
            Player_Audio.pitch = 1f + Random.Range(-0.2f, 0.2f);
            Player_Audio.PlayOneShot(footstepIndoor, 0.5f);

            //Check Ground Layer
            Ray ray = new Ray(gameObject.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var distance = Vector3.Distance(hit.transform.position, gameObject.transform.position);
                if(distance < 5.0f)
                {
                    Debug.Log("Ground Layer is: " + hit.transform.gameObject.layer);
                    switch (hit.transform.gameObject.layer)
                    {
                        case 8:
                            Player_Audio.PlayOneShot(footstepIndoor, 0.5f);
                            break;
                        case 9:
                            Player_Audio.PlayOneShot(footstepOutdoor, 0.5f);
                            break;
                    }
                }
            }
            stepCoolDown = stepRate;
        }
    }
}