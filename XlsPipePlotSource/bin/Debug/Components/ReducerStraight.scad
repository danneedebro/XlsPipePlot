$fn=50;  // REMOVE
LENGTH = 0.2;  // REMOVE
DO = 0.1; // REMOVE
C1_DO = 0.1; // REMOVE
C2_DO = 0.2; // REMOVE
PARAM1 = 0.2; // REMOVE
rotate([0, 90, 0])
translate([0, 0, (LENGTH)/2])
cylinder(h = (LENGTH), r1 = (C1_DO)/2, r2 = (C2_DO)/2, center = true);