using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem devil;
    [SerializeField] ParticleSystem lovers;
    [SerializeField] ParticleSystem hermit;
    [SerializeField] ParticleSystem magician;
    [SerializeField] ParticleSystem justice;
    [SerializeField] ParticleSystem fool;



    public void doMagician(float delay)
    {
        Debug.Log("Doing magician");
        StartCoroutine(magicianCoroutine(delay));
    }

    public void doHermit(float delay)
    {
        Debug.Log("Doing hermit");
        StartCoroutine(hermitCoroutine(delay));
    }

    public void doLovers(float delay)
    {
        Debug.Log("Doing lovers");
        StartCoroutine(loversCoroutine(delay));
    }

    public void doDevil(float delay)
    {
        Debug.Log("Doing devil");
        StartCoroutine(devilCoroutine(delay));
    }

    IEnumerator devilCoroutine(float delay)
    {
        devil.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        devil.gameObject.SetActive(false);
    }

    IEnumerator loversCoroutine(float delay)
    {
        lovers.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        lovers.gameObject.SetActive(false);
    }

    IEnumerator hermitCoroutine(float delay)
    {
        hermit.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        hermit.gameObject.SetActive(false);
    }

    IEnumerator magicianCoroutine(float delay)
    {
        magician.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        magician.gameObject.SetActive(false);
    }

    public void doFool()
    {
        Debug.Log("Doing fool");
        fool.Play();
    }

    public void doJustice()
    {
        Debug.Log("Doing justice");
        justice.Play();
    }
}
