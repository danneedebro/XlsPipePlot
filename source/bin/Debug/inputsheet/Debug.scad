// Generated 2019-08-31 23:06:53
$fn=50;
use <curvedPipe.scad>

colorPipe = "grey";
colorTank = "grey";
colorReducer = "red";
colorValve = "red";


// Section
scale([1000, 1000, 1000])
{
}


// From Tank 1 to Tank 2
scale([1000, 1000, 1000])
{
    // Type=Tank, Name=T1, Id=VOL_33
    color("grey")
    translate([0.0000,0.0000,107.2400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,1.5/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,1.5/2]) translate([0,0,-0.8]) sphere(r=0.8);
                        cylinder(h = 1.5/2, r = 1/2);
                    }
                    cylinder(h = 1.5/2-0.8, r=1/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,1.5/2]) translate([0,0,-0.8]) sphere(r=0.8);
                        cylinder(h = 1.5/2, r = 1/2);
                    }
                    cylinder(h = 1.5/2-0.8, r=1/2);
                }
            }
        } 
    } 
    
    // Type=Connection, Name=Conn, Id=JUNC_35
    color("red")
    translate([0.0000,0.0000,105.7400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,0/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_37
    color(colorPipe)
    translate([0.0000,0.0000,105.7400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,-0.5000],[1.5000,0.0000,-0.5000],[1.5000,0.0000,-1.1000],[1.5000,1.0000,-1.1000]],4,[0.15,0.15,0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Valve, Name=V1, Id=JUNC_42
    color(colorValve)
    translate([1.5000,1.0000,104.6400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*270])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.2/2])
            cylinder(h = 0.2, r = 0.1/2, center = true);
            translate([0.2/2, 0, 0.2/4])
            cylinder(h = 0.2/2, r = 0.1/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_44
    color(colorPipe)
    translate([1.5000,1.2000,104.6400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,1.0000,0.0000],[0.0000,1.0000,-0.4000],[2.0000,1.0000,-0.4000]],3,[0.15,0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Connection, Name=Connection, Id=JUNC_48
    color("red")
    translate([3.5000,2.2000,104.2400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*90,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,1/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,1/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 1/2, r = 0/2);
                    }
                    cylinder(h = 1/2-0, r=0/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,1/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 1/2, r = 0/2);
                    }
                    cylinder(h = 1/2-0, r=0/2);
                }
            }
        } 
    } 
    
    // Type=Tank, Name=T2, Id=VOL_50
    color("grey")
    translate([3.5000,2.2000,105.2400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,1.5/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,1.5/2]) translate([0,0,-0.8]) sphere(r=0.8);
                        cylinder(h = 1.5/2, r = 1/2);
                    }
                    cylinder(h = 1.5/2-0.8, r=1/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,1.5/2]) translate([0,0,-0.8]) sphere(r=0.8);
                        cylinder(h = 1.5/2, r = 1/2);
                    }
                    cylinder(h = 1.5/2-0.8, r=1/2);
                }
            }
        } 
    } 
    
}


// Valve bypass
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=JUNC_55
    color("red")
    translate([1.5000,0.5000,104.6400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,0/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_57
    color(colorPipe)
    translate([1.5000,0.5000,104.6400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[-0.7000,0.0000,0.0000],[-0.7000,0.5000,0.0000]],2,[0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Valve, Name=V2, Id=JUNC_60
    color(colorValve)
    translate([0.8000,1.0000,104.6400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*270])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.2/2])
            cylinder(h = 0.2, r = 0.1/2, center = true);
            translate([0.2/2, 0, 0.2/4])
            cylinder(h = 0.2/2, r = 0.1/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_62
    color(colorPipe)
    translate([0.8000,1.2000,104.6400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.5000,0.0000],[0.7000,0.5000,0.0000]],2,[0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Connection, Name=Conn, Id=JUNC_65
    color("red")
    translate([1.5000,1.7000,104.6400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,0/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.09793/2);
                    }
                    cylinder(h = 0/2-0, r=0.09793/2);
                }
            }
        } 
    } 
    
}


// Faulty connections
module line(start, end, thickness = 0.010)
{
    $fn=10;
    hull()
    {
        translate(start) sphere(thickness);
        translate(end) sphere(thickness);
    }
}

scale([1000, 1000, 1000])
{
}
