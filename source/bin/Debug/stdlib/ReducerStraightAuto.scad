$fn=50;  // REMOVE
LENGTH = 1;  // REMOVE
DO = max(0.1,0.2); // REMOVE
C1_DO = 0.2; // REMOVE
C2_DO = 0.1; // REMOVE
PARAM1 = 0.2; // REMOVE
rotate([(C1_ANGLE_AXIS),-1*(C1_ANGLE_VERTICAL),-1*(C1_ANGLE_AZIMUTHAL)])
rotate([0, 90, 0])
translate([0, 0, ((C1_DO)>(C2_DO))?(C1_DO)/4:-(C1_DO)/4])
cylinder(h = max((C1_DO),(C2_DO))/2, r1 = (C1_DO)/2, r2 = (C2_DO)/2, center = true);