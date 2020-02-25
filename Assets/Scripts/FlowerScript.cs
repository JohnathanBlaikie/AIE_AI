using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    public GameObject[] flowers = new GameObject[1];
    public GameObject partE;
    public ParticleSystem particleEmitter;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Bee")
        {
            Debug.Log("Bee time");

            //partE.GetComponent<ParticleSystem>().enableEmission = true;
            particleEmitter.GetComponent<ParticleSystem>().Play();
            StartCoroutine(stopParticles());
        }
    }
    IEnumerator stopParticles()
    {
        yield return new WaitForSeconds(.4f);
        //partE.GetComponent<ParticleSystem>().enableEmission = false;
        particleEmitter.GetComponent<ParticleSystem>().Stop();
    }

}
