using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController trailController;

    public GameObject PanelGameOver;
    public GameObject PanelWin;

    public List<Bird> Birds;
    public List<Enemy> Enemies;

    private Bird _shotBird;
    public BoxCollider2D TapCollider;

    public GameObject birdContainer;
    public GameObject enemyContainer;

    private bool _isGameEnded = false;

    private void Awake()
    {
        Birds.AddRange(birdContainer.GetComponentsInChildren<Bird>());
        Enemies.AddRange(enemyContainer.GetComponentsInChildren<Enemy>());
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }


    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }

        if (Birds.Count <= 0 && Enemies.Count >= 0)
        {
            StartCoroutine(LoseC(2f));
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0 && Birds.Count >= 0)
        {
            _isGameEnded = true;
            StartCoroutine(WinC(2f));
        }
    }

    private IEnumerator LoseC(float second)
    {
        yield return new WaitForSeconds(second);
        PanelGameOver.SetActive(true);
    }

    private IEnumerator WinC(float second)
    {
        yield return new WaitForSeconds(second);
        PanelGameOver.SetActive(false);
        PanelWin.SetActive(true);
    }


    public void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}
