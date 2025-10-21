using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class InputManager : MonoBehaviour
{
    private SceneController _sceneController;
    private Controls.GameActions _controls;

    private Coroutine _restartCoroutine;
    private bool _isRestarting = false;

    [SerializeField]
    private GameObject _restartUI;
    [SerializeField]
    private Image _restartFill;
    [SerializeField, Range(1.0f, 3.0f)]
    private float _restartHoldTime = 1.5f;

    private IEnumerator Restarter()
    {
        _isRestarting = true;
        float elapsedTime = 0f;

        while (elapsedTime < _restartHoldTime)
        {
            elapsedTime += Time.deltaTime;
            _restartFill.fillAmount = elapsedTime / _restartHoldTime;
            yield return null;
        }

        _isRestarting = false;
        _sceneController.OpenGameScene();
    }

    private void OnRestartPerformed(InputAction.CallbackContext obj)
    {
        if (_isRestarting) return;

        _restartUI.SetActive(true);
        _restartCoroutine = StartCoroutine(Restarter());
    }

    private void OnRestartCancelled(InputAction.CallbackContext obj)
    {
        if (!_isRestarting) return;

        if (_restartCoroutine != null)
        {
            StopCoroutine(_restartCoroutine);
            _restartCoroutine = null;
        }

        _isRestarting = false;
        _restartFill.fillAmount = 0f;
        _restartUI.SetActive(false);
    }

    private void Start()
    {
        _controls.Restart.performed += OnRestartPerformed;
        _controls.Restart.canceled += OnRestartCancelled;
        _restartFill.fillAmount = 0f;
        _restartUI.SetActive(false);
    }

    private void OnDestroy()
    {
        _controls.Restart.performed -= OnRestartPerformed;
        _controls.Restart.canceled -= OnRestartCancelled;

        if (_restartCoroutine != null)
        {
            StopCoroutine(_restartCoroutine);
        }
    }

    [Inject]
    private void Construct(SceneController sceneController, Controls.GameActions controls)
    {
        _sceneController = sceneController;
        _controls = controls;
    }
}