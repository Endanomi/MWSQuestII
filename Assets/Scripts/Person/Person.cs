using UnityEngine;

public class Person : MonoBehaviour
{
    private float speed = 0.03f; //floatは小数点
    private Animator animator;
    public string state;

    public PersonProperties properties;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PropertiesCreator propertiesCreator = new PropertiesCreator();
        properties = propertiesCreator.CreatePersonProperties();
        state = "pass";
    }

    void Update()
    {
        Vector2 pos = transform.position;
        if (pos.x >= 30 || pos.y >= 20 || pos.y <= -20)
        {
            Destroy(gameObject);
        }

        switch (state)
        {
            case "pass":
                GoRight();
                break;
            case "reject":
                GoUp();
                break;
            case "drop":
                break;
        }
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