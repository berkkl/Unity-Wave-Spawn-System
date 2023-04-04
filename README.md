# Wave Spawn System for Unity

This is a simple and efficient wave-based enemy spawner system for Unity. It uses object pooling for better performance and allows you to configure different waves with multiple enemy types.

### Features
- Wave-based enemy spawning
- Object pooling for better performance
- Multiple enemy types
- Customizable waves, enemy types, and spawn points
- Easy to integrate and extend
### How to use
- Import the EnemySpawner script into your Unity project.
- Create an empty GameObject in your scene and attach the EnemySpawner script to it.
- Set up your spawn points by creating empty GameObjects and adding them to the spawnPoints list in the EnemySpawner script.
- Create waves by adding elements to the waves list in the EnemySpawner script. Each wave should have a unique waveNumber and a list of pools for the enemies.
- For each enemy type, create a Pool in the wave configuration. Set the prefab, and size (number of instances) for the pool.

### Notes
- Prefabs should be set up with appropriate enemy scripts and components.
- Adjust timeToNextSpawn, timeBetweenWaves, and other settings as needed for your game.

### Example Configuration
 
![image](https://user-images.githubusercontent.com/30018589/229891518-2af0f9a7-2058-4376-b9b2-2963d39a7d4c.png)
![image](https://user-images.githubusercontent.com/30018589/229892179-e35332ae-3a93-4000-9b03-e8c93e998fdf.png)
