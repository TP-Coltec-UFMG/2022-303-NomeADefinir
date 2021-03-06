using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D player_rb;
    public float speed;
    private float direction;
    public float jump_velocity;
    private bool jump_request;
    public BoxCollider2D player_col;
    [SerializeField] private LayerMask platforms;
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.down, 0.05f,platforms);
    }
    private bool IsSliding()
    {
        if((Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.left, 0.05f, platforms)) || (Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.right, 0.05f, platforms)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Start()
    {
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }
    private void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            jump_request = true;
            StartCoroutine(QueueJump());
        }
        if((IsGrounded()) && (player_rb.velocity.y == 0) && (player_rb.velocity.x == 0))
        {
            direction *= -1;
        }
    }
    private void FixedUpdate()
    {
        player_rb.velocity = new Vector2(speed * direction, player_rb.velocity.y);
        if((jump_request) && ((IsGrounded()) || (IsSliding())))
        {
            if(IsSliding())
            {
                direction *= -1;
            }
            player_rb.velocity = new Vector2(player_rb.velocity.x, jump_velocity);
            jump_request = false;
        }
        if(IsSliding())
        {
            if(player_rb.velocity.y > 0)
            {
                player_rb.gravityScale = 2.5f;
            }
            else if(player_rb.velocity.y <= 0)
            {
                player_rb.gravityScale = 0.5f;
            }
        }
        else
        {
            if(player_rb.velocity.y < 0)
            {
                player_rb.gravityScale = 2f;
            }
            else if((player_rb.velocity.y > 0) && (!Input.GetKey("space")))
            {
                player_rb.gravityScale = 2.5f;
            }
            else
            {
                player_rb.gravityScale = 1f;
            }
        }
    }
    IEnumerator QueueJump()
    {
        yield return new WaitForSeconds(.25f);
        jump_request = false;
    }
}
