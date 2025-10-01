using UnityEngine;

public class chara_move : MonoBehaviour
{
    private float speed = 0.02f; //floatは小数点
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       GoRight();
    }

    void GoLeft()
    {
        Vector2 pos = transform.position;
        pos.x -= speed;
        animator.SetInteger("direction", 1);//左を向く
        transform.position = pos;
    }

    void GoRight()
    {
        Vector2 pos = transform.position;
        pos.x += speed;
        animator.SetInteger("direction", 2);//右を向く
        transform.position = pos;
    }

    void GoUp()
    {
        Vector2 pos = transform.position;
        pos.y += speed;
        animator.SetInteger("direction", 3);//後ろを向く
        transform.position = pos;
    }

    void GoDown()
    {
        Vector2 pos = transform.position;
        pos.y -= speed;
        animator.SetInteger("direction", 0);//正面を向く
        transform.position = pos;
    }
}