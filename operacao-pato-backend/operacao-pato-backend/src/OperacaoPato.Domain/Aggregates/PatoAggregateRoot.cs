using System.Collections.Generic;
using OperacaoPato.Domain.Entities;

namespace OperacaoPato.Domain.Aggregates
{
    public class PatoAggregateRoot
    {
        public PatoPrimordial PatoPrimordial { get; private set; }
        private List<Drone> _drones;

        public PatoAggregateRoot(PatoPrimordial patoPrimordial)
        {
            PatoPrimordial = patoPrimordial;
            _drones = new List<Drone>();
        }

        public void AddDrone(Drone drone)
        {
            _drones.Add(drone);
        }

        public IReadOnlyList<Drone> GetDrones()
        {
            return _drones.AsReadOnly();
        }

        public void UpdatePatoPrimordial(PatoPrimordial patoPrimordial)
        {
            PatoPrimordial = patoPrimordial;
        }

        // Additional methods to manage the lifecycle and invariants of PatoPrimordial can be added here
    }
}