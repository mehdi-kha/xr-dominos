# Project setup
You should the TextMeshPro assets via Window -> TextMeshPro -> Import TMP Essential Resources

Set the Unity platform to be Android.

# Universal Render Pipeline
Some of the Oculus interaction SDK shaders are not compatible with the Universal Render Pipeline. That is why I have used compatible versions that can be found there https://github.com/rje/interaction_urp. You don't need to download anything though. The shaders that we need should be copied over already and referenced in the scene. This issue in the repository also helped fixing some shaders not compiling in this library https://github.com/rje/interaction_urp/issues/2

# Dependency injection
This project uses Zenject to handle dependency injection. This installer is a scriptable object, referenced in the MainScene in the ZenjectSceneContext gameobject.

# Tutorial slides
New tutorial slides can be created by creating a SlideData: Right click -> Create -> ScriptableObjects -> SlideData.

Complete the data, then reference this slide in the SceneSetupModel scriptable object, located under Assets/Modules/SceneSetup/ScriptableObjects. The slides registered in this model will be displayed one after the other during the tutorial.

# Jenkins
You can build the project with Jenkins, by calling the methods implemented in the Builder class from the Jenkins Job. See https://fadhilnoer.medium.com/automating-unity-builds-part-1-ba0c60e8d06b for a setup example

# Credits
<a href="https://www.flaticon.com/free-icons/domino" title="domino icons">Domino icons created by Freepik - Flaticon</a>

"Gold medieval bowl (Low-poly, game-ready)" (https://skfb.ly/ovWFF) by Delandi is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).

https://assetstore.unity.com/packages/3d/props/pbr-little-games-pack-4k-237164

Music by <a href="https://pixabay.com/users/alexiaction-26977400/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=126735">AlexiAction</a> from <a href="https://pixabay.com//?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=126735">Pixabay</a>

Music by <a href="https://pixabay.com/users/10270511-10270511/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=125178">MARCO DARIO</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=125178">Pixabay</a>

	# Male voice:

	Jeffrey M. Smith
	http://fiverr.com/jeffreymsmith

			------------------------------

	# Female voice:

	Giselle
	http://fiverr.com/easymedia