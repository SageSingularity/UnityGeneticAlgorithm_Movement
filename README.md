# SageSingularity Genetic Algorithm Demo
This demo shows how a genetic algorithm can be used to teach bots how to navigate a short maze. It was built to expand upon coursework code for learning, and to understand one of the ways this algorithm can be used. [Click here to view the Demo!](https://sagesingularity.github.io/UnityGeneticAlgorithm_Movement/)

## Table of Contents
 - [Scripts](#scripts)
   - [Cromosome.cs](#cromosome)
   - [Brain.cs](#brain)
   - [PopulationManager.cs](#population-manager)

## How this Demo Works
Genetic algorithms can be configured in numerous ways, to modify RGB values or to control movement. This demo focuses on navigating a maze, through control of movement. Each gene has a choice: left, right, forward, or backward. This choice is made every 2 seconds, in order to provide an index for reading genes (every 2 seconds we progress through the gene the bot has), or it proceeds to generate a new random addition to it's gene if it has reached the end of the strand.

Each bot is 'Killed' by falling off the platform, which stops the timer counting it's survival time. Survival time is what this demo selects for, thus rewarding the longest lived bots. It's important to note a pitfall of this setup is that a bot learns to basically go in a circle, pushing their survival time to infinity. I left this in the demo to demonstrate that it can happen, and a fix would be to also kill bots that do not continue to make significant progress away from prior 'Rooms' they have occupied.

Here's a figure showing a visual representation of individual genes being added to the overall strand over time:
![](GeneTimingDiagram.jpg?raw=true)

## How to Save the Results
Currently saving the eventual 'Best Survivor', the bot that ends up living the longest, is not implemented. However, you could store the genes generated after a run in file format in order to make use of the results in the future. This is particularly useful in game development for example, where you could save an AI controller that was able to best survive a particular maze or made the right decisions in a combat game.

## Applications for This Algorithm
It's important to note that almost any decision-making situation could be approached with a style of Genetic Algorithm. For example, in an RTS we could code a series of potential strategies that the bot could use and then create a gene that selects a strategy based on outcome:

 - Choice 1: Enemy is using Formation A, Bot is Using Formation A - Outcome, 54% of Bot's formation survives, Enemy Defeated
 - Choice 2: Enemy is using Formation A, Bot is Using Formation B - Outcome, Bot is Completely Defeated
 - Choice 3: Enemy is using Formation A, Bot is Using Formation C - Outcome, 95% of Bot's formation survives, Enemy Defeated (Greatest Reward)
 
In the example above, the bot is rewarded based on how many troops in it's formation survive the engagement when the enemy is using Formation A. Combine that with the Genetic Algorithm selecting for Genes that maximize the number of troops left over after a battle, and over time the Bot learns to counter Formation A with Forcmation C through simulation, not hardcoding. Because Genetic Algorithms can run at Runtime, this could change if Formation C stops being effective against Formation A; at first the bot would experience heavy losses, but a mutation could learn a new counter through random chance, and begin to propagate it's genes into new generations of bots.

## Scripts
This section covers scripts directly involved with implementing the genetic algorithm in this demo. Scripts not listed here are but present in this github repository are build output files that Unity3D engine outputs in order to [build an HTML5 standalone](https://docs.unity3d.com/Manual/webgl-gettingstarted.html).

### Cromosome
The Cromosome script stores the gene that each bot is using. Each bot gets its own 'Gene'.

Contains the following functions:
 - SetInt(): Allows manual modification of a specific gene. 

### Brain
The Brain script is in charge of reading each bots Cromosome, and implementing the genes it finds within. Each bot gets its own 'Brain'.

### Population Manager
Controls spawning the bots and managing each generation. This script is attached to the populationManager game object, and is NOT attached to the bots. It makes use of a template prefab in order to instantiate bots, so modifications should be made to that template in order to be implemented.
