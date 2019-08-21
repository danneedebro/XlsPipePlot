$fn=50;  // REMOVE
DO = 1.2; // REMOVE
PARAM1 = 0.75; // REMOVE
LENGTH = 2.0; // REMOVE
rotate([0,90,0])
translate([0,0,(LENGTH)/2])
union()
{
    union()
    {
        intersection()
        {
            translate([0,0,(LENGTH)/2]) translate([0,0,-(PARAM1)]) sphere(r=(PARAM1));
            cylinder(h = (LENGTH)/2, r = (DO)/2);
        }
        cylinder(h = (LENGTH)/2-(PARAM1), r=(DO)/2);
    }
    mirror([0,0,1])
    union()
    {
        intersection()
        {
            translate([0,0,(LENGTH)/2]) translate([0,0,-(PARAM1)]) sphere(r=(PARAM1));
            cylinder(h = (LENGTH)/2, r = (DO)/2);
        }
        cylinder(h = (LENGTH)/2-(PARAM1), r=(DO)/2);
    }
}