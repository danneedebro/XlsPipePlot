$fn=50;  // REMOVE
LENGTH = 0.3;  // REMOVE
DO = 0.1; // REMOVE
rotate([0,90,0]) { cylinder(h=(LENGTH), r=(DO)/2); translate([0,0,(LENGTH)-(DO)*0.1]) cylinder(h=(DO)*0.1, r=(DO)*1.5/2); }