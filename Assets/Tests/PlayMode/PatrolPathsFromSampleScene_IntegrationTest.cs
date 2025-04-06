using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Platformer.Mechanics;
using UnityEngine.Tilemaps;

namespace Tests.PlayMode
{
    public class PatrolPathsFromSampleScene_IntegrationTest
    {
        // For this example I am always using the same scene
        // Test Scenes should be created for each Tests Batch
        private const string TestSceneName = "SampleScene";
        
        private List<PatrolPath> patrolPaths;
        
        [UnityOneTimeSetUp]
        public IEnumerator SetUpOnce()
        {
            // Load the scene once if batch mode tests
            yield return SceneManager.LoadSceneAsync(TestSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));
            
            patrolPaths = Object.FindObjectsByType<PatrolPath>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            Assert.IsNotEmpty(patrolPaths, "No PatrolPaths found in Scene");
        }
        
        [UnityOneTimeTearDown]
        public IEnumerator TearDownOnce()
        {
            // Unload Scene
            yield return SceneManager.UnloadSceneAsync(TestSceneName);
        }
        
        [UnityTest]
        public IEnumerator AllPatrolPaths_HaveGroundBelow_AndAvoidWalls()
        {
            foreach (var path in patrolPaths)
            {
                var worldStart = ToWorldPosition(path.transform, path.startPosition);
                var worldEnd = ToWorldPosition(path.transform, path.endPosition);


                Assert.IsFalse(IsOverlappingWall(worldStart), $"{path.name} : StartPosition touches a Wall!");
                Assert.IsFalse(IsOverlappingWall(worldEnd), $"{path.name} : EndPosition touches a Wall!");

                Assert.IsTrue(HasTilemapGroundBelow(worldStart), $"{path.name} : No Ground under StartPosition!");
                Assert.IsTrue(HasTilemapGroundBelow(worldEnd), $"{path.name} : No Ground under EndPosition!");
                
                Assert.IsFalse(RaycastHitsObstacle(worldStart, worldEnd), $"{path.name} : Obstacle between StartPosition and EndPosition !");
                
                Assert.IsTrue(IsGroundedAlongPath(worldStart, worldEnd), $"{path.name} : Path is not fully grounded!");
            }
            
            yield return null;
        }
        
        private Vector2 ToWorldPosition(Transform reference, Vector2 localPosition)
        {
            return reference.TransformPoint(localPosition);
        }

        private bool IsOverlappingWall(Vector2 position)
        {
            var colliders = Physics2D.OverlapCircleAll(position, 0.01f);
            return colliders.OfType<TilemapCollider2D>().Any();
        }

        private bool HasTilemapGroundBelow(Vector2 position)
        {
            var hits = Physics2D.RaycastAll(position, Vector2.down, 2f);
            return hits.Any(hit => hit.collider is TilemapCollider2D);
        }

        private bool RaycastHitsObstacle(Vector2 start, Vector2 end)
        {
            var direction = (end - start).normalized;
            var distance = Vector2.Distance(start, end);
            var hits = Physics2D.RaycastAll(start, direction, distance);
            return hits.Any(hit => hit.collider != null && hit.collider is TilemapCollider2D);
        }
        
        private bool IsGroundedAlongPath(Vector2 start, Vector2 end, float step = 0.2f)
        {
            var direction = (end - start).normalized;
            var distance = Vector2.Distance(start, end);
            var steps = Mathf.CeilToInt(distance / step);

            for (int i = 0; i <= steps; i++)
            {
                var point = start + direction * (i * step);

                if (!HasTilemapGroundBelow(point))
                    return false;
            }
            return true;
        }
    }
}
