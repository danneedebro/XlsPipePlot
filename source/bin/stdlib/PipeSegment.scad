$fn=50;  // REMOVE
use <curvedPipe.scad> // REMOVE
LENGTH = 1;  // REMOVE
DO = 0.1; // REMOVE
WALLTHICKNESS = 0.002; // REMOVE
NODES = [[0.0000,0.0000,0.0000],[0.0000,0.0000,-0.5000],[2.0000,0.0000,-0.5000],[2.0000,0.0000,-2.5000],[2.0000,0.6000,-2.5000],[2.0000,1.2000,-2.5000]]; // REMOVE
SEGMENTS = 5; // REMOVE
BENDRADIUS = [0.146895,0.146895,0.146895,0.146895,0.146895]; // REMOVE
curvedPipe((NODES),(SEGMENTS),(BENDRADIUS),(DO),(DO)-2*(WALLTHICKNESS));