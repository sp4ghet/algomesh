using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCamera : MonoBehaviour
{
    public static TheCamera I;

    // Start is called before the first frame update
    void OnEnable()
    {if(I == null) {
            I = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
