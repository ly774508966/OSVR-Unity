/// OSVR-Unity Connection
///
/// http://sensics.com/osvr
///
/// <copyright>
/// Copyright 2014 Sensics, Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///     http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// </copyright>

using System;
using UnityEngine;

namespace OSVR
{
    namespace Unity
    {
        /// <summary>
        /// Class of static methods for converting from OSVR math/tracking types to Unity-native data types, including coordinate system change as needed.
        /// </summary>
        public class Math
        {
            public static Vector3 ConvertPosition(OSVR.ClientKit.Vec3 vec)
            {
                /// Convert to left-handed
                return new Vector3((float)vec.x, (float)vec.y, (float)-vec.z);
            }

            public static Vector2 ConvertPosition(OSVR.ClientKit.Vec2 vec)
            {
                return new Vector2((float)vec.x, (float)vec.y);
            }

            public static Quaternion ConvertOrientation(OSVR.ClientKit.Quaternion quat)
            {
                /// Wikipedia may say quaternions are not handed, but these needed modification.
                return new Quaternion(-(float)quat.x, -(float)quat.y, (float)quat.z, (float)quat.w);
            }

            public static Matrix4x4 ConvertPoseToMatrix(OSVR.ClientKit.Pose3 pose)
            {
                return Matrix4x4.TRS(Math.ConvertPosition(pose.translation), Math.ConvertOrientation(pose.rotation), Vector3.zero);
            }

            [Obsolete("Use Math.ConvertPoseToMatrix instead.")]
            public static Matrix4x4 ConvertPose(OSVR.ClientKit.Pose3 pose)
            {
                return Matrix4x4.TRS(Math.ConvertPosition(pose.translation), Math.ConvertOrientation(pose.rotation), Vector3.zero);
            }

            public static OSVR.Unity.Pose3 Convert(OSVR.ClientKit.Pose3 pose)
            {
                return new OSVR.Unity.Pose3
                {
                    Rotation = OSVR.Unity.Math.ConvertOrientation(pose.rotation),
                    Position = OSVR.Unity.Math.ConvertPosition(pose.translation),
                };
            }

            //Convert OSVR.ClientKit.Viewport to Rect
            public static Rect ConvertViewport(OSVR.ClientKit.Viewport viewport)
            {
                //Unity expects normalized coordinates, not pixel coordinates
                //@todo below assumes left and right eyes split the screen in half horizontally
                return new Rect(viewport.Left / (2f*viewport.Width), viewport.Bottom / viewport.Height, viewport.Width/(viewport.Width*2f), 1);
            }

            //Convert OSVR.ClientKit.Matrix44f to Matrix4x4
            public static Matrix4x4 ConvertMatrix(OSVR.ClientKit.Matrix44f matrix)
            {
                Matrix4x4 matrix4x4 = new Matrix4x4();
                matrix4x4[0, 0] = matrix.M0;
                matrix4x4[1, 0] = matrix.M1;
                matrix4x4[2, 0] = matrix.M2;
                matrix4x4[3, 0] = matrix.M3;
                matrix4x4[0, 1] = matrix.M4;
                matrix4x4[1, 1] = matrix.M5;
                matrix4x4[2, 1] = matrix.M6;
                matrix4x4[3, 1] = matrix.M7;
                matrix4x4[0, 2] = matrix.M8;
                matrix4x4[1, 2] = matrix.M9;
                matrix4x4[2, 2] = matrix.M10;
                matrix4x4[3, 2] = matrix.M11;
                matrix4x4[0, 3] = matrix.M12;
                matrix4x4[1, 3] = matrix.M13;
                matrix4x4[2, 3] = matrix.M14;
                matrix4x4[3, 3] = matrix.M15;
                return matrix4x4;
            }
        }
    }
}
