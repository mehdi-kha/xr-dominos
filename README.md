# Project setup
**Very important: In order for the project to compile, you must download and import the Oculus Platform and interaction SDKs for Unity. See https://developer.oculus.com/documentation/unity/unity-import/#import-sdk-from-unity-asset-store for more information on how to import it. Version 46 of the SDK was used during the development of this project.**

You should also import the TextMeshPro assets via Window -> TextMeshPro -> Import TMP Essential Resources

Set your the Unity platform to be Android.

# Universal Render Pipeline
Some of the Oculus interaction SDK shaders are not compatible with the Universal Render Pipeline. That is why I have used compatible versions that can be found there https://github.com/rje/interaction_urp. You don't need to download anything though. The shaders that we need should be copied over already and referenced in the scene. This issue in the repository also helped fixing some shaders not compiling in this library https://github.com/rje/interaction_urp/issues/2