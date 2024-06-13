using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{
    public Transform[] players; // Array, um die Spieler zu speichern

    private Vector3 offset; // Offset zwischen Kamera und Spielern

    void Start()
    {
        // Berechne den Durchschnitt der Spielerpositionen als Offset
        Vector3 averagePosition = Vector3.zero;
        foreach (Transform player in players)
        {
            averagePosition += player.position;
        }
        averagePosition /= players.Length;
        offset = transform.position - averagePosition;
    }

    void LateUpdate()
    {
        // Berechne den Durchschnitt der Spielerpositionen
        Vector3 averagePosition = Vector3.zero;
        foreach (Transform player in players)
        {
            averagePosition += player.position;
        }
        averagePosition /= players.Length;

        // Aktualisiere die Kameraposition basierend auf dem Durchschnitt und dem Offset
        transform.position = averagePosition + offset;
    }
}


