using System;

[Serializable]
public class PlayerData
{
    public int currentHealth;
    public int currentWeaponIndex;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public PlayerData(PlayerController player)
    {
        position = new SerializableVector3(player.transform.position);
        rotation = new SerializableQuaternion(player.transform.rotation);
    }
}
