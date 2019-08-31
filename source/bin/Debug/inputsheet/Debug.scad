// Generated 2019-08-31 10:33:24
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


// System 711 before BDAHSG-01 (unit B)
scale([1000, 1000, 1000])
{
    // Type=Pipe, Name=Pipe, Id=PIPE_27
    color(colorPipe)
    translate([0.0000,0.0000,107.2400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[-0.9590,0.0000,0.0000],[-1.8590,0.0000,0.0000],[-1.8590,0.0000,-3.2790]],3,[0.228888,0.215424,0.215424],0.1683,0.1683-2*0.00711);
    } 
    
    // Type=Valve, Name=V9823, Id=COMP_31
    color(colorValve)
    translate([-1.8590,0.0000,103.9610])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0.08/2])
            cylinder(h = 0.08, r = 0.1683/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_33
    color(colorPipe)
    translate([-1.8590,0.0000,103.8810])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,-3.9790],[0.0000,0.7500,-3.9790],[-4.5000,0.7500,-3.9790],[-4.5000,0.7500,-5.9790],[-3.8710,0.7500,-5.9790]],5,[0.215424,0.215424,0.215424,0.215424,0.215424],0.1683,0.1683-2*0.00711);
    } 
    
    // Type=Connection, Name=To-BDAHSG-01, Id=COMP_39
    color("red")
    translate([-5.7300,0.7500,97.9020])
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
                        cylinder(h = 0/2, r = 0.125/2);
                    }
                    cylinder(h = 0/2-0, r=0.125/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.125/2);
                    }
                    cylinder(h = 0/2-0, r=0.125/2);
                }
            }
        } 
    } 
    
}


// System 711 after BDAHSG-01 (unit B)
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=To-BDAHSG-01, Id=COMP_48
    color("red")
    translate([-2.1740,0.7500,97.9020])
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
                        cylinder(h = 0/2, r = 0.125/2);
                    }
                    cylinder(h = 0/2-0, r=0.125/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.125/2);
                    }
                    cylinder(h = 0/2-0, r=0.125/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_50
    color(colorPipe)
    translate([-2.1740,0.7500,97.9020])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.4810,0.0000],[0.0000,-0.4810,3.9000]],2,[0.228888,0.228888],0.1683,0.1683-2*0.00711);
    } 
    
    // Type=Valve, Name=FE648, Id=COMP_53
    color(colorValve)
    translate([-2.1740,0.2690,101.8020])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*90,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0.08/2])
            cylinder(h = 0.08, r = 0.1683/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_55
    color(colorPipe)
    translate([-2.1740,0.2690,101.8820])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,1.9000]],1,[0.215424],0.1683,0.1683-2*0.00711);
    } 
    
    // Type=Valve, Name=V9824, Id=COMP_57
    color(colorValve)
    translate([-2.1740,0.2690,103.7820])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*90,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0.076/2])
            cylinder(h = 0.076, r = 0.1683/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_59
    color(colorPipe)
    translate([-2.1740,0.2690,103.8580])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,2.5900],[0.0000,0.8530,2.5900]],2,[0.215424,0.215424],0.1683,0.1683-2*0.00711);
    } 
    
}


// Relief valve 711-9829
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_65
    color("red")
    translate([-2.1740,0.2690,98.2740])
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
    
    // Type=Pipe, Name=Pipe, Id=PIPE_67
    color(colorPipe)
    translate([-2.1740,0.2690,98.2740])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.1237,-0.1237,0.0000],[0.5987,-0.1237,0.0000],[0.5987,-0.5717,0.0000],[0.5987,-0.5717,0.1140]],4,[0.113792,0.113792,0.113792,0.113792],0.0889,0.0889-2*0.00549);
    } 
    
    // Type=ReducerAuto, Name=, Id=-
    color(colorPipe)
    translate([-1.5753,-0.3027,98.3880])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_71
    color(colorPipe)
    translate([-1.5753,-0.3027,98.3880])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.1430]],1,[0.0772096],0.06032,0.06032-2*0.00391);
    } 
    
    // Type=ReducerAuto, Name=, Id=-
    color(colorPipe)
    translate([-1.5753,-0.3027,98.5310])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_72
    color(colorPipe)
    translate([-1.5753,-0.3027,98.5310])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.2340]],1,[0.0617728],0.04826,0.04826-2*0.00368);
    } 
    
    // Type=Valve, Name=SV9829, Id=COMP_74
    color(colorValve)
    translate([-1.5753,-0.3027,98.7650])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.125/2, center = true);
            translate([0/2, 0, 0/4])
            cylinder(h = 0/2, r = 0.125/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_76
    color(colorPipe)
    translate([-1.5753,-0.3027,98.7650])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.6450,0.0000,0.0000]],1,[0.09],0.06,0.06-2*0.00305);
    } 
    
}


// BDAHSG-01  unit A
scale([1000, 1000, 1000])
{
    // Type=Tank, Name=BDAHSG-01A, Id=VOL_86
    color("grey")
    translate([-0.9300,0.7500,98.7400])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*180])
        { 
            rotate([0,90,0])
            translate([0,0,4.8/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,4.8/2]) translate([0,0,-0.2016125]) sphere(r=0.2016125);
                        cylinder(h = 4.8/2, r = 0.403225/2);
                    }
                    cylinder(h = 4.8/2-0.2016125, r=0.403225/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,4.8/2]) translate([0,0,-0.2016125]) sphere(r=0.2016125);
                        cylinder(h = 4.8/2, r = 0.403225/2);
                    }
                    cylinder(h = 4.8/2-0.2016125, r=0.403225/2);
                }
            }
        } 
    } 
    
}


// BDAHSG-01  unit B
scale([1000, 1000, 1000])
{
    // Type=Tank, Name=BDAHSG-01B, Id=VOL_91
    color("grey")
    translate([-0.9300,0.7500,97.9020])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*180])
        { 
            rotate([0,90,0])
            translate([0,0,4.8/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,4.8/2]) translate([0,0,-0.2524125]) sphere(r=0.2524125);
                        cylinder(h = 4.8/2, r = 0.504825/2);
                    }
                    cylinder(h = 4.8/2-0.2524125, r=0.504825/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,4.8/2]) translate([0,0,-0.2524125]) sphere(r=0.2524125);
                        cylinder(h = 4.8/2, r = 0.504825/2);
                    }
                    cylinder(h = 4.8/2-0.2524125, r=0.504825/2);
                }
            }
        } 
    } 
    
}


// Connecting pipe from shell outlet of unit A to inlet of tube package 1 in unit B (B1)
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_95
    color("red")
    translate([-2.0716,0.7500,98.7400])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_97
    color(colorPipe)
    translate([-2.0716,0.7500,98.7400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.4159],[-0.1625,0.3751,0.4159],[-0.1625,0.3751,-1.3780],[0.5003,0.3751,-1.3780]],4,[0.13335,0.13335,0.13335,0.13335],0.0889,0.0889-2*0.01112);
    } 
    
    // Type=Connection, Name=Flange, Id=COMP_102
    color("red")
    translate([-1.5713,1.1251,97.3620])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_104
    color(colorPipe)
    translate([-1.5713,1.1251,97.3620])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.2413,0.0000,0.0000],[0.2413,0.0000,0.5400],[0.2413,-0.3751,0.5400]],3,[0.13335,0.13335,0.13335],0.0889,0.0889-2*0.01112);
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_108
    color("red")
    translate([-1.3300,0.7500,97.9020])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
}


// Connecting pipe from outlet B1 to inlet tube side unit A
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_113
    color("red")
    translate([-1.3300,0.7500,97.9020])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_115
    color(colorPipe)
    translate([-1.3300,0.7500,97.9020])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.3800,0.3800],[0.4000,0.3800,0.3800],[0.4000,0.3800,1.2400],[0.0000,0.1800,1.2400],[0.0000,-0.0010,0.8250]],5,[0.13335,0.13335,0.13335,0.13335,0.13335],0.0889,0.0889-2*0.01112);
    } 
    
}


// Connecting pipe from tube side outlet of unit A  to inlet of B2
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_124
    color("red")
    translate([-1.3300,0.7500,98.7400])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_126
    color(colorPipe)
    translate([-1.3300,0.7500,98.7400])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.1810,0.4150],[0.4000,-0.3810,0.4150],[0.4000,-0.3810,-0.8380],[0.0000,-0.3810,-0.8380],[0.0000,0.0000,-0.8380]],5,[0.13335,0.13335,0.13335,0.13335,0.13335],0.0889,0.0889-2*0.01112);
    } 
    
}


// Connecting pipe from B2 outlet
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_134
    color("red")
    translate([-1.3300,0.7500,97.9020])
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
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0/2]) translate([0,0,-0]) sphere(r=0);
                        cylinder(h = 0/2, r = 0.0889/2);
                    }
                    cylinder(h = 0/2-0, r=0.0889/2);
                }
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_136
    color(colorPipe)
    translate([-1.3300,0.7500,97.9020])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.3269,-0.4669]],1,[0.13335],0.0889,0.0889-2*0.01112);
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
