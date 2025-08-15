using UnityEngine;

public interface IPunchable
{
    void OnPunch(Vector3 punchDirection, float force);
}
