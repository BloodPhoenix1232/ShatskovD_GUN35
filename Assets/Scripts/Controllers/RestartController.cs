using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class RestartController : MonoBehaviour
{
    public GameObject holdIndicator;
    public Image holdProgress;

    private GameControls controls;
    private Coroutine holdCoroutine;
    private float holdDuration = 4f;

    private void Awake()
    {
        controls = new GameControls();

        if (holdIndicator != null)
            holdIndicator.SetActive(true);
    }

    private void OnEnable()
    {
        controls.Game.Restart.started += OnRestartStarted;
        controls.Game.Restart.canceled += OnRestartCanceled;
        controls.Game.Restart.Enable();
    }

    private void OnDisable()
    {
        controls.Game.Restart.started -= OnRestartStarted;
        controls.Game.Restart.canceled -= OnRestartCanceled;
        controls.Game.Restart.Disable();

        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
    }

    private void OnRestartStarted(InputAction.CallbackContext context)
    {
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
        holdCoroutine = StartCoroutine(HoldCoroutine());
    }

    private void OnRestartCanceled(InputAction.CallbackContext context)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        if (holdIndicator != null)
            holdIndicator.SetActive(false);
    }

    private IEnumerator HoldCoroutine()
    {
        if (holdIndicator != null)
        {
            holdIndicator.SetActive(true);
            if (holdProgress != null)
                holdProgress.fillAmount = 0;
        }

        float elapsed = 0;

        while (elapsed < holdDuration)
        {
            elapsed += Time.deltaTime;

            if (holdProgress != null)
                holdProgress.fillAmount = elapsed / holdDuration;

            yield return null;
        }

        RestartGame();
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}