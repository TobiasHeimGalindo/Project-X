using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{
    public Transform[] playersToFollow; // Array der Spieler, denen die Kamera folgen soll
    public float smoothSpeed = 0.125f; // Geschwindigkeit, mit der die Kamera sich bewegt
    public Vector3 offset; // Offsets, um die Kamera-Position anzupassen

    void LateUpdate()
    {
        if (playersToFollow.Length == 0)
            return; // Wenn keine Spieler zum Folgen vorhanden sind, verlasse die Funktion

        Vector3 desiredPosition = GetCenterPosition() + offset; // Die Position, zu der die Kamera sich bewegen soll
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Weiche Bewegung der Kamera
        transform.position = smoothedPosition; // Setze die Position der Kamera auf die neue Position
    }

    Vector3 GetCenterPosition()
    {
        Vector3 center = Vector3.zero;
        foreach (Transform player in playersToFollow)
        {
            center += player.position; // Addiere die Position jedes Spielers hinzu
        }
        return center / playersToFollow.Length; // Durchschnittliche Position aller Spieler
    }
}
