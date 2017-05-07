using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGaG.Lab06 {
    public static class LabUtils {

        public static float F1(float x, float y) {
            return (x * x + y * y) / 30f;
        }

        public static float F2(float x, float y) {
            return (x * x - y * y) / 30f;
        }

        public static float F3(float x, float y) {
            float r = (float)Math.Sqrt(Math.Abs(1f - F1(x, y) * 30f));
            if (r < 0.001f) {
                return 10f;
            } else {
                return 10f * (float)Math.Sin(r) / r;
            }
        }

        public static float F4(float x, float y) {
            float r = (float)Math.Sqrt(Math.Abs(1f - F1(x, y) * 30f));
            if (r < 0.001f) {
                return 10f;
            } else {
                return 10f * (float)Math.Sin(r + x) / r;
            }
        }

        public static float F5(float x, float y) {
            return (float)Math.Sqrt(x * x + y * y) + 3f * (float)Math.Cos(Math.Sqrt(x * x + y * y)) - 20f;
        }

    }
}
