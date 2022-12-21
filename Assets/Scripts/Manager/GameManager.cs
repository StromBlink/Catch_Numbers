using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilites
{
    public static PlayerManager _PlayerManager => PlayerManager.Instance;
    public static SpawnEditor _SpawnEditor => SpawnEditor.Instance;
    public static GameManager _GameManager => GameManager.Instance;
    public static MovingController _MovingController => MovingController.Instance;

    public static CollisionManager _CollisionManager => CollisionManager.Instance;
    public static UIManager _UIManager => UIManager.Instance;
}
public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>(10);
    public List<GameObject> areaList = new List<GameObject>(10);
    public static GameManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
}
