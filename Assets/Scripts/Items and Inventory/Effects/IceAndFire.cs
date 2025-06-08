using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/IceAndFire")]
public class IceAndFire : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;

    [FormerlySerializedAs("velocity")] [SerializeField]
    private float xVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;
        bool thirdAttack = player.GetComponent<Player>().PrimaryAttackState.comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire =
                Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
        }
    }
}