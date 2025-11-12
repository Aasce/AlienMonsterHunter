using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.Entities
{
    public interface IHasEntityUI
    {
        public EntityUI UI { get; }
    }
}