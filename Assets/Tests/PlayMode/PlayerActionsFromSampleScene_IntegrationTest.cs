using System.Collections;
using NUnit.Framework;
using Platformer.Mechanics;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PlayerActionsFromSampleScene_IntegrationTest
    {
        // For this example I am always using the same scene
        // Test Scenes should be created for each Tests Batch
        private const string TestSceneName = "SampleScene";
        
        private GameObject playerGO;
        private GameObject enemyGO;
        private GameObject deathZoneGO;
        private GameObject tokenGO;

        private GameObject enemyPrefab;
        private GameObject tokenPrefab;
        
        private MockInputController mockInput;
        private PlayerController playerController;
        private TokenController tokenController;
        private TokenInstance tokenComponent;

        [UnityOneTimeSetUp]
        public IEnumerator SetUpOnce()
        {
            // Load the scene once if batch mode tests
            yield return SceneManager.LoadSceneAsync(TestSceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(TestSceneName));

            playerController = Object.FindFirstObjectByType<PlayerController>();
            Assert.IsNotNull(playerController, "PlayerController can't be found in Scene after LoadScene.");
            
            tokenController = Object.FindFirstObjectByType<TokenController>(FindObjectsInactive.Include);
            Assert.IsNotNull(tokenController, "TokenController can't be found in Scene after LoadScene.");

            var path = "Prefabs/Token";
            tokenPrefab = Resources.Load<GameObject>(path);
            Assert.IsNotNull(tokenPrefab, $"Resources: {path} can't be reached.");

            path = "Prefabs/Enemy";
            enemyPrefab = Resources.Load<GameObject>(path);
            Assert.IsNotNull(tokenPrefab, $"Resources: {path} can't be reached.");
            
            playerGO = playerController.gameObject;
            mockInput = new MockInputController();
            playerController.Setup(mockInput);
        }
        
        [UnitySetUp]
        public IEnumerator Setup()
        {
            // Reset Player before testing
            playerGO.transform.position = new Vector3(0, 1, 0);
            playerGO.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            mockInput.JumpPressed = false;
            
            yield return new WaitUntil(() => playerController.IsGrounded);
        }
        
        [UnityTearDown]
        public void TearDown()
        {
            if(tokenComponent != null && tokenController != null)
                tokenController.RemoveToken(tokenComponent);
            
            if (tokenGO != null)
                Object.Destroy(tokenGO);
        }
        
        [UnityOneTimeTearDown]
        public IEnumerator TearDownOnce()
        {
            // Delete Player
            if (playerGO != null)
                Object.Destroy(playerGO);

            // Delete Enemy
            if (enemyGO != null)
                Object.Destroy(enemyGO);

            // Unload Scene
            yield return SceneManager.UnloadSceneAsync(TestSceneName);
        }

        [UnityTest]
        public IEnumerator Player_DoesNotJump_WithoutInput()
        {
            var y0 = playerGO.transform.position.y;
            yield return new WaitForFixedUpdate();
            var y1 = playerGO.transform.position.y;

            Assert.LessOrEqual(y1, y0 + 0.01f, "Player should not move without input.");
        }

        [UnityTest]
        public IEnumerator Player_Jumps_WithInput()
        {
            var y0 = playerGO.transform.position.y;

            mockInput.JumpPressed = true;
            yield return new WaitForFixedUpdate();
            mockInput.JumpPressed = false;

            var y1 = playerGO.transform.position.y;
            Assert.Greater(y1, y0, "Player should have jumped.");
        }

        [UnityTest]
        public IEnumerator Player_Lands_AfterJump()
        {
            var startY = playerGO.transform.position.y;

            mockInput.JumpPressed = true;
            yield return new WaitForFixedUpdate();
            mockInput.JumpPressed = false;

            yield return new WaitUntil(() => playerController.IsGrounded);

            var endY = playerGO.transform.position.y;
            Assert.LessOrEqual(Mathf.Abs(endY - startY), 0.1f, "Player should be Grounded.");
        }

        [UnityTest]
        public IEnumerator Player_Collects_TokenPrefab()
        {
            Instantiate_Token(new Vector3(0, 0, 0));
            playerGO.transform.position = tokenGO.transform.position;

            yield return new WaitForFixedUpdate();
            Assert.IsTrue(tokenComponent.IsCollected, "Player should have collected token.");
        }

        [UnityTest]
        public IEnumerator Enemy_DoesNotCollect_TokenPrefab()
        {
            Instantiate_Token(new Vector3(2, 0, 0));
            Instantiate_Enemy(new Vector3(2, 0, 0));

            yield return new WaitForFixedUpdate();
            Assert.IsFalse(tokenComponent.IsCollected, "Enemy should not collect Token.");
        }

        [UnityTest]
        public IEnumerator Player_Dies_WhenCollidingWithEnemy()
        {
            Instantiate_Enemy(new Vector3(2, 0, 0));
            playerGO.transform.position = enemyGO.transform.position;
            
            yield return new WaitForFixedUpdate();
            yield return null;
            
            Assert.IsFalse(playerController.controlEnabled, "Player control should be disabled.");
            Assert.IsFalse(playerController.health.IsAlive, "Player should be dead.");
        }

        [UnityTest]
        public IEnumerator Player_Dies_WhenTouchingDeathZone()
        {
            var deathZoneGO = new GameObject("DeathZone");
            deathZoneGO.transform.position = new Vector3(-2, 0, 0);;
            var collider = deathZoneGO.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(20, 20);
            collider.isTrigger = true;
            deathZoneGO.AddComponent<DeathZone>();

            playerGO.transform.position = deathZoneGO.transform.position;

            yield return new WaitForFixedUpdate();
            yield return null;

            Assert.IsFalse(playerController.controlEnabled, "Player control should be disabled.");
            Assert.IsFalse(playerController.health.IsAlive, "Player should be dead.");

            if (deathZoneGO != null)
                Object.Destroy(deathZoneGO);
        }

        private void Instantiate_Token(Vector3 position)
        {
            // If it already exists, reset Token
            if (tokenGO != null)
            {
                if (tokenComponent == null)
                    tokenComponent = tokenGO.GetComponent<TokenInstance>();
                
                Assert.IsNotNull(tokenComponent, "Token does not contains TokenInstance.");
                tokenComponent.Reset();
            }
            else
            {
                Assert.IsNotNull(tokenPrefab, "Token Prefab can't be reached.");
                tokenGO = Object.Instantiate(tokenPrefab, position, Quaternion.identity);
                tokenComponent = tokenGO.GetComponent<TokenInstance>();
                Assert.IsNotNull(tokenComponent, "Token does not contains TokenInstance.");
                tokenController.AddToken(tokenComponent);
            }
            
            tokenGO.transform.position = position;
        }
        
        private void Instantiate_Enemy(Vector3 position)
        {
            // If it does not exist, instantiate
            if (enemyGO == null)
            {
                Assert.IsNotNull(enemyPrefab, "Enemy Prefab can't be reached.");
                enemyGO = Object.Instantiate(enemyPrefab, position, Quaternion.identity);
            }
            enemyGO.transform.position = position;
        }
    }
}