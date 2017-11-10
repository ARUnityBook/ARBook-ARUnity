# Augmented Reality for Developers [book] Projects

This repository contains Unity project versions of the book for the following platforms and SDK:

* 
* ARKit (unity-arkit-plugin, August 15, 2017)
* Unity 2017.1

For other SDK implementations, check other Branches of this repository and visit https://github.com/ARUnityBook

## AR Projects

The following book's AR projects are included as separate Unity scenes:

* Chapter 4 - Augmented Business Cards: Drone
* Chapter 5 - AR Solar System
* Chapter 6 - How to Change a Flat Tire
* Chapter 7 - Augmenting the Instruction Manual
* Chapter 8 - Room Decoration with AR: Photo Frames
* Chapter 9 - Poke the Ball Game

## About the Book

*Augmented Reality for Developers*

Build exciting AR applications on mobile and wearable devices with Unity 3D, Vuforia, ARToolKit, Microsoft Mixed Reality HoloLens, Apple ARKit, and Google ARCore

by Jonathan Linowes, Krystian Babilinski

Available at:

* [PacktPub](https://www.packtpub.com/web-development/augmented-reality-developers)
* [Amazon](https://www.amazon.com/Augmented-Reality-Developers-Jonathan-Linowes/dp/1787286436)

## How This Implementation Differs from the Book
This repository completes the ARKit implementations of the projects presented in the book.

## Chapter 9 Notes - Ball Game
An anchor-based version of the ball game (Chapter 9) was not covered directly in the book. For the implementation we basically made a few changes from the Vuforia smart terrain implementation described in the book. The rest of the project is essentially the same.

To anchor the game court in world coordinates we use the ARKit SDK. When the game starts you begin in "anchor mode". Once the court is placed, you press "Play" to begin the "game model" and play game as usual. A "Reset" button returns you to "anchor mode".

Anchor Mode:
* Instructions panel is displayed
* Room is scanned and the point cloud particles are displayed
* User places a temporary game model ("anchor" game object)
* The **CompleteButton** click changes to game mode

Game Mode:
* Instructions panel, anchor model (court), and CompleteButton are hidden
* Actual ball game objects are displayed
* Reset button is displayed
* GameController.StartGame() is called to start

We replace the Vuforia-specific AppStateManager script with the anchor-based AnchorStateManager script. It contains functions, SetGameMode and SetAnchorMode triggered by the CompleteButton and ResetButton, respectively.



