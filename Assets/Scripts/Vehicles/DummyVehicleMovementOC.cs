using UnityEngine;

namespace Assets.Scripts.Vehicles
{
    public class DummyVehicleMovementOC : MonoBehaviour
    {
        public TimeProviderGo TimeProvider;
        public float FlightSpeed;
        public float FlightRadius;
        private float _currentFlightAngle;
        private Vector2 _centerAnchor;

        void Start()
        {
            TimeProvider = FindObjectOfType<TimeProviderGo>(); //TODO remporary bad solution
            _centerAnchor = new Vector2(transform.position.x, transform.position.z);
        }

        void Update()
        {
            _currentFlightAngle = Mathf.Repeat(_currentFlightAngle + FlightSpeed*TimeProvider.DeltaTime, 2*Mathf.PI);

            var flatPosition =  _centerAnchor + MathUtils.PolarToCartesian(new Vector2(FlightRadius, _currentFlightAngle));
            transform.position = new Vector3(flatPosition.x, transform.position.y, flatPosition.y);
        }
    }
}