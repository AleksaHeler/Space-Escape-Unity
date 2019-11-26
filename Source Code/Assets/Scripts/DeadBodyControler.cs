using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyControler : MonoBehaviour {

    public float thrust;

    void Start () {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrust);
    }
}
