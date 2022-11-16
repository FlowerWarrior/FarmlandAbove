using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] RectTransform warningRT;
    [SerializeField] RectTransform warningDirRT;
    [SerializeField] Vector3 offset = new Vector3(0, 1, 0);

    [SerializeField] GameObject slimePrefab;
    [SerializeField] GrowSlot[] detectableGrowSlots;
    [SerializeField] Transform[] spawnPoints;

    Slime activeSlime = null;
    float screenWidth = 870.3f;
    float screenHeight = 462f;

    private void OnEnable()
    {
        GrowSlot.ReachedPerfectWater += OnReachedPerfectWater;
    }

    private void OnDisable()
    {
        GrowSlot.ReachedPerfectWater -= OnReachedPerfectWater;
    }

    private void OnReachedPerfectWater()
    {
        if (QuestMgr.instance.tutorialInProgress)
            SpawnSlime();
    }

    private void SpawnSlime()
    {
        for (int i = 0; i < detectableGrowSlots.Length; i++)
        {
            if (detectableGrowSlots[i].currentState == GrowSlot.PlantState.Growing)
            {
                Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                activeSlime = Instantiate(slimePrefab, pos, Quaternion.identity, transform).GetComponent<Slime>();
                activeSlime.target = detectableGrowSlots[i];
            }
        }
    }

    private void Update()
    {
        if (activeSlime != null)
        {
            Vector3 targetPos = activeSlime.transform.GetChild(0).position + offset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
            Transform camT = Camera.main.transform;

            if (Vector3.Distance(camT.position, targetPos) > 30)
            {
                warningRT.gameObject.SetActive(false);
                warningDirRT.gameObject.SetActive(false);
                return;
            }

            //if (screenPos.z < 0) // fix when obj behind camera
            //return;

            warningRT.gameObject.SetActive(true);
            warningDirRT.gameObject.SetActive(true);

            warningRT.position = screenPos;

            Vector2 lookPos = warningRT.localPosition;

            if (screenPos.z >= 0 && IsInsideScreen(warningRT.localPosition))
            {
                //warningRT.localPosition = ClampToScreen(warningRT.localPosition);
            }
            else
            {
                Vector3 fwdVector = camT.forward;
                Vector3 upVector = camT.up;
                Vector3 relVector = camT.position - targetPos;

                Vector3 newPos = Camera.main.WorldToScreenPoint(camT.position + Vector3.Normalize(relVector));
                warningRT.position = newPos;
                warningRT.localPosition = -ClampToScreen(warningRT.localPosition);

                if (newPos.z < 0)
                    warningRT.localPosition = -warningRT.localPosition;

                /*
                Vector3 targetPos = activeSlime.transform.GetChild(0).position;
                Vector3 camPos = Camera.main.transform.position;

                float y = (targetPos.y - camPos.y) / Vector3.Distance(targetPos, camPos);
                float x = (targetPos.x - camPos.x) / Vector3.Distance(targetPos, camPos);
                Vector2 pos = new Vector2(x * screenWidth, y * screenHeight);
                warningRT.localPosition = ClampToScreen(pos); */
            }
            

            if (IsInsideScreen(screenPos))
            {
                warningDirRT.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                //float y = lookPos.y - warningRT.localPosition.y;
                //float x = lookPos.x - warningRT.localPosition.x;
                
                //float correctionAngle = 90f;
                //float zRot = Mathf.Atan2(y,x) * 57.296f + correctionAngle;
                //Vector3 rot = new Vector3(0, 0, zRot + correctionAngle);
                
                //warningDirRT.localRotation = Quaternion.Euler(rot);
            }
        }
        else
        {
            warningRT.gameObject.SetActive(false);
            warningDirRT.gameObject.SetActive(false);
        }
    }

    private bool IsInsideScreen(Vector2 pos)
    {
        if (pos.x > -screenWidth && pos.x < screenWidth)
        {
            if (pos.y > -screenHeight && pos.y < screenHeight)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 ClampToScreen(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -screenWidth, screenWidth);
        pos.y = Mathf.Clamp(pos.y, -screenHeight, screenHeight);
        return pos;
    }

    private Vector2 ClampToEdge(Vector2 pos)
    {
        pos = ClampToScreen(pos);
        /*
        if (pos.y > -screenHeight && pos.y < screenHeight)
        {
            if (pos.x >= 0)
            {
                pos.x = screenWidth;
            }
            else
            {
                pos.x = -screenWidth;
            }
        }

        if (pos.x > -screenWidth && pos.x < screenWidth)
        {
            if (pos.y >= 0)
            {
                pos.y = screenHeight;
            }
            else
            {
                pos.y = -screenHeight;
            }
        }*/

        return pos;
    }
}
