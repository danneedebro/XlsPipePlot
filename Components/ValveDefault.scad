$fn=50;  // REMOVE
LENGTH = 0.12;  // REMOVE
DO = 0.1; // REMOVE
rotate([0, 90, 0])
translate([0, 0, (LENGTH)/2])
cylinder(h = (LENGTH), r = (DO)/2, center = true);
translate([(LENGTH)/2, 0, (LENGTH)/4])
cylinder(h = (LENGTH)/2, r = (DO)/2, center = true);