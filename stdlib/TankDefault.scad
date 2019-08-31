$fn=50;  // REMOVE
LENGTH = 1;  // REMOVE
DO = 0.1; // REMOVE
rotate([0, 90, 0])
translate([0, 0, (LENGTH)/2])
{
    cylinder(h = (LENGTH)-(DO), r = (DO)/2, center = true);
    translate([0,0,((LENGTH)-(DO))/2])
    sphere(r = (DO)/2);
    translate([0,0,-((LENGTH)-(DO))/2])
    sphere(r = (DO)/2);
}