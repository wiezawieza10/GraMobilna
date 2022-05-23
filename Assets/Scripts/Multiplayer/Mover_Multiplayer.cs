using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public abstract class Mover_Multiplayer : Fighter_Multiplayer
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float ySpeed = 0.55f;
    protected float xSpeed = 0.7f;
    public Animator animator;
    public PhotonView view;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        view = GetComponent<PhotonView>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {

        //Reset MoveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Change animation to 'run' state
        if(moveDelta.x != 0 || moveDelta.y != 0)
        {
            animator.SetBool("running", true);
        }
        else
            animator.SetBool("running", false);
        //Swap sprite direction, wether you're going right or left
        if(this.gameObject.name.Contains("Player"))
            if (moveDelta.x > 0)
                view.RPC("FlipFalse", RpcTarget.AllBuffered);
            else if (moveDelta.x < 0)
                view.RPC("FlipTrue", RpcTarget.AllBuffered);
        else
            if (moveDelta.x > 0)
                view.RPC("EnemyFlipFalse", RpcTarget.AllBuffered);
            else if (moveDelta.x < 0)
                view.RPC("EnemyFlipTrue", RpcTarget.AllBuffered);

        // Add push vector, if any
        moveDelta += pushDirection;

        // Reduce push force every frame
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
        
        // Make sure we can move in this direction, by casting a box there firts, if the box returns null, we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //make this thing move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
