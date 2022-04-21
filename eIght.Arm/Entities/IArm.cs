using System.Collections.Generic;

namespace eIght.Arm.Entities
{
    public interface IArm : IEnumerable<IArmNode>
    {
        IArmNode this[int index] { get; }
        public IArmNode FirstNode { get; }
        public IArmNode LastNode { get; }

        public void AddNode(int inverseKinematicPriority, AbstractArmNode node);
        public IEnumerable<(int inverseKinematicPriority, AbstractArmNode node)> GetArmNodesIKPriorityOrdered();

    }

}
