using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor.UI;

public class SwitchCharacter : MonoBehaviour
{
    private GameObject _currentPlayer;
    private InputAction _switchAction;
    private float _holdTimer;
    private bool _hasReturned;
    private List<GameObject> _sortedPikmin = new List<GameObject>();
    private int _currentPikminIndex = -1;
    private const float HoldThreshold = 1f;

    void Start()
    {
        _switchAction = new InputAction("Switch", binding: "<Keyboard>/y");
        _switchAction.started += _ => { _holdTimer = 0f; _hasReturned = false; };
        _switchAction.canceled += OnReleased;
        _switchAction.Enable();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) PossessPlayer(player);
    }

    void Update()
    {
        if (_switchAction.IsPressed())
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= HoldThreshold && !_hasReturned)
            {
                _hasReturned = true;
                ReturnToPlayer();
            }
        }
    }

    void OnReleased(InputAction.CallbackContext context)
    {
        if (!_hasReturned)
            CycleToNextPikmin();
    }

    void BuildSortedPikminList()
    {
        GameObject[] pikmin = GameObject.FindGameObjectsWithTag("Picmin");
        _sortedPikmin = new List<GameObject>(pikmin);
        _sortedPikmin.Sort((a, b) =>
        {
            float distA = Vector3.Distance(_currentPlayer.transform.position, a.transform.position);
            float distB = Vector3.Distance(_currentPlayer.transform.position, b.transform.position);
            return distA.CompareTo(distB);
        });
        _currentPikminIndex = -1;
    }

    void CycleToNextPikmin()
    {
        // Build list from main player position if we're on the main player
        if (_sortedPikmin.Count == 0)
            BuildSortedPikminList();

        if (_sortedPikmin.Count == 0) return;

        this.gameObject.GetComponent<SignalController>().Deactivate();
        
        
        _currentPikminIndex = (_currentPikminIndex + 1) % _sortedPikmin.Count;
        PossessPlayer(_sortedPikmin[_currentPikminIndex]);
        this.gameObject.GetComponent<SignalController>().LittleDude = _sortedPikmin[_currentPikminIndex];
        this.gameObject.GetComponent<SignalController>().Activate();
    }

    void ReturnToPlayer()
    {
        _sortedPikmin.Clear();
        _currentPikminIndex = -1;
        
        this.gameObject.GetComponent<SignalController>().Deactivate();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) PossessPlayer(player);
    }

    void PossessPlayer(GameObject target)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Picmin"))
            Unpossess(obj);
        GameObject mainPlayer = GameObject.FindGameObjectWithTag("Player");
        if (mainPlayer != null) Unpossess(mainPlayer);

        Player2D p2d = target.GetComponent<Player2D>();
        if (p2d != null) p2d.Possesed = true;

        Camera cam = target.GetComponentInChildren<Camera>(true);
        if (cam != null) cam.gameObject.SetActive(true);

        _currentPlayer = target;
    }

    void Unpossess(GameObject obj)
    {
        Player2D p2d = obj.GetComponent<Player2D>();
        if (p2d != null) p2d.Possesed = false;

        Camera cam = obj.GetComponentInChildren<Camera>(true);
        if (cam != null) cam.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _switchAction.Dispose();
    }
}