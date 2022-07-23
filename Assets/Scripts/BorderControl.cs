using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderControl : MonoBehaviour
{
    public Transform partnerBorder;

    public Vector3 GetDisplacement()
    {
        return partnerBorder.position - transform.position;
    }
}

public class BorderTimer
{
    public struct BorderTimerInfo
    {
        public GameObject border;
        public float timeLeft;
    }

    private List<BorderTimerInfo> times = new List<BorderTimerInfo>();
    public bool Contains(GameObject border)
    {
        foreach(BorderTimerInfo info in times)
        {
            if(info.border == border)
            {
                return true;
            }
        }
        return false;
    }
    public float minimumTime = 0.3f;

    public void Add(GameObject newBorder)
    {
        BorderTimerInfo borderTimerInfo = new BorderTimerInfo();
        borderTimerInfo.border = newBorder;
        borderTimerInfo.timeLeft = minimumTime;
        times.Add(borderTimerInfo);
    }

    public void Run()
    {
        for(int i = 0; i < times.Count; i++)
        {
            BorderTimerInfo info = times[i];
            info.timeLeft -= Time.deltaTime;
            times[i] = info;
        }
        times.RemoveAll(x => x.timeLeft <= 0);
    }
}
