using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localPosition += new Vector3(0, -2, 0);
    }
}
