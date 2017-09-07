using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public GameObject bladetrailPrefab;
    public float minCuttingVelocity = .001f; 

    Rigidbody2D rb;
    Camera cam;
    CircleCollider2D circleCollider;
    Vector2 privousPosition;
    GameObject currentBladeTrail;

    bool isCutting;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.circleCollider = GetComponent<CircleCollider2D>();
        this.cam = Camera.main;
    }
     
     // Update is called once per frame
    void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if(isCutting)
        {
            UpdateCut();
        }
	}

    private void UpdateCut()
    {
        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        this.rb.position = newPosition;

        float velocity = (newPosition - privousPosition).magnitude * Time.deltaTime;

        this.circleCollider.enabled = velocity > minCuttingVelocity ? true : false;

        this.privousPosition = newPosition;
    }

    private void StartCutting()
    {
        this.isCutting = true;
        this.currentBladeTrail = Instantiate(bladetrailPrefab, transform);
        this.privousPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        this.circleCollider.enabled = true;
    }

    private void StopCutting()
    {
        this.isCutting = false;
        this.currentBladeTrail.transform.SetParent(null);

        Destroy(currentBladeTrail, 2f);
        this.circleCollider.enabled = false;
    }
}
