$fn=50;  // REMOVE
LENGTH = 1;  // REMOVE
DO = 0.1; // REMOVE
NOTES = "Helloj"; // REMOVE
rotate([-(90-$vpr[0]),0,$vpr[2]])
rotate([90,0,0]) scale($vpd/5*[(DO)/10,(DO)/10,(DO)/10]) text("(NOTES)", size=10);