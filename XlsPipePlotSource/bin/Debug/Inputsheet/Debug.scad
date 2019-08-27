// Generated 2019-08-27 22:39:24
$fn=50;
use <curvedPipe.scad>

colorPipe = "grey";
colorTank = "grey";
colorReducer = "red";
colorValve = "red";


// Default flowpath
scale([1000, 1000, 1000])
{
}


// From Tank 1 to Tank 2
scale([1000, 1000, 1000])
{
    // Type=Tank, Name=T1, Id=VOL_25
    color("grey")
    translate([0.0000,0.0000,50.0000])
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
    
    // Type=Connection, Name=Conn, Id=JUNC_27
    color("red")
    translate([0.0000,0.0000,48.5000])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_29
    color(colorPipe)
    translate([0.0000,0.0000,48.5000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,-0.5000],[1.5000,0.0000,-0.5000],[1.5000,0.0000,-1.1000],[1.5000,1.0000,-1.1000]],4,[0.15,0.15,0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Valve, Name=V1, Id=JUNC_34
    color(colorValve)
    translate([1.5000,1.0000,47.4000])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_36
    color(colorPipe)
    translate([1.5000,1.2000,47.4000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,1.0000,0.0000],[0.0000,1.0000,-0.4000],[2.0000,1.0000,-0.4000]],3,[0.15,0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Connection, Name=Connection, Id=JUNC_40
    color("red")
    translate([3.5000,2.2000,47.0000])
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
                        cylinder(h = 0/2, r = 0/2);
                    }
                    cylinder(h = 0/2-0, r=0/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0/2);
                    }
                    cylinder(h = 0/2-0, r=0/2);
                }
            }
        } 
    } 
    
    // Type=Tank, Name=T2, Id=VOL_42
    color("grey")
    translate([3.5000,2.2000,48.0000])
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
    // Type=Connection, Name=Conn, Id=JUNC_47
    color("red")
    translate([1.5000,0.5000,47.4000])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_49
    color(colorPipe)
    translate([1.5000,0.5000,47.4000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[-0.7000,0.0000,0.0000],[-0.7000,0.5000,0.0000]],2,[0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Valve, Name=V2, Id=JUNC_52
    color(colorValve)
    translate([0.8000,1.0000,47.4000])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_54
    color(colorPipe)
    translate([0.8000,1.2000,47.4000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.5000,0.0000],[0.7000,0.5000,0.0000]],2,[0.15,0.15],0.1,0.1-2*0.00305);
    } 
    
    // Type=Connection, Name=Conn, Id=JUNC_57
    color("red")
    translate([1.5000,1.7000,47.4000])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_61
    color(colorPipe)
    translate([0.0000,0.0000,0.0000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.0000]],1,[0],0,0-2*0);
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
