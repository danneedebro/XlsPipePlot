$fn=50;  // REMOVE
LENGTH = 0.1;  // REMOVE
//DO = (LENGTH)*0.6; // REMOVE
translate([(LENGTH)/2,0,0]) sphere(r=(LENGTH)/2);
rotate([0,90,0]) cylinder(h = (LENGTH), r = (LENGTH)*0.6/2);
//translate([(LENGTH)/2,0,0]) cylinder(h = (LENGTH)/2, r = (LENGTH)*0.6/2);
translate([(LENGTH)/2,0,(LENGTH)*0.38]) cylinder(h = (LENGTH)*0.1, r = (LENGTH)*0.8/2);
//translate([(LENGTH)/2,0,(LENGTH)*0.42]) cylinder(h = (LENGTH)*0.2, r = (LENGTH)*0.5/2);