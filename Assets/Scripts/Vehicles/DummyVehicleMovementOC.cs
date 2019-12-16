using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class DummyVehicleMovementOC : MonoBehaviour
    {
        public float FlightSpeed;
        public float FlightRadius;
        private float _currentFlightAngle;
        private Vector2 _centerAnchor;

        void Start()
        {
            _centerAnchor = new Vector2(transform.position.x, transform.position.z);
        }

        void Update()
        {
            _currentFlightAngle = Mathf.Repeat(_currentFlightAngle + FlightSpeed*Time.deltaTime, 2*Mathf.PI);

            var flatPosition =  _centerAnchor + MathUtils.PolarToCartesian(new Vector2(FlightRadius, _currentFlightAngle));
            transform.position = new Vector3(flatPosition.x, transform.position.y, flatPosition.y);
        }
    }
}