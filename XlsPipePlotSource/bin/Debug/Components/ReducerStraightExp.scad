$fn=50;  // REMOVE
LENGTH = 1;  // REMOVE
DO = 0.1; // REMOVE
PARAM1 = 0.2; // REMOVE
rotate([0, 90, 0])
translate([0, 0, -(PARAM1)/2])
cylinder(h = (PARAM1), r1 = (DO)/2, r2 = (PARAM1)/2, center = true);