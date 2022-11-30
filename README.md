# AR branch
Warning: This branch is experimental, some crashes were observed.

This is what you need to apply to the scene when merging main into this branch, in case there are merge conflicts:
- Take the MainScene from main
- Modify the ModeMenu object in that scene: Assign the 3 main cameras from the OVR camera rig to the "Cameras to modify" field.

# Project setup
You should the TextMeshPro assets via Window -> TextMeshPro -> Import TMP Essential Resources

Set the Unity platform to be Android.

# Universal Render Pipeline
Some of the Oculus interaction SDK shaders are not compatible with the Universal Render Pipeline. That is why I have used compatible versions that can be found there https://github.com/rje/interaction_urp. You don't need to download anything though. The shaders that we need should be copied over already and referenced in the scene. This issue in the repository also helped fixing some shaders not compiling in this library https://github.com/rje/interaction_urp/issues/2

# Jenkins
You can build the project with Jenkins, by calling the methods implemented in the Builder class from the Jenkins Job. See https://fadhilnoer.medium.com/automating-unity-builds-part-1-ba0c60e8d06b for a setup example

# Credits
<a href="https://www.flaticon.com/free-icons/domino" title="domino icons">Domino icons created by Freepik - Flaticon</a>

"Gold medieval bowl (Low-poly, game-ready)" (https://skfb.ly/ovWFF) by Delandi is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).

https://assetstore.unity.com/packages/3d/props/pbr-little-games-pack-4k-237164