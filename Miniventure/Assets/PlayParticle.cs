using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    public ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
