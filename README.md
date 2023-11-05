# Rubik Cubes Simulator
## Description
This is a repository for my dissertation I created during my 3rd year of the BSc (Hons) Computing Science course at the University of East Anglia.
The project has the following features:
* Simulating a Rubik’s cube.
* A 3D interactive GUI to allow the user to interact with a Simulated cube.
* The ability to execute more than one solving algorithm, those being:
..* The Layer by Layer method.
..* The CFOP method.
* The ability to measure the efficiency of the algorithms using various metrics.
* The ability to show the effectiveness of the methods via a histogram.
* To show an animation of a cube being solved using the implemented methods with some information about the method being displayed alongside it.
* To allow the user to input a Rubik’s cube state into the software and see it be solved via the animation system.
* The ability to save and load multiple Rubik’s cube states.
## Screenshots
![Screenshot of the Cube input system.](/images/Screenshot-Input.png "Screenshot of the Cube input system.")
![Screenshot of the Interactive cube in its default state.](/images/Screenshot-3D.png "Screenshot of the Interactive cube in its default state.")
![Screenshot of the Cube being solved.](/images/Screenshot-3D-Animate.png "Screenshot of the Cube being solved.")
![Screenshot of the bar chart showing information about Layer By Layer.](/images/Screenshot-Bar1.png "Screenshot of the bar chart showing information about Layer By Layer.")
![Screenshot of the bar chart showing information about CFOP.](/images/Screenshot-Bar2.png "Screenshot of the bar chart showing information about CFOP.")
![Screenshot of the input system save menu.](/images/Screenshot-Save1.png "Screenshot of the Cube input system save menu.")
![Screenshot of the interactive cube save menu.](/images/Screenshot-Save2.png "Screenshot of the interactive cube save menu.")
![Screenshot of the loading menu in the interactive cube scene.](/images/Screenshot-Load.png "Screenshot of the loading menu in the interactive cube scene.")
## Running the project
To run the project first install Unity Editor version 2021.3.18f1 LTS then open the project.
Next, you can run the program in the editor or build it into an exe.
To run it in the editor hit the play button at the top of the editor.
To build the project go to File > Build Settings.
Here you can select that target platform however the program currently only has support for PC platforms.
Then select either build or build and run, the first builds the project and asks where you want to save it while the second does the same but automatically starts the project afterwards.
## TODO
### Improve Animation
* add the ability to pause, rewind and step through moves
* Give the user the ability to select individual sections of the solvers such as the white cross.
..* This requires the program to be able to check if the cube is ready for that stage, e.g. before the white corners of LBL can be executed there needs to be a white cross on the bottom.
### Improve Move System
The move system could be improved to give the user the ability to store, input and replay sequences of moves.
### Improving the Implementation of Solving Algorithms
The current implementation of solving CFOP and LBL could be improved, for example:
* it is currently possible to end up with a solution that contains a move that is immediately followed by its inverse
* The order in which parts of the solution are executed could be improved, e.g. so that edges that are closer to being solved are solved first to prevent accidentally moving pieces that would be easier to solve, thus bringing down the average number of moves.
* Code implemented earlier in development, e.g. white cross, could be refactored to take advantage of helper functions implemented later in development making them both more efficient and easier to read.
### Implement Additional Solving Algorithms

Additional solving methods other than CFOP and LBL could be implemented such as:
* Thistlethwaite (partially implemented)
* Kociemba
* Korf’s algorithm
* Roux
* Mentha
* ZZ.
### Implementation of Additional Puzzles
The software could also be improved by the implementation of additional puzzles similar to Rubik’s cubes referred to as twisty puzzles, these include:
* Megaminx
* Rubik’s revenge
* Rubik’s Cube void
* Pocket Cube
* Pyraminx
* Skewb
The easiest to implement would be the pocket cube, Rubik’s cube void, Rubik’s Revenge and other Bigcubes.
