using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eIght.Arm.Entities
{
    public class Arm : IArm
    {
        #region PRIVATE FIELDS
        private IList<AbstractArmNode> _armNodes = new List<AbstractArmNode>();
        private Dictionary<int, AbstractArmNode> _armNodesPriority = new Dictionary<int, AbstractArmNode>();

        #endregion

        #region IArm
        public IArmNode this[int index]
        {
            get
            {
                return _armNodes[index];

            }
        }

        public IArmNode FirstNode => _armNodes[0];
        private AbstractArmNode _lastNode => _armNodes[_armNodes.Count() - 1];
        public IArmNode LastNode => _lastNode;

        public void AddNode(int inverseKinematicPriority, AbstractArmNode node)
        {
            if (_armNodesPriority.ContainsKey(inverseKinematicPriority))
            {
                throw new System.Exception($"Node priority {inverseKinematicPriority} is already declared.");

            }

            if (this.Count() > 0 && this.Contains(node))
            {
                throw new System.Exception("Arm already contains this node.");

            }

            if (this.Count() == 0)
            {
                node.ParentNode = null;

            }
            else
            {
                node.ParentNode = _lastNode;
                _lastNode.ChildNode = node;
            }

            _armNodes.Add(node);
            _armNodesPriority.Add(inverseKinematicPriority, node);

            node.UpdateNodesTransformationMatrix();

        }
        public IEnumerable<(int inverseKinematicPriority, AbstractArmNode node)> GetArmNodesIKPriorityOrdered()
        {
            IEnumerable<(int, AbstractArmNode)> orderedArmNodesPriority = _armNodesPriority.OrderBy(key => key.Key).Select(k => (k.Key, k.Value));

            return orderedArmNodesPriority;

        }

        #endregion

        #region IEnumerable
        public IEnumerator<IArmNode> GetEnumerator()
        {
            return _armNodes.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _armNodes.GetEnumerator();

        }

        #endregion

    }

}
