using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    public static UpdateScore instance;


    public TextMeshProUGUI ScoreTxt;
    public int Score = 0;
    

    private void Awake()
    {
        instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreTxt.text = "Score: " + Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore() { 
        Score++;   
        ScoreTxt.text = "Score: " + Score.ToString();   
    }
}
