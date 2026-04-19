using UnityEngine;


public class AnimationEventRelay : MonoBehaviour
{
    private Player2D _player;

    void Awake() => _player = GetComponentInParent<Player2D>();

    public void PlaySwoosh(int pipe) => _player.PlaySwoosh(pipe);
    
}