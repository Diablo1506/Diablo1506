using UnityEngine;

namespace New.Managers
{
    public class ParticleManager : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem PunchFX { get; private set; }
        [field: SerializeField] public ParticleSystem PlusTokenFX { get; private set; }

        public void PlayPunchVFX(Vector3 position)
        {
            if (PunchFX == null)
                return;
            
            var punchVFX = Instantiate(PunchFX, position, Quaternion.identity);
        }

        public void PlayPlusTokenVFX(Vector3 position)
        {
            if (PlusTokenFX == null)
                return;
            
            var plusTokenVFX = Instantiate(PlusTokenFX, position, Quaternion.identity);
        }
    }
}