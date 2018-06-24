using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRTK;

public class EnemyMovement : MonoBehaviour {

    public List<Transform> points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Rigidbody rigidBody;
    private VRTK_InteractableObject interactableObject;
    private bool physicsEnabled = true;
    private float physics_start_time = 0;

    void Start()
    {
        if (points.Count <= 0)
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    points.Add(GameObject.Find("SpiderPoint" + (i+1).ToString()).transform);
            //}
            points.Add(GameObject.Find("SpiderTarget").transform);
        }

        agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        interactableObject = GetComponent<VRTK_InteractableObject>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        //GotoNextPoint();

        if (interactableObject != null && rigidBody != null)
        {
            interactableObject.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
            interactableObject.InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectReleased);

            //agent.enabled = false;
            //rigidBody.isKinematic = false;
            //rigidBody.WakeUp();
            //physicsEnabled = true;
        }

        physics_start_time = Time.time;
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Count == 0)
            return;


        //// Choose the next point in the array as the destination,
        //// cycling to the start if necessary.
        //destPoint = (destPoint + 1) % points.Length;

        destPoint = Random.Range(0, points.Count);


        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (agent.enabled)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        agent.enabled = false;
    }

    private void ObjectReleased(object sender, InteractableObjectEventArgs e)
    {
        StartPhysics();
    }

    public void StartPhysics()
    {
        interactableObject.isGrabbable = false;
        rigidBody.isKinematic = false;
        rigidBody.WakeUp();
        physicsEnabled = true;
        physics_start_time = Time.time;
    }

    void StopPhysics()
    {
        physicsEnabled = false;
        rigidBody.isKinematic = true;
        agent.enabled = true;
        interactableObject.isGrabbable = true;
    }

    //IEnumerable DelayedStopPhysics()
    //{
    //    yield return new WaitForSeconds(2);
    //    StopPhysics();
    //}

    private void FixedUpdate()
    {
        if (physicsEnabled && (rigidBody.IsSleeping() || Time.time - physics_start_time > 2.0f))
        {
            StopPhysics();
        }
        else if (!physicsEnabled)
        {
            // To fix bug
            rigidBody.isKinematic = true;
        }
    }
}
