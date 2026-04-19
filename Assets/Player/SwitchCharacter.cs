using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    private GameObject cameraManager;

    void Start()
    {
        _switchAction = new InputAction("Switch", binding: "<Keyboard>/y");
        _switchAction.started += _ => { _holdTimer = 0f; _hasReturned = false; };
        _switchAction.canceled += OnReleased;
        _switchAction.Enable();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) PossessPlayer(player);

        cameraManager = GameObject.FindGameObjectWithTag("MainCamera");
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
        CinemachineTargetGroup group = cameraManager.GetComponentInChildren<CinemachineTargetGroup>();

        if (_sortedPikmin.Count == 0)
            BuildSortedPikminList();

        if (_sortedPikmin.Count == 0) return;

        // Remove any null or destroyed pikmin from the list
        _sortedPikmin.RemoveAll(p => p == null);

        if (_sortedPikmin.Count == 0) return;

        this.gameObject.GetComponent<SignalController>().Deactivate();

        _currentPikminIndex = (_currentPikminIndex + 1) % _sortedPikmin.Count;
        GameObject currentPikmin = _sortedPikmin[_currentPikminIndex];

        if (currentPikmin == null) return;

        GetComponent<Rigidbody2D>().linearVelocityX = 0;
        PossessPlayer(currentPikmin);
        this.gameObject.GetComponent<SignalController>().LittleDude = currentPikmin;

        if (gameObject.CompareTag("Player"))
        {
            if (group.Targets.Count > 1)
                group.Targets.RemoveRange(1, group.Targets.Count - 1);

            this.gameObject.GetComponent<SignalController>().Activate();

            if (cameraManager)
                group.AddMember(currentPikmin.transform, 1f, 0.5f);
        }
    }

    public void ReturnToPlayer()
    {
        _sortedPikmin.Clear();
        _currentPikminIndex = -1;

        CinemachineTargetGroup group = cameraManager.GetComponentInChildren<CinemachineTargetGroup>();
        if (group.Targets.Count > 1)
            group.Targets.RemoveRange(1, group.Targets.Count - 1);

        this.gameObject.GetComponent<SignalController>().Deactivate();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) PossessPlayer(player);
    }

    void PossessPlayer(GameObject target)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Picmin"))
        {
            obj.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            Unpossess(obj);
        }

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