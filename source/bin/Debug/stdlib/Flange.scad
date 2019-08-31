$fn=50;  // REMOVE
PARAM1 = 0.15; // REMOVE
DO = 0.07; // REMOVE
LENGTH = 0.02; // REMOVE
rotate([0,90,0])
linear_extrude(height = (LENGTH), center = true, convexity = 10, twist = 0)
difference()
{
circle(r = (PARAM1)/2);
circle(r = (DO)/2);
}