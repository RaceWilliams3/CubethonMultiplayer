using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;
    public bool dead;

    [Header("Info")]
    public float moveSpeed;
    public float Acceleration;
    public float jumpForce;

    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;


    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        dead = false;

        GameManager.instance.players[id - 1] = this;

        if (!photonView.IsMine)
        {
            rig.isKinematic = true;
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
            if (Input.GetKey("a"))
            {
                MoveLeft();
            }
            if (Input.GetKey("d"))
            {
                MoveRight();
            }
        }

        if (GetComponent<Transform>().localPosition.y < 0)
        {
            Die();
        }
    }

    void MoveLeft()
    {
        rig.AddForce(-moveSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    void MoveRight()
    {
        rig.AddForce(moveSpeed * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine && rig.velocity.z < 75)
        {
            rig.AddForce(0, 0, Acceleration * Time.deltaTime);
        }
    }

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 0.7f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine && collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die()
    {
        GetComponentInChildren<cameraController>().isSpectating = true;
        GetComponentInChildren<Transform>().localPosition = new Vector3(0, 10, 0);
        GetComponent<Transform>().localPosition = new Vector3(0, -3, GetComponent<Transform>().localPosition.z);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX |
                                                RigidbodyConstraints.FreezeRotationY |
                                                RigidbodyConstraints.FreezeRotationZ |
                                                RigidbodyConstraints.FreezePositionX |
                                                RigidbodyConstraints.FreezePositionY;
        GetComponent<MeshRenderer>().enabled = false;
        GameManager.instance.alivePlayers--;
        dead = true;
    }

    public Vector3 getPos()
    {
        return this.GetComponent<Transform>().localPosition;
    }
}