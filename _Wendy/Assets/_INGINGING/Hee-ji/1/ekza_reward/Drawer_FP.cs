using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer_FP : MonoBehaviour
{
    //public Transform _start_trans;
    public Transform _end_trans;

    public GameObject real_drawer;
    public GameObject fake_drawer;

    private bool _reward_on = false;
    public float speed = 200f;

    void Start()
    {
        real_drawer.SetActive(false);
    }

    void Update()
    {

    }

    public void RewardActive()
    {
        if (_reward_on)
            return;

        _reward_on = true;

        fake_drawer.SetActive(false);
        real_drawer.SetActive(true);

        StartCoroutine(DraweMove());
    }

    IEnumerator DraweMove()
    {
        while (transform.position.x != _end_trans.position.x)
        {
            float step = 1f * speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _end_trans.position, step);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
