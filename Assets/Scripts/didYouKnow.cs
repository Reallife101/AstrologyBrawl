using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class didYouKnow : MonoBehaviour
{
    [SerializeField] private List<string> tooltips;
    [SerializeField] private float tooltipLifetime = 5f;
    [SerializeField] private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(newTooltip());
    }

    IEnumerator newTooltip()
    {
        text.text = tooltips[Random.Range(0, tooltips.Count)];
        yield return new WaitForSeconds(tooltipLifetime);
        StartCoroutine(newTooltip());
    }

}
