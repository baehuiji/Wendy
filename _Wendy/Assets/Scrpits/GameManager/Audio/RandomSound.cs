using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField]
    private string[] RondomSound;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RondomPlaylist());
    }


    IEnumerator RondomPlaylist()
    {

        while(true)
        {
            float w = Random.Range(15f, 30f);

            yield return new WaitForSeconds(w);

            int i = Random.Range(0, 8);
            SoundManger.instance.PlaySound(RondomSound[i]);
        }

    }

}
