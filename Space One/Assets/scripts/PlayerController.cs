        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float Speed = 3;

    private Rigidbody2D rigidbody;
    private Vector3 direction;
    private Vector3 mousePose;
    
	void Start () {

        this.rigidbody = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void Update () {

        // rotate by mouse
        mousePose = Input.mousePosition;
        direction = Quaternion.LookRotation(mousePose).eulerAngles;
        transform.rotation = Quaternion.LookRotation(mousePose);
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }


}
