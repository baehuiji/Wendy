using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class CustomFan : MonoBehaviour
{
    public Mesh generatedMesh;
    public int vc = 0;

    Vector3[] vertices;
    Vector2[] uv;
    int[] indices;

    Vector3[] vertices2;
    Vector2[] uv2;
    int[] indices2;

    [Range(0, 1)]
    public float fill; // = 1;
    float prevfill;

    public float Time_sec; //30초
    float percent; //1 / 30초, speed

    int reverseFan = 0;

    public bool state; //상태

    // - 시계바늘 스크립트
    public HandRotate[] handRot_script;

    private Coroutine coroutine;

    void FillQuadData()
    {
        Array.Copy(vertices, vertices2, 10);
        Array.Copy(uv, uv2, 10);

        fill = Mathf.Clamp01(fill);

        int triCount = (int)(fill * 8.0f);

        // 잔여분이 있다면 버텍스 정보 수정  
        if (triCount < fill * 8.0f)
        {
            float adpfill = -fill + 0.25f;
            triCount += 1;
            float nz = Mathf.Sin(adpfill * Mathf.PI * 2) * 0.5f;
            float nx = Mathf.Cos(adpfill * Mathf.PI * 2) * 0.5f;

            if (Mathf.Abs(nz) > Mathf.Abs(nx))
            {
                float nztemp = Mathf.Sign(nz) * 0.5f;
                nx = nztemp * nx / nz;
                nz = nztemp;
            }
            else
            {
                float nxtemp = Mathf.Sign(nx) * 0.5f;
                nz = nxtemp * nz / nx;
                nx = nxtemp;
            }

            //Debug.Log("TC:" + triCount);

            vertices2[triCount + 1].z = nz;
            vertices2[triCount + 1].x = nx;
            uv2[triCount + 1].x = nx + 0.5f;
            uv2[triCount + 1].y = nz + 0.5f;
        }

        indices2 = new int[3 * triCount];
        Array.Copy(indices, indices2, 3 * triCount);
    }

    Mesh BuildQuadMesh()
    {
        vertices2 = new Vector3[10];
        uv2 = new Vector2[10];

        //0. origin
        vertices = new Vector3[10]{new Vector3(0, 0, 0),
            new Vector3(0, 0, 0.5f),new Vector3(0.5f, 0, 0.5f),new Vector3(0.5f, 0, 0),
            new Vector3(0.5f, 0, -0.5f),new Vector3(0, 0, -0.5f),new Vector3(-0.5f, 0, -0.5f),
            new Vector3(-0.5f, 0, 0),new Vector3(-0.5f, 0, 0.5f),new Vector3(0, 0, 0.5f)};
        uv = new Vector2[10]{ new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 1),new Vector2(1, 1), new Vector2(1, 0.5f),
            new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0, 0),
            new Vector2(0, 0.5f), new Vector2(0, 1), new Vector2(0.5f, 1)};

        indices = new int[24] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 7, 0, 7, 8, 0, 8, 9 };

        FillQuadData();

        Mesh mesh = new Mesh();
        mesh.name = "Generated Mesh";

        mesh.vertices = vertices2;
        mesh.triangles = indices2;
        mesh.uv = uv2;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.Optimize();
        return mesh;
    }

    // Use this for initialization  
    void Start()
    {
        fill = 1;
        RebuildMesh();

        percent = 1 / Time_sec;
        reverseFan = -1;

        state = false;

        handRot_script = GameObject.FindObjectsOfType<HandRotate>();

        coroutine = StartCoroutine(ChangeGauge());
    }

    void RebuildMesh()
    {
        Mesh sm = BuildQuadMesh();
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            mf.mesh = sm;
        }
        prevfill = fill;
    }

    // Update is called once per frame  
    void Update()
    {
    }

    public void Set_fill()
    {
        fill += reverseFan * 0.00139f; //0.033f;//reverseFan * percent * Time.deltaTime;
    }

    public bool Set_reverseFan() //void 
    {
        if (!Check_fill())
            return false;

        if (reverseFan == -1)
        {
            transform.Rotate(0f, 0f, 180f);
            //transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            transform.Rotate(0f, 0f, 180f);
            //transform.rotation = Quaternion.Euler(180f, 0f, 180f);
        }

        return true;
    }

    bool Check_fill()
    {
        if (fill <= 0)
        {
            reverseFan = -reverseFan; // 1 -> -1
            return true;
        }
        else if (fill >= 1)
        {
            reverseFan = -reverseFan; // -1 -> 1
            return true;
        }
        else
            return false;
    }

    IEnumerator ChangeGauge()
    { 
        // - 2.
        while (true)
        {
            if (fill != prevfill)
                RebuildMesh();

            Set_fill();

            yield return new WaitForSeconds(0.1668f); //1초마다 0.00139f

            if (fill <= 0 || fill >= 1)
                break;
        }

        Set_reverseFan();

        // - 시계바늘 움직이기
        handRot_script[0].start_rotate();
        handRot_script[1].start_rotate();

        coroutine = StartCoroutine(ChangeGauge());
    }

    public void cp_is_over()
    {
        StopCoroutine(ChangeGauge());
    }
}