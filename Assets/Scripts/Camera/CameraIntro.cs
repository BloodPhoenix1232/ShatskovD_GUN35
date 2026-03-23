using UnityEngine;
using Cinemachine;

public class CameraIntro : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _introCamera;
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private float _introDuration = 2f;

    private PlayerController _playerController;
    private PlayerResize _playerResize;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerResize = FindObjectOfType<PlayerResize>();

        if (_playerController != null) _playerController.canMove = false;
        if (_playerResize != null) _playerResize.canResize = false;

        _introCamera.Priority = 10;
        _followCamera.Priority = 0;

        Invoke(nameof(SwitchToFollowCamera), _introDuration);
    }

    private void SwitchToFollowCamera()
    {
        _introCamera.Priority = 0;
        _followCamera.Priority = 10;

        if (_playerController != null) _playerController.canMove = true;
        if (_playerResize != null) _playerResize.canResize = true;
    }
}