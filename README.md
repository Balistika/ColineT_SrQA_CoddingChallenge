# Platformer Game Architecture (Unity + Zenject)

## Assumptions & Architectural Goals

- The game architecture requires a StateMachine, at minimum to manage the `Jump` state.
- For this test, only the `JumpStateMachine` was created.
- This StateMachine must be extendable to handle all possible player states: Idle, Run, Attack, Slide, etc.
- Zenject is used as the main Dependency Injection framework.
- Alternatively, a custom lightweight DI container may be considered, but Zenject is preferred due to its tooling, flexibility, and Unity integration.
- For this limited-time test, the Zenject architecture was only applied to critical controllers such as `PlayerController` and `InputController`.

## To Go Further with Dependency Injection & Controller Structure

- All core controllers (Player, Enemy, Token, GameManager, etc.) should be registered and resolved through Zenject.
- Controllers must implement interfaces to ensure testability and flexibility.
- Use Factory patterns for dynamic creation (e.g., projectiles, pickups, tokens).

## Testing

- Unit Tests and PlayMode Tests are written using Unity Test Framework + Zenject's test utilities.
- Critical logic such as:
    - Player death handling
    - Enemy path validation
    - State transitions
    - Ground checks

## CI Integration

### High-Level Design: Runtime Test Integration in CI Pipeline

To integrate runtime (PlayMode) tests into the CI pipeline effectively, the following architecture is proposed. This design assumes access to standard CI runners and schedulers (such as GitHub-hosted runners or self-hosted runners).

#### 1. **Triggering the Pipeline**

- The CI pipeline is triggered automatically on:

    - Push to `main` or `develop` branches
    - Pull requests targeting `main`

- Pull requests cannot be merged into `main` unless all tests pass successfully. This ensures the main branch remains stable and production-ready.

- The CI pipeline is triggered automatically on:

    - Push to `main` or `develop` branches
    - Pull requests targeting `main`

Additionally, for code quality and maintainability, SonarQube can be integrated into the CI pipeline. It provides static analysis, detects bugs, code smells, and helps enforce coding standards across the project.

A  quality gate can also be configured as a required check for pull request merging, ensuring that no code is merged unless it passes defined quality thresholds.&#x20;

#### 2. **CI Runner Setup**

- A CI runner with Unity installed (e.g., using `game-ci/unity-actions/setup`) is used to execute the job.
- Unity version is pinned in the workflow file to avoid environment mismatches.

#### 3. **Test Execution Stage**

- The `unity-test-runner` action is used to run both EditMode and PlayMode tests.
- Results are stored as JUnit-style test reports.
- Failures in runtime tests (e.g., player death, state transition validation) will fail the build.

#### 4. **Result Collection & Reporting**

- Test outputs are uploaded as CI artifacts.
- GitHub Actions natively displays test summaries.
- Optional: Codecov integration for coverage badge.

#### 5. **Build Stage** (optional)

- `unity-builder` compiles the game for WebGL or standalone platforms after tests pass.
- Build artifacts can be deployed or uploaded to GitHub Releases.

#### 6. **Scalability Considerations**

- For large projects or long tests:
    - Use matrix builds to run EditMode and PlayMode in parallel.
    - Cache the Unity Library folder for faster builds.
    - Use self-hosted runners with GPU support for performance profiling.



