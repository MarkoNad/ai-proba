﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarCollider : MonoBehaviour {

    private int carHit = 0;
    private int pedestrianHit = 0;
    private int wallHit = 0;
    private int streetObjectHit = 0;
    
    int _carHit { get; set; }
    int _pedestrainHit { get; set; }
    int _wallHit { get; set; }
    int _streetObjectHit { get; set; }

    public Text collisionText;

    private AudioSource crashSound;

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("tag name " + col.collider.tag);
        if (col.collider.tag == "Pedestrian")
        {
            
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            PedestrianController pedestrian = col.gameObject.transform.GetComponentInParent<PedestrianController>();
            pedestrian.enabled = false;

            col.collider.attachedRigidbody.AddForce(1, 1, 500, ForceMode.Acceleration);
            
            Renderer renderer = col.gameObject.transform.Find("Cube").GetComponent<Renderer>();
            Color color = renderer.material.color;
            Color blink = Color.red;

            //collision sound
            AudioSource[] sounds = GetComponents<AudioSource>();
            crashSound = sounds[1];
            StartCoroutine(PlaySound(crashSound));

            StartCoroutine(Blink(renderer, color, blink));
            
            pedestrianHit++;

            Debug.Log("Hit a pedestrian!");
        }

        if(col.collider.tag == "AICar")
        {
            
            AICarController aiCar = col.gameObject.GetComponent<AICarController>();
            aiCar.enabled = false;
            
            Renderer renderer = col.gameObject.GetComponent<Renderer>();
            Color color = renderer.material.color;
            Color blink = Color.black;

            //collision sound
            AudioSource[] sounds = GetComponents<AudioSource>();
            crashSound = sounds[0];
            StartCoroutine(PlaySound(crashSound));

            //changing color
            StartCoroutine(Blink(renderer, color, blink));
           
            carHit++;
            
            Debug.Log("Hit a car!");
        }

        if(col.collider.tag == "Wall")
        {
            Renderer renderer = col.gameObject.GetComponent<Renderer>();
            Color color = renderer.material.color;
            Color blink = Color.yellow;

            //collision sound
            AudioSource[] sounds = GetComponents<AudioSource>();
            crashSound = sounds[0];
            StartCoroutine(PlaySound(crashSound));

            StartCoroutine(Blink(renderer, color, blink));

            wallHit++;
            Debug.Log("Hit a wall!");
        }

        if(col.collider.tag == "StreetObject")
        {
            Renderer renderer = col.gameObject.GetComponent<Renderer>();
            Color color = renderer.material.color;
            Color blink = Color.blue;

            //collision sound
            AudioSource[] sounds = GetComponents<AudioSource>();
            crashSound = sounds[0];
            StartCoroutine(PlaySound(crashSound));

            StartCoroutine(Blink(renderer, color, blink));

            streetObjectHit++;
            Debug.Log("Hit a street object!");
        }
    }


    void OnCollisionExit(Collision col)
    {
        if (col.collider.tag == "AICar")
        {
            Wait(1f);
            AICarController aiCar = col.gameObject.GetComponent<AICarController>();
            aiCar.enabled = true;
        }
    }
    IEnumerator Blink(Renderer renderer, Color color, Color blinkColor)
    {
       
        float blink = 0.1f;

        for (int i = 0; i < 3; i++)
        {

            renderer.material.SetColor("_Color", blinkColor);
            yield return new WaitForSeconds(blink);

            renderer.material.SetColor("_Color", color);
            yield return new WaitForSeconds(blink);

        }

        renderer.material.SetColor("_Color", color);

    }

    IEnumerator PlaySound(AudioSource audio)
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
