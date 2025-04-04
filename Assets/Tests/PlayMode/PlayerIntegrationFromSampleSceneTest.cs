using System.Collections;
using NUnit.Framework;
using Platformer.Mechanics;
using Tests.Mocks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerIntegrationFromSampleSceneTest
{
    private GameObject playerGO;
    private PlayerController playerController;
    private MockInputController mockInput;
    
    private static bool sceneLoaded;

    [OneTimeSetUp]
    public void LoadSceneOnce()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    [UnitySetUp]
    public IEnumerator SetupPlayerReference()
    {
        if (!sceneLoaded)
        {
            float timer = 0f;
            float timeout = 5f;

            while (playerController == null && timer < timeout)
            {
                playerController = GameObject.FindFirstObjectByType<PlayerController>();
                timer += Time.deltaTime;
                yield return null;
            }

            Assert.IsNotNull(playerController, "PlayerController can't be found in Scene after LoadScene.");
            playerGO = playerController.gameObject;

            mockInput = new MockInputController();
            playerController.Setup(mockInput);

            sceneLoaded = true;
        }
        else
        {
            // Reset Player before testing
            playerGO.transform.position = new Vector3(0, 1, 0);
            playerGO.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            mockInput.JumpPressed = false;
        }

        yield return new WaitUntil(() => playerController.IsGrounded);
    }

    [UnityTest]
    public IEnumerator Player_Does_Not_Jump_Without_Input()
    {
        float y0 = playerGO.transform.position.y;
        yield return new WaitForSeconds(0.3f);
        float y1 = playerGO.transform.position.y;

        Assert.LessOrEqual(y1, y0 + 0.01f, "Player should not move without input.");
    }
    
    [UnityTest]
    public IEnumerator Player_Jumps_When_JumpInput_Is_Given()
    {
        float y0 = playerGO.transform.position.y;

        mockInput.JumpPressed = true;
        yield return new WaitForSeconds(0.05f);
        mockInput.JumpPressed = false;

        float y1 = playerGO.transform.position.y;
        Assert.Greater(y1, y0, "Player should have jumped.");
    }

    [UnityTest]
    public IEnumerator Player_Lands_After_Jump()
    {
        float startY = playerGO.transform.position.y;

        mockInput.JumpPressed = true;
        yield return new WaitForSeconds(0.05f);
        mockInput.JumpPressed = false;

        yield return new WaitUntil(() => playerController.IsGrounded);

        float endY = playerGO.transform.position.y;
        Assert.LessOrEqual(Mathf.Abs(endY - startY), 0.1f, "Player should be Grounded.");
    }
    
    [UnityTest]
    public IEnumerator Player_Collects_Token_Prefab()
    {
        var desiredPosition = new Vector3(0, 0, 0);
        
        GameObject tokenPrefab = Resources.Load<GameObject>("Prefabs/Token");
        Assert.IsNotNull(tokenPrefab, "Token Prefab can't be reached.");
        
        GameObject tokenGO = GameObject.Instantiate(tokenPrefab, desiredPosition, Quaternion.identity);
        var token = tokenGO.GetComponent<TokenInstance>();
        var tokenController = Object.FindFirstObjectByType<TokenController>(FindObjectsInactive.Include);
        
        Assert.IsNotNull(token, "Token does not contains TokenInstance.");
        Assert.IsNotNull(tokenController, "Scene does not contains TokenController.");
        
        tokenController.AddToken(token);
        playerGO.transform.position = tokenGO.transform.position;
        
        yield return new WaitForFixedUpdate();
        Assert.IsTrue(token.IsCollected, "Player should have collected token.");

        if (tokenGO != null)
        {
            tokenController.RemoveToken(token);
            GameObject.DestroyImmediate(tokenGO);
        }
    }
    
    [UnityTest]
    public IEnumerator Enemy_Does_Not_Collect_Token_Prefab()
    {
        var desiredPosition = new Vector3(2, 0, 0);
        
        GameObject tokenPrefab = Resources.Load<GameObject>("Prefabs/Token");
        Assert.IsNotNull(tokenPrefab, "Token Prefab can't be reached.");
        
        GameObject tokenGO = GameObject.Instantiate(tokenPrefab, desiredPosition, Quaternion.identity);
        var token = tokenGO.GetComponent<TokenInstance>();
        var tokenController = Object.FindFirstObjectByType<TokenController>(FindObjectsInactive.Include);
        
        Assert.IsNotNull(token, "Token does not contains TokenInstance.");
        Assert.IsNotNull(tokenController, "Scene does not contains TokenController.");
        
        tokenController.AddToken(token);
        
        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        Assert.IsNotNull(enemyPrefab, "Enemy Prefab can't be reached.");
        
        var enemyGo = GameObject.Instantiate(enemyPrefab, desiredPosition, Quaternion.identity);

        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);

        Assert.IsFalse(token.IsCollected, "Enemy should not collect Token.");
        
        if (tokenGO != null)
        {
            tokenController.RemoveToken(token);
            GameObject.DestroyImmediate(tokenGO);
        }
        
        if(enemyGo != null)
            GameObject.DestroyImmediate(enemyGo);
    }
    
    [UnityTest]
    public IEnumerator PlayerDies_WhenCollidingWithEnemy()
    {
        var desiredPosition = new Vector3(2, 0, 0);
        
        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        Assert.IsNotNull(enemyPrefab, "Enemy Prefab can't be reached.");
        
        var enemyGo = GameObject.Instantiate(enemyPrefab, desiredPosition, Quaternion.identity);
        playerGO.transform.position = enemyGo.transform.position;
        
        yield return new WaitForFixedUpdate();
        yield return null;
        
        Assert.IsFalse(playerController.controlEnabled, "Player control should be disabled.");
        Assert.IsFalse(playerController.health.IsAlive, "Player should be dead.");
        
        if(enemyGo != null)
            GameObject.DestroyImmediate(enemyGo);
    }
    
    [UnityTest]
    public IEnumerator PlayerDies_WhenTouchingDeathZone()
    {
        var desiredPosition = new Vector3(2, 0, 0);
        
        var deathZoneGO = new GameObject("DeathZone");
        deathZoneGO.transform.position = desiredPosition;
        var collider = deathZoneGO.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(20, 20);
        collider.isTrigger = true;
        deathZoneGO.AddComponent<DeathZone>();
        
        playerGO.transform.position = deathZoneGO.transform.position;
        
        yield return new WaitForFixedUpdate();
        yield return null;
        
        Assert.IsFalse(playerController.controlEnabled, "Player control should be disabled.");
        Assert.IsFalse(playerController.health.IsAlive, "Player should be dead.");
    }
}