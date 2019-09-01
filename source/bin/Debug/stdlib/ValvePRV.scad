$fn=50;  // REMOVE
LENGTH = 0.1;  // REMOVE
DO = (LENGTH)*0.05; // REMOVE
//rotate([0,-1*90,-1*180])
rotate([0,90,0])
translate([0,0,-(DO)*1.6])
{
    cylinder(h = (DO)*0.1, r =(DO)*1.5/2); // Inlet flange
    cylinder(h = (DO), r =(DO)/2); // Inlet pipe
    translate([0,0,(DO)*0.9]) cylinder(h = (DO)*1.5, r =(DO)*1.4/2); // Main body
    translate([0,0,(DO)*3.2/2]) rotate([0,90,0])
    {
       cylinder(h = (DO)*1.2, r =(DO)*1.2/2);  // Outlet pipe
       translate([0,0,(DO)*1.2]) cylinder(h = (DO)*0.1, r =(DO)*1.5/2); // Outlet flange
    }
    translate([0,0,(DO)*2.3]) cylinder(h = (DO)*0.8, r1 =(DO)*1.2/2, r2=(DO)*0.5/2);
    translate([0,0,(DO)*2]) cylinder(h = (DO)*3, r =(DO)*0.5/2);
}
