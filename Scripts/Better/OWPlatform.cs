
//Abu Kingly
﻿using UnityEngine;
using System.Collections.Generic;

namespace Better
{
    [RequireComponent(typeof(BoxCollider2D))]
    // One Way Platform Collision
    public class OWPlatform : MonoBehaviour
    {

        #region Fields

        private BoxCollider2D cachedCollider;
        private Matrix4x4 preCalMatrix;

        public bool isPassable; // false, then platform works as normal collider
        public bool isMoveable = false; // If true, then the platform will be moving in its life time
        public bool temporaryPass; // Allows objects to drop through 

        public float checkDistance = 1f; // how close does the actor have to be befor checking the collision

        public static List<Collider2D> testAgainstColliders; // testedAgainst colliders

        #endregion

        #region Properties

        #endregion

        #region Unity Event Functions

        void Awake() {
            testAgainstColliders = new List<Collider2D>();

            cachedCollider = this.GetComponent<BoxCollider2D>();

            if (!isMoveable)
                CalculateMatrix(ref preCalMatrix);
        }

        // Used for initialization
        void Start() {
            checkDistance = CalculateCheckDistance();
        }

        void FixedUpdate() {

            if (testAgainstColliders.Count != 0) {
                for (int i = 0; i < testAgainstColliders.Count; i++) {
                    if ((this.transform.position - testAgainstColliders[i].transform.position).magnitude < checkDistance) {
                        CollisionCheck(this.transform, testAgainstColliders[i].transform.position, testAgainstColliders[i]);
                    }
                }
            }

        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            if (DebugManager.Instance.m_showDebug && cachedCollider != null) {

                //Matrix4x4 matrix = Matrix4x4.TRS(this.transform.localPosition, this.transform.rotation, this.transform.lossyScale);
                Matrix4x4 matrix =  preCalMatrix;
                //CalculateMatrix(ref matrix);
                Gizmos.matrix = preCalMatrix;

                Gizmos.color = Color.magenta;
                Vector3 adjusted = new Vector3(1/cachedCollider.size.x, 1/cachedCollider.size.y, 0) / 4f;

                Vector2 rCorner = Vector2.zero + Vector2.right * (.5f) + Vector2.up * (.5f); // Right Corner
                Vector2 lCorner = Vector2.zero - Vector2.right * (.5f) + Vector2.up * (.5f); // Left Corner

                Gizmos.DrawLine(lCorner, rCorner); // Draws line representing the platforms floor 

                // Reverts the matrix back to normal
                Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

                Vector3 adjLeftCorner = matrix.MultiplyPoint3x4(lCorner);
                Vector3 adjRightCorner = matrix.MultiplyPoint3x4(rCorner);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(adjLeftCorner, this.transform.up);
                Gizmos.DrawRay(adjRightCorner, this.transform.up);

                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(adjLeftCorner, .15f);
                Gizmos.DrawWireSphere(adjRightCorner, .15f);

                // Find the bottom of the player ie feet/ farthest down point of the collider
                Gizmos.color = new Color(0f, 0f, 256f, .2f);
                Gizmos.DrawCube(testAgainstColliders[0].bounds.center, testAgainstColliders[0].bounds.size);    // Colors the bounding box area
            }
        }
#endif

        #endregion

        #region Methods

        // Does a vector to point and does a dot product comparison
        // Checks if the collision with bCollider should be active or not depending on bCollider position relative to the platform
        private void CollisionCheck(Transform platformTrans, Vector2 bPoint, Collider2D bColl) {

            // a line from the platform to the actor
            Debug.DrawLine(this.transform.position, bColl.bounds.ClosestPoint(bColl.transform.position + this.transform.up * 1f));

            Matrix4x4 matrix = Matrix4x4.identity;
            if (isMoveable) {
                CalculateMatrix(ref matrix);
            } else { matrix = preCalMatrix; }

            Vector2 adjCenter = matrix.MultiplyPoint3x4(Vector2.zero + Vector2.up * (.5f)); // middle top point of platform is multiplied by matrix to get adjustedCenter
                                                                                            //Debug.DrawRay(adjCenter, this.transform.up, Color.red); // The up vector of this platform

            Vector2 newVect = bPoint - adjCenter;  // Vector points from this.pos -> testedPoint

            float  vectorDiffrence = Vector2.Dot(platformTrans.transform.up, newVect.normalized); // used to see if avatar is above or below the platforms plane

            if (vectorDiffrence >= 0) {
                // Above the plane

                Vector2 testPoint = bColl.bounds.ClosestPoint(bColl.transform.position - this.transform.up * 2f);
                newVect = testPoint - adjCenter; // vector points from this.pos -> bottom of player
                vectorDiffrence = Vector2.Dot(platformTrans.transform.up, newVect.normalized);

                if (vectorDiffrence > 0) {
                    // The closest point of bounding box is in front of the platform
                    PhysCollisionIgnore(bColl, false); // turn the collision on
                }
            } else if (vectorDiffrence < 0) {
                // Below the plane

                Vector2 testPoint = bColl.bounds.ClosestPoint(bColl.transform.position + this.transform.up * 2f);
                newVect = testPoint - adjCenter; // vector points from this.pos -> bottom of player
                vectorDiffrence = Vector2.Dot(platformTrans.transform.up, newVect.normalized);

                // Do an or. if velocity is going along the up direction of platform then turn collision off
                // If the platform normal and the character.vel are pointing in the same direction then turn collision off
                Rigidbody2D rigi = bColl.GetComponent<Rigidbody2D>();
                float diff = Vector3.Dot(this.transform.up, rigi.velocity.normalized);

                // || diff > 0
                if (vectorDiffrence < 0) {
                    // farthest point of bounding box is clear of the platform
                    PhysCollisionIgnore(bColl, true); // turn the collision off
                }
            }
        }

        // true: will ignore collisions with bCollider
        // false: doesn't ignore collisions with bCollider
        private void PhysCollisionIgnore(Collider2D bColl, bool state) {
            if (cachedCollider != null) {
                Physics2D.IgnoreCollision(cachedCollider, bColl, state);
                Physics2D.IgnoreCollision(bColl, cachedCollider, state);
            }
        }

        // checks a point against another to see the diffrence ie (is it above or bellow the platform)
        private float DiffrenceCheck(Vector2 pStart, Vector2 pEnd, Vector2 up) {
            Vector2 outcome = pEnd - pStart; // Vector pointing from pStart -> pEnd
            return Vector2.Dot(up, outcome.normalized);
        }

        private void CalculateMatrix(ref Matrix4x4 mtrx) {
            mtrx = Matrix4x4.TRS(
                this.transform.localPosition + (Vector3)cachedCollider.offset, // Position
                cachedCollider.transform.rotation, // Rotation
                new Vector2(cachedCollider.size.x * this.transform.lossyScale.x, cachedCollider.size.y * this.transform.lossyScale.y)); // Scale taking into account parent and colliders.size
        }

        // Turns of the collision for a certain object temporarily
        public void TemporaryTurnOff(GameObject obj) {
            PhysCollisionIgnore(obj.GetComponent<BoxCollider2D>(), true);
            //temporaryPass = true;
        }

        // Calculates the minimum distance the object needs to be before any collision calculation is done
        private float CalculateCheckDistance() {
            return (cachedCollider.size.x * this.transform.localScale.x + cachedCollider.size.y * this.transform.localScale.y);
        }

        // Adds another collider that the platform will check if to turn on/off collision
        public static void AddColliderCheck(BoxCollider2D coll) {
            if (!testAgainstColliders.Contains(coll))
                testAgainstColliders.Add(coll);
            else
                Debug.Log("Already has collider in list");
        }

        #endregion
    }
}