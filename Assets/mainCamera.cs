using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestFairyUnity;

public class mainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
				Application.logMessageReceivedThreaded += CaptureLogThread;
        TestFairy.begin("5b3af35e59a1e074e2d50675b1b629306cf0cfbd");
    }

    // Update is called once per frame
    void Update()
    {
			Debug.Log("I love logging");
    }

		void CaptureLogThread(string message, string stacktrace, LogType type) {
			TestFairy.log(message);
		}
}
