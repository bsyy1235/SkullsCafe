// ******------------------------------------------------------******
// FillCupHelper.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******
using UnityEngine;
using System.Collections;
namespace PW
{

    public class FillCupHelper : MonoBehaviour
    {

        Collider m_Collider;

        BewerageMaker m_Machine;

        public Transform fluid;

        public 


        void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;
            if (fluid != null)
                fluid.gameObject.SetActive(false);
        }

        public void SetMachine(BewerageMaker maker)
        {
            m_Machine = maker;
        }

        
        public void DoFill(float amount)
        {
            Debug.Log("Doing the filling");
            StartCoroutine(FillAnimation(amount));
        }
        
        IEnumerator FillAnimation(float amount)
        {
            fluid.gameObject.SetActive(true);
            float timeAmount = amount;
            float totalDist = 0- fluid.transform.localPosition.y;
            float totalScale = fluid.transform.localScale.y;
            if (amount <= 0.0001f)
            {
                fluid.transform.position += new Vector3(0f, totalDist, 0f);
                fluid.transform.localScale = Vector3.one;

            }
            while (timeAmount > 0)
            {
                var timePassed = Time.deltaTime;
                fluid.transform.position += new Vector3(0f, timePassed * totalDist / amount, 0f);
                if (1 - totalScale > 0)
                {
                    var scaleNow = timePassed * 1f / amount;
                    fluid.transform.localScale+= Vector3.one * scaleNow;
                    if (fluid.transform.localScale.y > 1)
                        fluid.transform.localScale = Vector3.one;
                    totalScale = fluid.transform.localScale.y;
                }
                timeAmount -= timePassed;
                yield return null;
            }
                fluid.transform.localScale = Vector3.one;
        }

        public void FillEnded()
        {
            m_Collider.enabled = true;

            gameObject.tag = "item";

            Rigidbody rb = GetComponent<Rigidbody>();
            if(rb == null)
                rb = gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = true; // 잡을 시 false로 변경됨
            rb.useGravity = true;

            if(m_Machine != null)
            {
                m_Machine.UnSetTheCup();
            }
        }
    }
}
