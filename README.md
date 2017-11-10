# Augmented Reality for Developers [book] Projects

This repository contains Unity project versions of the book for the following platforms and SDK:

* Unity experimental-ARInterface, [GitHub](https://github.com/Unity-Technologies/experimental-ARInterface)
* ARKit and ARCore SDK versions as provided in the ARInterface (commit cited below)
* Unity 2017.2.0f3

The Unity Technologies experimental-ARInterface is a cross-platform framework for mobile AR shown as part of ["So You Think You Can Augment Reality?"](https://youtu.be/oNekBgognFE) talk at Unite Austin 2017. Also see [blog post](https://blogs.unity3d.com/2017/11/01/experimenting-with-multiplayer-arcore-and-arkit-jump-in-with-sample-code/). It allows code-once, build-many so our AR scenes can build for ARKit (iOS) and/or ARCore (Android).

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
This repository implements the projects presented in the book using the Unity ARInterface components. The Unity ARInterface (ARI) is experimental and subject to change. 

This implementation uses [this commit](https://github.com/Unity-Technologies/experimental-ARInterface/tree/8a6b5aa10f7b850714d9cc1849d7b70d24af9b02) of the ARInterface. 

While the ARInterface is convenient, providing a single API for either ARKit or ARCore, it does have limitations. In particular, presently it only detects horizontal planes. Some of our projects pefer vertical planes, or at least, arbitrary planes, and fallback to point cloud. 

## Chapter 4 - Drone

Uses Unity ARI components for camera and plane hit test.

## Chapter 5 - SolarSystem

Uses Unity ARI components for camera and plane hit test.

## Chapter 7 - Instruction Manual

Uses Unity ARI components for camera. 

In the ARPrompt object, we replace ARHitHandler with a new UnityARIHitHandler script. It uses a horizontal plane to detect the anchor point, and places vertically, and facing the camera.

## Chapter 8 - PhotoFrames

Uses Unity ARI components for camera. 

Because we only detect horizontal planes, we implemented a new Move mechanic (script MoveToolUnityARI) which detects a plane to set an initial anchor point, and as the user continues touching we modify the Y height of the photoframe. The photoframe is always oriented facing the camera so also be sure to face your wall head-on.

## Chapter 9 Notes - Ball Game

An anchor-based version of the ball game (Chapter 9) was not covered directly in the book. For the implementation we basically made a few changes from the Vuforia smart terrain implementation described in the book. The rest of the project is essentially the same.

To anchor the game court in world coordinates we use horizontal plane detection. When the game starts you begin in "anchor mode". Once the court is placed, you press "Play" to begin the "game model" and play game as usual. A "Reset" button returns you to "anchor mode".

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

We replace the Vuforia-specific AppStateManager script with the anchor-based AnchorStateManagerUnityARI script. It contains functions, SetGameMode and SetAnchorMode triggered by the CompleteButton and ResetButton, respectively.



