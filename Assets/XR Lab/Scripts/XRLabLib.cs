using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Reflection;
using UnityEditor;

namespace XRLab
{
    public class XRLabLib : MonoBehaviour
    {
        #region Detect VR Device
        /// <summary>
        /// different HMD types
        /// </summary>
        public enum VRHMD
        {
            vive,
            vive_pro,
            vive_cosmos,
            rift,
            indexhmd,
            holo_hmd,
            none
        }


        /// <summary>
        /// different controller types
        /// </summary>
        public enum VRController
        {
            vive_controller,
            vive_cosmos_controller,
            oculus_touch,
            knuckles,
            holo_controller,
            none
        }

        public static bool GetHMDConnected()
        {
            return ("OpenXR Display" == XRSettings.loadedDeviceName);
        }
        /// <summary>
        /// Gets the active hmd. Note that in openXR the output will read openXR display
        /// </summary>
        /// <returns></returns>
        public static string GetHMDType()
        {
            return XRSettings.loadedDeviceName;
        }

        /// <summary>
        /// Prints the currently connected display
        /// </summary>
        public static void PrintHMDType()
        {
            Debug.Log("Connected: " + GetHMDConnected() + " type: " + XRSettings.loadedDeviceName);
        }
        #endregion

        #region Transform functions
        /// <summary>
        /// An enumeration representing all 6 directions of movement
        /// </summary>
        [System.Serializable]
        public enum VectorDirections { forward, backward, up, down, left, right }

        /// <summary>
        /// used to store transform data without references
        /// </summary>
        public struct TransformValue { public Vector3 position; public Quaternion rotation; }

        /// <summary>
        /// converts a transform into a transform value and returns it
        /// </summary>
        /// <param name="input">transform to convert</param>
        /// <returns>transform value of input</returns>
        public TransformValue GetTransformValue(Transform input) 
        { 
            return new TransformValue { position = input.position, rotation = input.rotation };
        }

        /// <summary>
        /// Returns the percentage distance of midpoint between start and end vactors
        /// </summary>
        /// <param name="midpoint"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private float FindPercentageDifference(Vector3 midpoint, Vector3 start, Vector3 end)
        {
            Vector3 difference = end - start;

            float length = difference.magnitude;
            difference.Normalize();

            return Vector3.Dot(midpoint, difference) / length;
        }
        #endregion

        #region Data managment
        /// <summary>
        /// Takes 2 arrays and returns a new array with the contents
        /// of both array parameters. If types do not match, will produce 
        /// an error  
        /// </summary>
        /// <typeparam name="Template">A generic array type that matches the type of the inputs</typeparam>
        /// <param name="array1">Base array, this will be at the starting index of the array</param>
        /// <param name="array2">Secondary array, this will be after the last index of the base array</param>
        /// <returns></returns>
        public static Template[] JoinArrays<Template>(Template[] array1, Template[] array2)
        {
            Template[] output = new Template[array1.Length + array2.Length];

            array1.CopyTo(output, 0);
            array2.CopyTo(output, array1.Length);

            Debug.Log("outputting an array of type " + output.GetType());
            return output;
        } 
        #endregion

        #region Selection
        /// <summary>
        /// Returns true if target layer is located 
        /// within layermask, returns false if target
        /// layer is not found
        /// 
        /// Note contains a bug where checking only default layer will always return false
        /// </summary>
        /// <param name="layer">The layer to search for</param>
        /// <param name="layermask">The layer mask to search</param>
        /// <returns>Is in layer mask</returns>
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
        
        /// <summary>
        /// Get a reference to a GameObject by shooting a raycast from the originTransform forward vector
        /// </summary>
        /// <param name="originTransform">The transform for the ray origin</param>
        /// <param name="maxDistance">The distance to shoot the ray</param>
        /// <param name="layerMask">The layermask for selection targets</param>
        /// <returns>Reference to selected object</returns>
        public GameObject GetSelectedObject(Transform originTransform, float maxDistance, LayerMask layerMask)
        {
            // Create a ray from the camera's position and forward vector
            Ray ray = new Ray(originTransform.position, originTransform.forward);

            // Cast the ray and check if it hits anything
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
            {
                // Return the GameObject that was hit by the raycast
                return hit.transform.gameObject;
            }

            // If nothing was hit, return null
            return null;
        }
        #endregion

        #region Editor Utilities

        /// <summary>
        /// Returns a reference to an editor. Uses reflection 
        /// to get references and can cause memory leaks if wrong 
        /// targetType string is used, Use with caution
        /// </summary>
        /// <param name="targets">targets array inherited from <c>Editor</c> by caller class</param>
        /// <param name="targetType">the type of editor to get a reference to</param>
        /// <returns></returns>
        public static Editor GetEditorReflection(Object[] targets, string targetType)
        {
            // Use reflection to get a reference to the default editor
            var assembly = Assembly.GetAssembly(typeof(Editor));
            var type = assembly.GetType(targetType);
            return Editor.CreateEditor(targets, type);
        }
        #endregion

        #region Misc Utilities
        /// <summary>
        /// Quits the program
        /// </summary>
        public static void ExitGame()
        {
            Application.Quit(0);
        }

        /// <summary>
        /// Returns the offset value required to use a sprite sheet
        /// 
        /// Note: images need to be an equal distance apart for this 
        /// function to return correct values
        /// </summary>
        /// <param name="imageCount"> the number of image tiles on the x and Y axis</param>
        /// <returns>UV offset values</returns>
        public static Vector2 GetUVTilemapOffsets(Vector2 imageCount)
        {
            return new Vector2(1/imageCount.x, 1/imageCount.y);
        }

        /// <summary>
        /// Incomplete function
        /// 
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector2 GetAspectRatio(Texture2D input)
        {
            //int aspect (a / gcf(a, b)) * b;
            Debug.Log("INCOMPLETE LIB: GetAspectRatio used but is incomplete, 0,0 will be returned as a placeholder");
            return new Vector2(0, 0);
        } 
        #endregion
    }
}

