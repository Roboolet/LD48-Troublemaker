using System.Collections;
using System.Collections.Generic;
using Assets.Code.Enemy;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] particle;
    [SerializeField] GameObject policeman;
    public List<Policeman> police = new List<Policeman>();
    [SerializeField] Vector2[] policeSpawns;

    [SerializeField] TextMeshProUGUI scoretext;
    public GameObject deathpanel;

    int score;

    public static GameManager main { get; private set; }
    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        AddScore(30);
    }

    public void AggroPolice()
    {
        foreach(Policeman p in police)
        {
            p.ai = AIState.aggro;
        }
    }

    public void RespawnPolice(Policeman p)
    {
        police.Remove(p);
        Instantiate(policeman, policeSpawns[Random.Range(0, policeSpawns.Length)], Quaternion.identity);
    }

    public void AddScore(int amount)
    {
        score += amount;
        //update UI
        scoretext.text = score + "\nDays prison time";
    }

    public void RestartScene()
    {
        deathpanel.SetActive(false);
        score = 0;
        police = new List<Policeman>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AddScore(30);
    }

    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Create a particle at the specified location
    /// </summary>
    public void Particle(Vector2 pos, int particleID)
    {
        Instantiate(particle[particleID], pos, Quaternion.identity);
    }
}
