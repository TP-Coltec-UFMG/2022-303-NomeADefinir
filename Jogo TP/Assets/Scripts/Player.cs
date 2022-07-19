using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Eu acho que usar um enum pra simular uma "máquina de estados" vai ajudar a ter um controle melhor das ações resultantes do movimento do jogador
    // Da linha 8-26 Tá implementado um sistema pra ficar definido qual que é o estado atual do player, se é no chão, se é deslizando ou se é no ar, sem ter que usar bool; ao invés de bool, usar Enum
    private enum EstadoAtual
    {
        NoChao,
        Deslizando,
        NoAr
    };
    private EstadoAtual estadoAtual;
    
    private void DefineEstadoAtualPlayer(){
        if(Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.down, 0.05f,platforms)){
            estadoAtual = EstadoAtual.Nochao;
        }
        if((Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.left, 0.05f, platforms)) || (Physics2D.BoxCast(player_col.bounds.center, player_col.bounds.size, 0f, Vector2.right, 0.05f, platforms))){
            estadoAtual = EstadoAtual.Deslizando;
        }
        else{
        estadoAtual = EstadoAtual.NoAr;
    }
    
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
        if(estadoAtual == EstadoAtual.NoChao) && (player_rb.velocity.y == 0) && (player_rb.velocity.x == 0))
        {
            direction *= -1;
        }
    }
    private void FixedUpdate()
    {
        DefineEstadoAtualPlayer();
        player_rb.velocity = new Vector2(speed * direction, player_rb.velocity.y);
        if((jump_request) && ((estadoAtual == EstadoAtual.NoChao) || (estadoAtual == EstadoAtual.Deslizando)))
        {
            if(IsSliding())
            {
                direction *= -1;
            }
            player_rb.velocity = new Vector2(player_rb.velocity.x, jump_velocity);
            jump_request = false;
        }
        if(estadoAtual == EstadoAtual.NoChao)
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
