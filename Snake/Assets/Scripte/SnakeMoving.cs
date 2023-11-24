using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class SnakeMoving : MonoBehaviour
{
    public Transform SnakeBodyPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float countdwomTime = 5f;
    private int count = 0;
    private bool pressSpace = false; 
    private bool GameOver = false;
    public GameObject StartGame;
    public GameObject EndGame;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Timer;
    
    private List<Transform> SnakeBody = new List<Transform>();
    private Vector2Int input;

    private void Start()
    {
        ResetState();
        EndGame.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame.SetActive(false);
            Time.timeScale = 1;
            pressSpace = true;
        }
        // Only allow turning up or down while moving in the x-axis
        if (GameOver==false && pressSpace)
        {
            if (direction.x != 0f)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    input = Vector2Int.up;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    input = Vector2Int.down;
                    transform.eulerAngles = new Vector3(0, 0, 180);
                }
            }
            // Only allow turning left or right while moving in the y-axis
            else if (direction.y != 0f)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    input = Vector2Int.right;
                    transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    input = Vector2Int.left;
                    transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
            countdwomTime -= 1 * Time.deltaTime;
            Timer.text = "Timer \n" + countdwomTime.ToString("#.##");
            Score.text = "Your Score \n" + count.ToString();
        }
        if (countdwomTime < 0 || GameOver == true)
        {
            Debug.Log("aaaaaaa");
            EndGame.SetActive(true);
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.R) )
            {
                gameover();
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (GameOver == false && pressSpace)
        {
            // Set the new direction based on the input
            if (input != Vector2Int.zero)
            {
                direction = input;
            }

            // Set each SnakeBody's position to be the same as the one it follows.
            for (int i = SnakeBody.Count - 1; i > 0; i--)
            {
                SnakeBody[i].position = SnakeBody[i - 1].position;
                SnakeBody[i].eulerAngles = SnakeBody[i - 1].eulerAngles;
            }

            // Move the snake in the direction it is facing
            // Round the values to ensure it aligns to the grid
            int x = Mathf.RoundToInt(transform.position.x) + direction.x;
            int y = Mathf.RoundToInt(transform.position.y) + direction.y;
            transform.position = new Vector2(x, y);

            // Set the next update time based on the speed
        }
    }

    public void Grow()// add SnakeBody to Snake
    {
        Transform segment = Instantiate(SnakeBodyPrefab);
        segment.position = SnakeBody[SnakeBody.Count - 1].position;
        SnakeBody.Add(segment);
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = Vector3.zero;
        countdwomTime = 5;
        count = 0;
        // Start at 1 to skip destroying the head
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            Destroy(SnakeBody[i].gameObject);
        }

        // Clear the list but add back this as the head
        SnakeBody.Clear();
        SnakeBody.Add(transform);
        StartGame.SetActive(true);
        Time.timeScale = 0;
        EndGame.SetActive(false);
        GameOver = false;
    }
    
    public bool CheckPosition(int x, int y) // To Put Apples in Different Positon of Snake
    {
        foreach (Transform segment in SnakeBody)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            Grow();
            count++;
            countdwomTime = 5;
        }
        else if (other.gameObject.CompareTag("SnakeBody"))
        {
            GameOver = true;
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            GameOver = true;
        }
    }
    

    void gameover()
    {
        Debug.Log("Game Over");
        GameOver = true;
        pressSpace = false;
        ResetState();
    }
    
    
}
