using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public bool isSpectating;
    public float cameraSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        isSpectating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpectating)
        {
            GetComponent<Transform>().position += new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, 0);
        }
    }
}
