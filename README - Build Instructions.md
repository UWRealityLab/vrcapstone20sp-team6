# vrcapstone20sp-team6
CSE 481v - VR Capstone @ University of Washington
Project Build Instructions

## Very Important:
I can take up to an hour to do the first build of our project. An apk pack that can be loaded via sidequest is provided in the build folder at the root of the project.


Start a new unity project

## Install Photon Pun and Oculus Integration Plugin

Install the Photon Pun 2 Free and Oculus Integration packages from the Unity store.

Save project.

## Clone the project repo into new project root folder

Open the project and open the build setting in the file menu.

## Build Settings

Install the XR Plugin manager and load the oculus plugin. Then, change the build settings to Android, set the texture compression to ASTC.

Connect the oculus quest and set as the output device.

## Set the Photon Server

Finally in the Assets/Photon/PhotnUnityNetworking/Resources folder, click on PhotonServerSettings and view in the inspector. Expand the Server/Cloud Settings and past the app id: 18aaaf72-b065-45d7-a84a-e6b81794e716

## Set the scene build order

In the build settings, order the scenes in the following order: _HoD/Scenes/HoD_LauncherVR, _HoD/Scenes/HoD_Launcher, _HoD/Scenes/HoD_Prototype

Select build and run and the apk should load onto the device. It is recommnedned to restart the program, as the headset has not zeroed correctly and the user can experience a height issues. If this occurs, just restart the application.