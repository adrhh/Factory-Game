using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private string word = "Hello world!";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(word);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = new(horizontal, 0, vertical);
        transform.Translate(moveSpeed * Time.deltaTime * moveDir);
    }
}
