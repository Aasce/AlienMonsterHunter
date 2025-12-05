using UnityEngine;

namespace Asce.Core
{
    public abstract class GameComponent : MonoBehaviour
    {
        protected virtual void Reset()
        {
            this.RefReset();
        }

        protected virtual void RefReset()
        {

        }
    }
}
