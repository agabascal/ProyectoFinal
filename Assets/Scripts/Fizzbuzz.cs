using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fizzbuzz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 101; i++)
        {
            if (i % 15 == 0)
            {
                Debug.Log("fizzBuzz");
            }
            else
            {

                if (i % 3 == 0)
                {
                    Debug.Log("fizz");
                }
                if (i % 5 == 0)
                {
                    Debug.Log("buzz");
                }
                else if (i % 3 != 0 && i % 5 != 0)
                {
                    Debug.Log(i);
                }
            }

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
