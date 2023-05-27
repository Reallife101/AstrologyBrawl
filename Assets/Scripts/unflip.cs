using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unflip : MonoBehaviour
{
    [SerializeField] Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(parent.localScale.x > 0 ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }
}
