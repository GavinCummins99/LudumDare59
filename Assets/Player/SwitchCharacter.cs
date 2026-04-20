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
    private const float MaxDistance = 20f;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 referencePos = player.transform.position;

        GameObject[] pikmin = GameObject.FindGameObjectsWithTag("Picmin");
        _sortedPikmin = new List<GameObject>();
        foreach (GameObject p in pikmin)
        {
            if (Vector3.Distance(referencePos, p.transform.position) <= MaxDistance)
                _sortedPikmin.Add(p);
        }

        _sortedPikmin.Sort((a, b) =>
        {
            float distA = Vector3.Distance(referencePos, a.transform.position);
            float distB = Vector3.Distance(referencePos, b.transform.position);
            return distA.CompareTo(distB);
        });
    }

    void CycleToNextPikmin()
    {
        CinemachineTargetGroup group = cameraManager.GetComponentInChildren<CinemachineTargetGroup>();

        GameObject previousPikmin = (_currentPikminIndex >= 0 && _currentPikminIndex < _sortedPikmin.Count)
            ? _sortedPikmin[_currentPikminIndex]
            : null;

        // Stop whoever is currently being controlled, not just the base player
        if (_currentPlayer != null)
        {
            Rigidbody2D currentRb = _currentPlayer.GetComponent<Rigidbody2D>();
            if (currentRb != null) currentRb.linearVelocity = Vector2.zero;
        }

        BuildSortedPikminList();

        if (_sortedPikmin.Count == 0) return;

        int resumeIndex = previousPikmin != null ? _sortedPikmin.IndexOf(previousPikmin) : -1;
        _currentPikminIndex = (resumeIndex + 1) % _sortedPikmin.Count;

        GameObject currentPikmin = _sortedPikmin[_currentPikminIndex].gameObject;

        PossessPlayer(currentPikmin);

        // Always update signal on the player only
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SignalController sc = player.GetComponent<SignalController>();
            sc.Deactivate();
            sc.LittleDude = currentPikmin;
            sc.Activate();
        }

        if (gameObject.CompareTag("Player"))
        {
            if (group.Targets.Count > 1)
                group.Targets.RemoveRange(1, group.Targets.Count - 1);

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
        
        // Deactivate signal on player only
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.GetComponent<SignalController>()?.Deactivate();

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
        obj.GetComponent<Strong>()?.NotifyUnpossessed();

        Player2D p2d = obj.GetComponent<Player2D>();
        if (p2d != null) p2d.Possesed = false;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Animator anim = obj.GetComponentInChildren<Animator>();
        if (anim != null) anim.SetBool("Walking", false);

        Camera cam = obj.GetComponentInChildren<Camera>(true);
        if (cam != null) cam.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _switchAction.Dispose();
    }
}