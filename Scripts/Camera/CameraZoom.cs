using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class CameraZoom : MonoBehaviour
{
    [SerializeField] private HarpoonShooter _shooter;

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomFactor = 0.65f;
    [SerializeField] private float _zoomTime = 0.25f;
    [SerializeField] private Ease _zoomEase = Ease.OutQuad;

    private CinemachineCamera _cinemachine;
    private float _baseOrthographicSize;
    private float _currentTargetOrthographicSize;
    private Tweener _zoomTween;

    private void Awake()
    {
        _cinemachine = GetComponent<CinemachineCamera>();

        _baseOrthographicSize = _cinemachine.Lens.OrthographicSize;
        _currentTargetOrthographicSize = _baseOrthographicSize;
    }

    private void Update()
    {
        if (_shooter == null || _cinemachine == null)
            return;

        bool zoomActive = _shooter.IsAiming;

        float targetOrthographicSize = zoomActive
            ? _baseOrthographicSize * _zoomFactor
            : _baseOrthographicSize;

        if (targetOrthographicSize == _currentTargetOrthographicSize)
            return;

        _currentTargetOrthographicSize = targetOrthographicSize;

        // 이전 트윈 정리
        _zoomTween?.Kill();

        // DOTween으로 OrthographicSize 트윈 (부드러운 줌 효과)
        _zoomTween = DOTween.To(
                () => _cinemachine.Lens.OrthographicSize,
                value =>
                {
                    var lens = _cinemachine.Lens;
                    lens.OrthographicSize = value;
                    _cinemachine.Lens = lens;
                },
                targetOrthographicSize,
                _zoomTime
            )
            .SetEase(_zoomEase)
            .SetUpdate(true); // unscaled time (타임슬로우에 영향 안 받게)
    }
}
