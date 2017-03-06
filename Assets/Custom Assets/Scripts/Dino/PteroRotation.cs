using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Courbe
{
    Circle,
    Bernouilli,
    Hypotrochoide,
    Epitrochoide
}

public class PteroRotation : MonoBehaviour {

    public Transform ptero;
    public float distance = 50f;
    [Tooltip("Rotation (deg/s)")]
    public float speedPtero = 60f;
    public float penchement = 3f;

    public Courbe courbe;

    public float bigR = 2f;
    public float littleR = 1f;

    float angle = 0f;

    void Update()
    {
        //Calcul angle
        angle += speedPtero * Time.deltaTime * Mathf.Deg2Rad;

        switch (courbe)
        {
            case Courbe.Circle:
                Circle();
                break;

            case Courbe.Bernouilli:
                Bernouilli();
                break;

            case Courbe.Hypotrochoide:
                Choide(true);
                break;

            case Courbe.Epitrochoide:
                Choide(false);
                break;
        }
    }

    void Circle()
    {
        //Calcul position
        float cos = distance * Mathf.Cos(angle);
        float sin = distance * Mathf.Sin(angle);

        ptero.localPosition = new Vector3(cos, 0f, sin);

        //Calcul vitesse
        Vector3 forward = new Vector3(-sin, 0f, cos);

        //Calcul acceleration
        Vector3 upwards = new Vector3(-cos, distance * penchement, -sin);

        ptero.localRotation = Quaternion.LookRotation(forward, upwards);
    }

    void Bernouilli()
    {
        //Calcul position
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        float coef = sin / (1f + cos * cos);

        float x = distance * coef;

        ptero.localPosition = new Vector3(x, 0f, cos * x);

        //Calcul vitesse
        float dcoef = cos * (1f + 2f * coef * coef);

        float dx = distance * dcoef;
        float dz = dx * cos - x * sin;

        Vector3 forward = new Vector3(dx, 0f, dz);

        //Calcul acceleration
        float ddcoef = 4f * cos * coef * dcoef - sin * (1f + 2f * coef * coef);

        float ddx = distance * ddcoef;
        float ddz = (ddx - x) * cos - 2f * dx * sin;

        Vector3 upwards = new Vector3(ddx, distance * penchement, ddz);

        ptero.localRotation = Quaternion.LookRotation(forward, upwards);
    }

    void Choide(bool hypo)
    {
        //Calcul position
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        float diff = bigR - littleR;
        float div = diff / littleR;

        float cosR = Mathf.Cos(angle * div);
        float sinR = Mathf.Sin(angle * div);

        float x = diff * cos + distance * cosR;
        float z = diff * sin - distance * sinR;

        ptero.localPosition = new Vector3(x, 0f, z);

        //Calcul vitesse
        float dx = -diff * sin - distance * div * sinR;
        float dz = diff * cos - distance * div * cosR;

        Vector3 forward = new Vector3(dx, 0f, dz);

        //Calcul acceleration
        float ddx = -diff * cos - distance * div * div * cosR;
        float ddz = -diff * sin + distance * div * div * sinR;

        Vector3 upwards = new Vector3(ddx, distance * penchement, ddz);

        ptero.localRotation = Quaternion.LookRotation(forward, upwards);
    }
}
