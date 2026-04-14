using System;
using UnityEngine;

public class Click_Manager_M : MonoBehaviour
{
    [SerializeField] private RectTransform disk_transform;
    [SerializeField] private GameObject slide_parent;
    private Vector2 object_pos;
    private Vector2 mouse_pos;              //ドラッグしている時のマウスの座標
    private Vector2 click_pos;              //クリックした時のマウスの座標
    private float start_angle;              //クリックされた時のディスクの角度の保持用
    private Vector2[] start_pos;            //クリックされた時のスライダーの位置の保持用
    private float slide_height;             //スライダーの高さ

    private RectTransform[] slide_child;
    private int child_index = 0;

    private int ignore = 10000;

    //ディスクがクリックされた時
    public void ClickMouseOnDisk()
    {
        //オブジェクトのスクリーン上の位置を取得(ワールド→スクリーン変換)
        object_pos = Camera.main.WorldToScreenPoint(disk_transform.position);
        //クリック時の入力位置(スクリーン座標)
        click_pos = (Vector2)Input.mousePosition - object_pos;
        start_angle = disk_transform.localEulerAngles.z;

        Debug.Log("ディスクをクリック");
    }

    //ディスクがドラッグされた時
    public void DragMouseOnDisk()
    {
        //現在の入力位置(スクリーン座標)
        mouse_pos = (Vector2)Input.mousePosition - object_pos;

        //開始位置と現在位置の角度差を計算(アークタンジェントでベクトル→角度変換)
        float F_angle_start = Mathf.Atan2(click_pos.y, click_pos.x) * Mathf.Rad2Deg;
        float F_angle_current = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        float F_angle_delta = F_angle_current - F_angle_start;

        //計算された角度差をもとにオブジェクトを回転させる
        disk_transform.rotation = Quaternion.Euler(0, 0, start_angle + F_angle_delta);

        Debug.Log("ディスクをドラッグ中");
    }

    //スライダーがクリックされた時
    public void ClickMouseOnSlide()
    {
        //オブジェクトのスクリーン上の位置を取得(ワールド→スクリーン変換)
        object_pos = Camera.main.WorldToScreenPoint(slide_parent.transform.position);
        //クリック時の入力位置(スクリーン座標)
        click_pos = (Vector2)Input.mousePosition - object_pos;

        //スライダーの高さを記憶
        slide_height = object_pos.y;

        //スライダーの子オブジェクト達を取得
        child_index = 0;
        slide_child = new RectTransform[slide_parent.transform.childCount];
        start_pos = new Vector2[slide_parent.transform.childCount];
        foreach (RectTransform child in slide_parent.transform)
        {
            //"SlideChild"tagのオブジェクトとその座標を記憶
            if (child.gameObject.tag == "SlideChild")
            {
                slide_child[child_index] = gameObject.GetComponent<RectTransform>();
                slide_child[child_index] = child;
                start_pos[child_index] = child.anchoredPosition;
                child_index++;
            }
        }

        Debug.Log("スライダーをクリック");
    }

    //スライダーがドラッグされた時
    public void DragMouseOnSlide()
    {
        //現在の入力位置(スクリーン座標)
        mouse_pos = (Vector2)Input.mousePosition - object_pos;
        Vector2 F_move_vec = mouse_pos - click_pos;

        //記憶した子オブジェクト達の移動処理
        for (int i = 0; i < child_index; i++)
        {
            slide_child[i].anchoredPosition = new Vector3(start_pos[i].x + F_move_vec.x, start_pos[i].y, 0);
        }

        Debug.Log("スライダーをドラッグ中");
    }

    public void RevertSlide()
    {
        RectTransform F_parent_rt = slide_parent.GetComponent<RectTransform>();

        Debug.Log("スライダーを離した");
        
    }
}
