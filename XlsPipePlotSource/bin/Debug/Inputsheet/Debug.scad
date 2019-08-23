// Generated 2019-08-23 20:43:47
$fn=50;
use <curvedPipe.scad>

colorPipe = "grey";
colorReducer = "grey";
colorValve = "red";
colorTank = "grey";


// Default flowpath
scale([1000, 1000, 1000])
{
}


// From VCT to common suction header 8"
scale([1000, 1000, 1000])
{
    // Type=Tank, Name=T1, Id=TANK_25
    color("grey")
    translate([0.0000,0.0000,50.0000])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 2/2])
            {
                cylinder(h = 2-1.2, r = 1.2/2, center = true);
                translate([0,0,(2-1.2)/2])
                sphere(r = 1.2/2);
                translate([0,0,-(2-1.2)/2])
                sphere(r = 1.2/2);
            }
        } 
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_27
    color("red")
    translate([0.0000,0.0000,48.0000])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.09793/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_29
    color("grey")
    translate([0.0000,0.0000,48.0000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,-0.5000],[0.9998,1.6997,-0.5000],[0.9998,1.6997,-2.0000],[0.9998,2.1997,-2.0000],[0.9998,2.1997,-3.7000],[0.9998,1.7267,-3.7000]],6,[0.17145,0.17145,0.17145,0.17145,0.17145,0.17145],0.1143,0.1143-2*0.00305);
    } 
    
    // Type=Valve, Name=LCV115C, Id=COMP_36
    color("red")
    translate([0.9998,1.7267,44.3000])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.355/2])
            cylinder(h = 0.355, r = 0.125/2, center = true);
            translate([0.355/2, 0, 0.355/4])
            cylinder(h = 0.355/2, r = 0.125/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_38
    color("grey")
    translate([0.9998,1.3717,44.3000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.6450,0.0000]],1,[0.17145],0.1143,0.1143-2*0.00305);
    } 
    
    // Type=Valve, Name=LCV115E, Id=COMP_40
    color("red")
    translate([0.9998,0.7267,44.3000])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.355/2])
            cylinder(h = 0.355, r = 0.125/2, center = true);
            translate([0.355/2, 0, 0.355/4])
            cylinder(h = 0.355/2, r = 0.125/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_42
    color("grey")
    translate([0.9998,0.3717,44.3000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.3800,0.0000],[0.0000,-0.4850,0.0000]],2,[0.17145,0.17145],0.1143,0.1143-2*0.00305);
    } 
    
    // Type=Valve, Name=V8440, Id=COMP_45
    color("red")
    translate([0.9998,-0.1133,44.3000])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.355/2])
            cylinder(h = 0.355, r = 0.125/2, center = true);
            translate([0.355/2, 0, 0.355/4])
            cylinder(h = 0.355/2, r = 0.125/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_47
    color("grey")
    translate([0.9998,-0.4683,44.3000])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.1520,0.0000],[-1.2140,-0.1520,0.0000],[-1.8136,0.4476,0.0000],[-1.8136,0.7976,0.0000],[-1.8136,0.7976,-5.3000],[0.2364,0.7976,-5.3000],[0.1492,0.7976,-5.4878],[0.1492,0.7976,-7.0978]],8,[0.17145,0.17145,0.17145,0.17145,0.17145,0.17145,0.17145,0.17145],0.1143,0.1143-2*0.00305);
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_56
    color("red")
    translate([1.1491,0.3294,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.125/2, center = true);
        } 
    } 
    
}


// From RWST
scale([1000, 1000, 1000])
{
    // Type=Pipe, Name=Pipe, Id=PIPE_60
    color("grey")
    translate([-2.7509,-25.3146,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,12.0000,0.0000],[5.2500,12.0000,0.0000],[5.2500,20.0000,0.0000],[5.2500,20.2160,0.0000]],4,[0.4095,0.4095,0.4095,0.4095],0.273,0.273-2*0.00419);
    } 
    
    // Type=Reducer, Name=Conn, Id=COMP_65
    color("red")
    translate([2.4991,-5.0986,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*270])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.178/2])
            cylinder(h = 0.178, r1 = 0.273/2, r2 = 0.2198/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_67
    color("grey")
    translate([2.4991,-4.9206,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,10.1560,0.0000],[-1.3500,10.1560,0.0000],[-1.3500,9.5100,0.0000]],3,[0.3297,0.3297,0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=LCV115B, Id=COMP_71
    color("red")
    translate([1.1491,4.5894,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_73
    color("grey")
    translate([1.1491,4.0814,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-1.0240,0.0000]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=V8127A, Id=COMP_75
    color("red")
    translate([1.1491,3.0574,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_77
    color("grey")
    translate([1.1491,2.5494,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.8780,0.0000]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=V8127B, Id=COMP_79
    color("red")
    translate([1.1491,1.6714,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_81
    color("grey")
    translate([1.1491,1.1634,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-1.6240,0.0000]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=V8128A, Id=COMP_83
    color("red")
    translate([1.1491,-0.4606,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_85
    color("grey")
    translate([1.1491,-0.9686,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.9920,0.0000]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=V8128B, Id=COMP_87
    color("red")
    translate([1.1491,-1.9606,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_89
    color("grey")
    translate([1.1491,-2.4686,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-1.7920,0.0000]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Valve, Name=LCV115D, Id=COMP_91
    color("red")
    translate([1.1491,-4.2606,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*90])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.22/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.22/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_93
    color("grey")
    translate([1.1491,-4.7686,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,-0.5460,0.0000],[1.3500,-0.5460,0.0000]],2,[0.3297,0.3297],0.2198,0.2198-2*0.00376);
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_96
    color("red")
    translate([2.4991,-5.3146,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.22/2, center = true);
        } 
    } 
    
}


// Towards charging pump 2
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_101
    color("red")
    translate([1.1491,0.7854,37.2022])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.1524/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_103
    color("grey")
    translate([1.1491,0.7854,37.2022])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.5000,0.0000,0.0000],[0.5000,0.0000,1.4900],[0.5000,1.4900,1.4900],[1.9900,1.4900,1.4900],[1.9900,1.4900,4.6900],[2.1970,1.2343,4.6900]],6,[0.2286,0.2286,0.2286,0.2286,0.2286,0.2286],0.1524,0.1524-2*0.00376);
    } 
    
    // Type=Valve, Name=V8471B, Id=COMP_110
    color("red")
    translate([3.3461,2.0197,41.8922])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*51])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.508/2])
            cylinder(h = 0.508, r = 0.1524/2, center = true);
            translate([0.508/2, 0, 0.508/4])
            cylinder(h = 0.508/2, r = 0.1524/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_112
    color("grey")
    translate([3.6658,1.6249,41.8922])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.1888,-0.2331,0.0000],[0.7621,-0.9411,0.0000],[0.7621,-0.9411,-0.2290]],3,[0.2286,0.2286,0.2286],0.1524,0.1524-2*0.00376);
    } 
    
    // Type=Reducer, Name=Conn, Id=COMP_116
    color("red")
    translate([4.4279,0.6838,41.6632])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*-90,-1*0])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.152/2])
            cylinder(h = 0.152, r1 = 0.1524/2, r2 = 0.2198/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_118
    color("grey")
    translate([4.4279,0.6838,41.5112])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,-0.2290]],1,[0.3297],0.2198,0.2198-2*0.00376);
    } 
    
}


// Connects 0.5 m after V8471B
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_123
    color("red")
    translate([3.9805,1.2363,41.8922])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.02667/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_125
    color("grey")
    translate([3.9805,1.2363,41.8922])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.1000]],1,[0.040005],0.02667,0.02667-2*0.00287);
    } 
    
    // Type=Reducer, Name=Connection, Id=COMP_127
    color("red")
    translate([3.9805,1.2363,41.9922])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*90,-1*0])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.03/2])
            cylinder(h = 0.03, r1 = 0.02667/2, r2 = 0.0213/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_129
    color("grey")
    translate([3.9805,1.2363,42.0222])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.1600],[0.0000,-0.1495,0.2198],[-0.3073,-0.1495,0.3400],[-0.3073,0.1439,0.4601],[-0.0978,0.1439,0.5499]],5,[0.03195,0.03195,0.03195,0.03195,0.03195],0.0213,0.0213-2*0.00277);
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_135
    color("red")
    translate([3.8827,1.3802,42.5722])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.0213/2, center = true);
        } 
    } 
    
    // Type=Tank, Name=CSTRCH-02, Id=VOL_137
    color("grey")
    translate([3.8327,1.3802,42.6422])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0,0,0.45/2])
            union()
            {
                union()
                {
                    intersection()
                    {
                        translate([0,0,0.45/2]) translate([0,0,-0.273]) sphere(r=0.273);
                        cylinder(h = 0.45/2, r = 0.2731/2);
                    }
                    cylinder(h = 0.45/2-0.273, r=0.2731/2);
                }
                mirror([0,0,1])
                union()
                {
                    intersection()
                    {
                        translate([0,0,0.45/2]) translate([0,0,-0.273]) sphere(r=0.273);
                        cylinder(h = 0.45/2, r = 0.2731/2);
                    }
                    cylinder(h = 0.45/2-0.273, r=0.2731/2);
                }
            }
        } 
    } 
    
    // Type=Connection, Name=Conn, Id=COMP_139
    color("red")
    translate([4.2827,1.3802,42.6422])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.0213/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_141
    color("grey")
    translate([4.1777,1.3802,42.7787])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[-0.0087,0.0000,0.0996],[0.4838,0.4925,0.1693],[0.4838,1.0099,0.2210],[0.4838,1.0099,1.2210],[-0.0137,1.0099,1.2708]],5,[0.03195,0.03195,0.03195,0.03195,0.03195],0.0213,0.0213-2*0.00277);
    } 
    
    // Type=Valve, Name=Connection, Id=COMP_147
    color("red")
    translate([4.1640,2.3901,44.0495])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*5.71059313766286,-1*180])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.05/2])
            cylinder(h = 0.05, r = 0.0213/2, center = true);
            translate([0.05/2, 0, 0.05/4])
            cylinder(h = 0.05/2, r = 0.0213/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_149
    color("grey")
    translate([4.1142,2.3901,44.0545])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[-1.9652,0.0000,0.1965],[-1.9652,0.1000,0.3965],[-1.9652,0.1000,1.6165],[-1.9652,-0.4721,1.6737],[-1.9652,-0.4721,3.2937],[-1.9652,-0.3229,3.3087],[-5.0001,-0.3229,3.6121],[-5.0001,-0.3229,5.7121],[-5.0001,-0.5223,5.9116],[-5.0001,-0.5223,6.6016]],10,[0.03195,0.03195,0.03195,0.03195,0.03195,0.03195,0.03195,0.03195,0.03195,0.03195],0.0213,0.0213-2*0.00277);
    } 
    
    // Type=Valve, Name=Connection, Id=COMP_160
    color("red")
    translate([-0.8858,1.8678,50.6560])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*90,-1*0])
        { 
            rotate([0, 90, 0])
            translate([0, 0, 0.1/2])
            {
                cylinder(h = 0.1-0.06, r = 0.06/2, center = true);
                translate([0,0,(0.1-0.06)/2])
                sphere(r = 0.06/2);
                translate([0,0,-(0.1-0.06)/2])
                sphere(r = 0.06/2);
            }
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_162
    color("grey")
    translate([-0.8758,1.8678,50.7560])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0000,0.0000,0.2820]],1,[0.03195],0.0213,0.0213-2*0.00277);
    } 
    
}


// Towards
scale([1000, 1000, 1000])
{
    // Type=Connection, Name=Conn, Id=COMP_167
    color("red")
    translate([4.2827,1.3802,42.6422])
    { 
        translate([0.0000,0.0000,0.0000])
        rotate([0,-1*0,-1*0])
        { 
            rotate([0,90,0])
            translate([0, 0, 0/2])
            cylinder(h = 0, r = 0.0213/2, center = true);
        } 
    } 
    
    // Type=Pipe, Name=Pipe, Id=PIPE_169
    color("grey")
    translate([4.2327,1.3802,42.7422])
    { 
        curvedPipe([[0.0000,0.0000,0.0000],[0.0839,0.0000,0.0545],[0.0839,0.1879,0.1229],[0.3879,0.4920,0.1229],[0.3879,1.0592,0.1796],[0.3879,1.0592,1.0296],[0.3879,1.0592,1.8796],[0.5538,1.0592,1.9167]],7,[0.03195,0.03195,0.03195,0.03195,0.03195,0.03195,0.03195],0.0213,0.0213-2*0.00277);
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
