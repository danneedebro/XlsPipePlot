$fn=50;  // REMOVE
LENGTH = 0.1;  // REMOVE
DO = 0.1; // REMOVE

rotate([(C1_ANGLE_AXIS),-1*(C1_ANGLE_VERTICAL),-1*(C1_ANGLE_AZIMUTHAL)])
difference()
{
    translate([(LENGTH)/2,0,0]) cube([(LENGTH),(LENGTH)*4,(LENGTH)*4], center = true);
    rotate([0,90,0]) cylinder(h = (LENGTH)*3, r = (DO)*1.1/2, center = true);
}