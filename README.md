# $P Recognizer Interface
Gesture recognizer interface I created for my Natural User Interaction class that utilizes the $P recognizer engine, which was developed by researchers at the University of Washington.

## Usage
Open the project's folder in a terminal and compile the program by typing `make`. Then refer to the help menu by just running the program with `./pdollar`. Here are the commands and descriptions detailing their usage:
1. Add a gesture file to the list of gesture templates: `pdollar -t <gesturefile>`
2. Print the name of gestures when recognized from the event stream: `pdollar <eventstream>`
3. Clear all stored templates: `pdollar -r`

Example command format: `./pdollar pdollar -t gestureFiles/arrowhead.txt`.

## Acknowledgements
The `PDollarGestureRecognizer.dll` file is where the actual gesture recognizing takes place, and it is product of the hard work of researchers at the University of Washington. Please consider looking into their [research paper](http://faculty.washington.edu/wobbrock/pubs/icmi-12.pdf), as well their [website](http://depts.washington.edu/acelab/proj/dollar/pdollar.html), if you are interested in learning more about $P.

## More Information
This project took around 6-8 hours to complete, and was completed on January 31st, 2020.

## License
[BSD](https://opensource.org/licenses/BSD-3-Clause)
