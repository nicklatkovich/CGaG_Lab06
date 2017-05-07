using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CGaG.Lab06 {
    public static class Utils {

        public static Tuple<VertexPositionColor[ ] /*vertexList*/, short[ ] /*indices*/> BuildFunction(Func<float, float, float> f, Vector2 xRange, Vector2 yRange, Vector2 delta) {
            int xx = (int)Math.Ceiling((xRange.Y - xRange.X) / delta.X);
            int yy = (int)Math.Ceiling((yRange.Y - yRange.X) / delta.Y);
            VertexPositionColor[ ] vertexList = new VertexPositionColor[(xx + 1) * (yy + 1)];
            short[ ] indices = new short[4 * (xx + 1) * (yy + 1)];
            int indice = 0;
            for (int i = 0; i <= xx; i++) {
                for (int j = 0; j <= yy; j++) {
                    float x = xRange.X + i * delta.X;
                    float y = yRange.X + j * delta.Y;
                    float h = f(x, y);
                    vertexList[i * (yy + 1) + j] = new VertexPositionColor(new Vector3(x, h, y), HSVToColor(h * 60f, 1f, 1f));
                    if (i > 0) {
                        indices[indice++] = (short)(i * (yy + 1) + j);
                        indices[indice++] = (short)((i - 1) * (yy + 1) + j);
                    }
                    if (j > 0) {
                        indices[indice++] = (short)(i * (yy + 1) + j);
                        indices[indice++] = (short)(i * (yy + 1) + j - 1);
                    }
                }
            }
            return new Tuple<VertexPositionColor[ ], short[ ]>(vertexList, indices);
        }

        public static void DrawLineList(this Game thread, VertexPositionColor[ ] vertexList, short[ ] indices) {

            VertexBuffer vertexBuffer = new VertexBuffer(thread.GraphicsDevice, typeof(VertexPositionColor), vertexList.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertexList);
            thread.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            thread.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertexList, 0, vertexList.Length, indices, 0, indices.Length / 2);
        }

        public static Vector3 SphereToCart(this Vector3 v) {
            float cos = (float)Math.Cos(MathHelper.ToRadians(v.Z));
            return v.X * new Vector3(
                (float)Math.Cos(MathHelper.ToRadians(v.Y)) * cos,
                (float)Math.Sin(MathHelper.ToRadians(v.Z)),
                (float)Math.Sin(MathHelper.ToRadians(v.Y)) * cos);
        }

        public static void Median(ref float value, float min, float max) {
            if (min > max) {
                throw new Exception( );
            }
            if (value < min) {
                value = min;
            }
            if (value > max) {
                value = max;
            }
        }

        public static Vector2 XY(this Vector3 v) {
            return new Vector2(v.X, v.Y);
        }

        public static Color HSVToColor(float h, float S, float V) {
            float H = h;
            while (H < 0) {
                H += 360;
            };
            while (H >= 360) {
                H -= 360;
            };
            float R, G, B;
            if (V <= 0) {
                R = G = B = 0;
            } else if (S <= 0) {
                R = G = B = V;
            } else {
                float hf = H / 60.0f;
                int i = (int)Math.Floor(hf);
                float f = hf - i;
                float pv = V * (1 - S);
                float qv = V * (1 - S * f);
                float tv = V * (1 - S * (1 - f));
                switch (i) {

                // Red is the dominant color

                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;

                // Green is the dominant color

                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;

                // Blue is the dominant color

                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;

                // Red is the dominant color

                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // The color is not defined, we should throw an error.

                default:
                    //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                    R = G = B = V; // Just pretend its black/white
                    break;
                }
            }
            return new Color(R, G, B);
        }

    }
}
