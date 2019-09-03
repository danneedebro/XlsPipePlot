$fn=50;  // REMOVE
LENGTH = 0.1;  // REMOVE
C1_DO = 0.2; // REMOVE
C1_ANGLE_VERTICAL = 90; // REMOVE
C1_ANGLE_AZIMUTHAL = 0; // REMOVE
C1_ANGLE_AXIS = 0; // REMOVE

rotate([0,-1*(C1_ANGLE_VERTICAL),-1*(C1_ANGLE_AZIMUTHAL)])
rotate([0,90,0]) difference(){
   union(){cylinder(h=0.25*(C1_DO), r=(C1_DO)*1.1/2, center=true); cube([0.125*(C1_DO),1.5*(C1_DO),0.25*(C1_DO)],center=true);}
   cylinder(h=0.3*(C1_DO), r=(C1_DO)/2, center=true);
}
   
