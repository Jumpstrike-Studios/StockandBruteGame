Usage in Unity:
Setup the Enemy GameObject:

Create a new enemy GameObject and attach the EnemyAI script to it.
Add a Rigidbody2D component (set Gravity Scale to 0 and Freeze Rotation Z).
Assign the Player GameObject to the player field in the EnemyAI script.
Tuning Values:

Adjust moveSpeed, attackRange, and detectionRange for desired behavior.
Testing:

Play the scene and watch the enemy patrol, chase the player upon detection, and attack when close enough.
Explanation:
PatrolState: The enemy moves between two points until the player enters detection range.
ChaseState: The enemy chases the player until it gets close enough to attack or loses sight.
AttackState: The enemy stops to attack the player at a set interval.