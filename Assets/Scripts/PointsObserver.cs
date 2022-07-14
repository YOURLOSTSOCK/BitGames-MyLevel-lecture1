
using UnityEngine;

public class PointsObserver : MonoBehaviour
{
    [SerializeField] private Transform[] _observablePoints;
    [SerializeField] private float _lookDelay;

    [SerializeField] private GameObject _fieldOfView;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _playerMask;

    private float _timer = 0f;
    private int _currentObservablePointIndex = 0;

    private void Awake()
    {
        _timer = _lookDelay;
    }

    private void Update()
    {
        LookAtTimerTick();
        LookAtPoint();
    }

    private void LookAtPoint()
    {
        Vector3 distanceToPoint = _observablePoints[_currentObservablePointIndex].transform.position - transform.position;
        Vector3 directionToPoint = distanceToPoint.normalized;

        if (Physics.Raycast(transform.position, directionToPoint, out RaycastHit hit, distanceToPoint.magnitude, _playerMask))
            hit.collider.GetComponent<Player>().Kill();

        

        _fieldOfView.SetActive(Physics.Raycast(transform.position, directionToPoint, distanceToPoint.magnitude, _obstacleMask) == false);
        transform.forward = new Vector3(directionToPoint.x, 0f, directionToPoint.z);

    }

    private void LookAtTimerTick()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            int nextPoint = _currentObservablePointIndex + 1;

            if (nextPoint >= _observablePoints.Length)
                nextPoint = 0;

            _currentObservablePointIndex = nextPoint;
            _timer = _lookDelay;
        }
    }
}
