using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MVerse.Libs.Regulation
{

    public class PI_RegulatorClass
    {
        protected float minLimit;
        protected float maxLimit;
        protected float minIntegral;
        protected float maxIntegral;

        protected float kp;
        protected float ki;

        protected float lastOutput;
        protected float integral;

        public float LastOutput => lastOutput;


        public PI_RegulatorClass(float kp, float ki, float minlim = float.MinValue, float maxlim = float.MaxValue, float minint = float.MinValue, float maxint = float.MaxValue, float preset = 0)
        {
            minLimit = minlim;
            maxLimit = maxlim;
            

            this.kp = kp;
            this.ki = ki;

            if((kp == 0) || (ki == 0))
            {
                Debug.LogError("KP or KI set to 0");
            }
            else
            {
                SetIntegralLimits(minint, maxint);
                PresetValue(preset);
            }
        }

        public void SetKP(float newKp)
        {
            kp = newKp;
        }

        public void SetKI(float newKi)
        {
            ki = newKi;
        }

        public void SetIntegralLimits(float minint, float maxint)
        {
            minIntegral = minint / ki;
            maxIntegral = maxint / ki;
        }

        public void PresetValue(float preset)
        {
            integral = Mathf.Clamp(preset / ki, minIntegral, maxIntegral);
            lastOutput = preset;
        }

        public float RegulateUponTarget(float target)
        {
            float error = target - lastOutput;

            return RegulateUponError(error);
        }

        public float RegulateUponError(float error)
        {
            float Kpart;
            float Ipart;
            float outputCandidate;

            integral = Mathf.Clamp(integral + error*ki, minIntegral, maxIntegral);

            Kpart = error * kp;
            Ipart = integral;

            outputCandidate = Kpart + Ipart;

            float excessmax = outputCandidate - maxLimit;
            float excessmin = minLimit - outputCandidate;

            if(excessmax > 0)
            {
                integral -= excessmax;
                lastOutput = maxLimit;
            }
            else if(excessmin > 0)
            {
                integral += excessmin;
                lastOutput = minLimit;
            }
            else
            {
                lastOutput = outputCandidate;
            }

            return lastOutput;
        }
    }
}
