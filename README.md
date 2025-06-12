## Game Programming Assignment

**Game Name: Mission Alpha**

This is the code repository of **DI32002 - Games Programming ( SEM 2 24/25 )'s** assignment.

Project files are in `Assets/`.

Resource reference is in `ResourceReference.md`.

Executable file is released in `GitHub releases`.

If you want to compile the project, Unity 2022.3.55f1c1 is recommended (other versions may also be OK). 
You should:
1. Create a new Unity project with 2D Core template
2. Install `Newton Json` package from Unity Package Manager
3. Close Unity Editor
4. Replace the `Assets/` folder of your Unity project by the `Assets/` folder from GitHub repository
5. Copy the `ProjectSettings/TagManager.asset` file to the `ProjectSettings/` folder of your Unity project
6. Open Unity Editor and wait for the project to be imported
7. Add scenes from `Assets/Scenes/` 

**Arrange the scenes like this**
```
Main
Game (not loaded)
PrivatePolicy (not loaded)
Rule (not loaded)
Setting(notloaded)
Shop (not loaded)
Warehouse (not loaded)
```

**IMPORTANT**

The game may work a little bit different in your Unity Editor and the executable file. Because many automatically generated project files by Unity Editor are not uploaded to GitHub.
If you follow the instructions above, you should be able to run the game in Unity Editor without meeting severe errors.