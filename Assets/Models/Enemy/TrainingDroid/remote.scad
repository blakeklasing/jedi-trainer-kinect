Q=15;
radius=75;
application_thickness=0.5;
main_angle=117.7;
A=359;
hollow=true;
show_rims=false;
show_top=false;
show_bottom=true;

module rounded_rectangle(h, w, r) {
  offset(r = r, $fn=Q*3*r)
    square([h - r*2, w - r*2], center=true);
}

module rounded_poly(pts, r) {
  offset(r = r, $fn=Q*3*r)
    polygon(pts, convexity=4);
}

module x_mirror() {
  children();
  mirror([1,0,0]) children();
}

module y_mirror() {
  children();
  mirror([0,1,0]) children();
}

module z_mirror() {
  children();
  mirror([0,0,1]) children();
}

module mr() {
  if (show_top) children();
  if (show_bottom) rotate([180,0,45+90]) { children(); }
}

module pyra() {
  translate([0,0,-80]) linear_extrude(height=20, convexity=4, scale=1.0) { children(); }
}

module pyra2() {
  render() intersection() {
    translate([0,0,-80]) linear_extrude(height=20, convexity=4, scale=1.0) { children(); }
    intersection() {
     translate([0,0,-radius*2]) cylinder(r=radius * 0.5, h=radius*2, $fn=8);
      rotate([0,90,0]) difference() {
      sphere(r=radius+application_thickness, $fn=Q*30);
      sphere(r=radius-1, $fn=Q*10);
    }
    }
  }
}

module remoteA() {
  difference() {
    union() {
      difference() {
        sphere(r=radius, $fn=Q*10);
        if (hollow) {
          sphere(r=radius-1.5, $fn=Q*10);
          mr()
            for (z=[0:90:A])
              rotate([0,main_angle, z+45])
                pyra()
                  circle(r=22/2, $fn=Q*5);
        }
      }

    color([0.5,0.5,0.5]) intersection() {
      sphere(r=radius+application_thickness, $fn=Q*10);
      union() {
        for (z=[45/2:45:350])
          rotate([0,90,z])
            pyra()
              rounded_rectangle(15, 30, 0.5);
        
        mr() union() {
          for (z=[0:90:A]) {
            for (x=[-6.5,0,6.5]) {
              rotate([0,0,x])
                rotate([0,main_angle,z])
                  pyra()
                    rounded_rectangle(14.3, 6.4, 0.5);
            }
          }

          for (z=[0:90:A]) {
            rotate([0,main_angle,z+45]) pyra() difference() {
              circle(r=54/2, $fn=Q*5);
              circle(r=32/2, $fn=Q*5);
              square([100,7], center=true);
            }
          }
        }
      }
    }
  }
  if (hollow) {
    sphere(r=radius-2, $fn=Q*10);
  }
}
}


// Peg's seems to be slightly too big.
PEGSIZE=8.0;
module peg1() {
  render() {
    translate([0,0,-1]) cylinder(r=PEGSIZE/2, h=2.5, $fn=Q*3);
    cylinder(r=0.7, h=3.5, $fn=Q*2);
  }
}

module peg2() {
  render() {
    translate([0,0,-1]) cylinder(r=PEGSIZE/2, h=2.5, $fn=Q*3);
    difference() {
      cylinder(r=1, h=3.5, $fn=Q*2);
      cylinder(r=0.7, h=10, $fn=Q*2);
    }
  }
}

module peg3() {
  render() {
    translate([0,0,-1]) cylinder(r=6.35/2, h=2, $fn=Q*3);
    translate([0,0,-1]) cylinder(r=4.5/2, h=2.55, $fn=Q*3);
    cylinder(r=0.7, h=4.5, $fn=Q*2);
    cylinder(r=0.6, h=5.5, $fn=Q*2);
  }
}

module peg4() {
  color([0.3,0.3,0.3]) render() {
    cylinder(r=2.5, h=1, $fn=Q*2);
    translate([0,0,0.99]) cylinder(r1=2.5, r2=1.7, h=1.5, $fn=Q*2);
  }
}


module dome(dia) {
  rotate_extrude(convexity=10, $fn=Q*3) intersection() {
  translate([0,-5]) square([10,10]);
  offset(0.2) offset(-0.2) intersection() {
    translate([0,-7]) circle(r=9, $fn=200);
    square([dia, 9], center=true);
  }
}
}

module gunport() {
  color([0.3,0.3,0.3]) render() difference() {
    dome(9);

  rotate([0,0,10])
  translate([-1.5,1,0.5]) cube([3,5,5]);
  rotate([0,0,-10])
  translate([-1.5,1,0.5]) cube([3,5,5]);
    translate([0,0,-0.7]) dome(7.5);
  translate([-10,-10,-21]) cube([20,20,20]);
  }
}

thickness=0.8;
wiggle=0.01;
module torus(t) {
  rotate_extrude(convexity = 10, $fn=40) translate([thickness,0,0]) circle(r=t/2-wiggle, $fn=20);
}

// Openscad doesn't like recursion, so replicate this function 3 times.
module stretchA(l) {
  for (m=[0:1]) {
    mirror([0,0,m]) {
      translate([0,0,-l/2-0.001]) {
        difference() {
          union() { children(); }
          translate([-100,-100, 0 ]) cube([200, 200, 200]);
        }
      }
    }
  }
  linear_extrude(height=l, center=true, convexity=10) {
    projection(cut=true) union() { children(); }
  }
}
module stretchB(l) {
  for (m=[0:1]) {
    mirror([0,0,m]) {
      translate([0,0,-l/2-0.001]) {
        difference() {
          union() { children(); }
          translate([-100,-100, 0 ]) cube([200, 200, 200]);
        }
      }
    }
  }
  linear_extrude(height=l, center=true, convexity=10) {
    projection(cut=true) union() { children(); }
  }
}
module stretchC(l) {
  for (m=[0:1]) {
    mirror([0,0,m]) {
      translate([0,0,-l/2-0.001]) {
        difference() {
          union() { children(); }
          translate([-100,-100, 0 ]) cube([200, 200, 200]);
        }
      }
    }
  }
  linear_extrude(height=l, center=true, convexity=10) {
    projection(cut=true) union() { children(); }
  }
}


module link(t, x, y, z) {
  stretchA(z) rotate([90,0,0])
  stretchB(y) rotate([0,90,0])
  stretchC(x) rotate([90,0,0])
  torus(thickness);
}

module u() {
  render() intersection() {
    link(1,1,3,0);
    translate([-5,-1,-5]) cube([10,11,10]);
  }
  translate([1.3,-1, 0]) cube([1,1,1], center=true);
  translate([-1.3,-1, 0]) cube([1,1,1], center=true);
  translate([-1.3,-1, 0]) rotate([0,90,0]) cylinder(r=0.2, $fn=20, h=0.8);
}

module tt() {
  difference() {
    linear_extrude(height=14.5, convexity=10, center=true) hull() {
      translate([-4.5,0]) circle(r=0.6, $fn=Q);
      translate([4.5,0]) circle(r=0.6, $fn=Q);
    }

    S=3.7;
    N=3;
    for (x=[-S*N:S:S*N]) {
      translate([4.5,0,x]) cube([3,2,S/2], center=true);
      translate([-4.5,0,x+2]) cube([3,2,S/2], center=true);
    }

    z_mirror() x_mirror() translate([1.5, 0, S]) cube([1.5,10,1.5], center=true);
  }
}

module tt_single() {
  difference() {
    union() {
      linear_extrude(height=14.5, convexity=10, center=true) hull() {
        translate([-2,0]) circle(r=0.75, $fn=Q);
        translate([2,0]) circle(r=0.75, $fn=Q);
      }

      z_mirror() hull() {
      translate([0,1.8,S-1]) rotate([0,90,0]) cylinder(r=0.2, h=1, center=true, $fn=Q);
      translate([0,0,S-1]) rotate([0,90,0]) cylinder(r=0.4, h=1.4, center=true, $fn=Q);
      }
    }

    S=3.7;
    N=3;
    for (x=[-S*N:S:S*N]) {
      translate([2.5,0,x]) cube([3,2,S/2], center=true);
      translate([-2.5,0,x+S/2]) cube([3,2,S/2], center=true);
    }

    z_mirror() translate([0, 0, S]) cube([1.5,10,1.5], center=true);
  }
}

module tt() {
  render() translate([0,0,0.5]) rotate([90,0,0]) {
    translate([-3.5/2,0,0]) tt_single();
    translate([3.5/2,0,0]) tt_single();
  }
}


module rim() {
  color([0.9, 0.9, 0.9]) render() union() {
     difference() {
    union() {
      cylinder(r=21.7/2, h=11.2, $fn=Q*4);
      translate([0,0,10.5]) cylinder(r=22.8/2, h=0.7, $fn=Q*4);
    }
    translate([0,0,11]) cylinder(r=21.76/2, h=1, $fn=Q*4);

    // TODO(hubbe): Replicate bottom curve better
    R=1;
    minkowski() {
      sphere(r=R, $fn=Q*3); 
      translate([0,0,1+R]) cylinder(r1=18.73/2-R, r2=19.5/2-R, h=11, $fn=Q*5);
    }

    translate([0,0,-1]) cylinder(r=8.26/2, h=11, $fn=Q*4);
    for (a=[0:360/10:360]) {
      rotate([0,0,a]) {
        translate([15.7/2,0,-1]) cylinder(r=1, h=11, $fn=Q);
      }
    }
  }
  for (a=[360/20:360/10:360]) {
    rotate([0,0,a]) {
      translate([10.5/2,0,0.5]) cylinder(r=0.7, h=1, $fn=6);
    }
  }
  }
}

// rim();


module clamp() {
  W=2.8;
  D=3;
  scale(1.15) {
  linear_extrude(height=1, convexity=10) union() {
  hull() {
    circle(r=2, $fn=Q*2);
    translate([W,D]) circle(r=0.5, $fn=Q);
    translate([-W,D]) circle(r=0.5, $fn=Q);
  }
  hull() {
    translate([W,D]) circle(r=0.5, $fn=Q);
    translate([W,7]) circle(r=0.5, $fn=Q);
  }
  hull() {
    translate([-W,D]) circle(r=0.5, $fn=Q);
    translate([-W,7]) circle(r=0.5, $fn=Q);
  }
  translate([0,3.5]) square([6,3], center=true);
  }

  cylinder(r=2, h=1.3, $fn=Q*2);
  cylinder(r1=1.8, r2=1.5, h=2.5, $fn=Q*2);
  }
}

//clamp();


module turret() {
  S=1;
  render() scale(0.5) difference() {
    union() {
      minkowski() {
        sphere(r=S, $fn=Q*3);
        intersection() {
          cylinder(r1=2.54 * 8 - S, r2=2.54 * 7 - S, 10, $fn=Q*4);
          translate([0,0,-90]) sphere(r=100, $fn=Q*4);
        }
      }
     translate([0,13.5,9.8]) difference() {
        cylinder(r=1.8, h=1.8, $fn=Q);
        translate([0,0,-1]) cylinder(r=1, h=5, $fn=Q);
      }

      translate([0,13.5,9])
      linear_extrude(height=2.8, convexity=4)
      intersection() {
        difference() {
  hull() {
    circle(r=3.3,$fn=Q);
    translate([0,-3]) circle(r=3.3,$fn=Q);
    translate([-9,3]) circle(r=3.3,$fn=Q);
  }
  hull() {
    circle(r=2.3,$fn=Q);
    translate([0,-3]) circle(r=2.3,$fn=Q);
    translate([-9,3]) circle(r=2.3,$fn=Q);
  }
  }
  translate([-5,-1.5]) square([10,10]);
  translate([-5,-2]) rotate(-20) square([10,10]);
}

     for (r = [-150:50:180]) rotate(r)
        translate([0,-12,9])
        rotate([90,0,0])
       difference() {
        linear_extrude(height=5, convexity=4) 
          offset(r=0.8, $fn=Q) square([4,4], center=true);
       }

       translate([0,-14.5,11])
         cylinder(r=0.6, h=1.2, $fn=Q);
       rotate(-50) translate([0,-14.5,11])
         cylinder(r=0.6, h=1.2, $fn=Q);
    }
    translate([0,0,-5]) cylinder(r=10, h=20, $fn=Q*3);
    translate([0,0,-5]) cylinder(r1=19, r2=17, h=13.7, $fn=Q*3);
    translate([0,0,9.8]) cylinder(r=10.5, h=13.5, $fn=Q*3);
    rotate([0,0,55]) translate([-0.25,0,9.8]) cube([0.5,20,1]);
    rotate([0,0,-55]) translate([-0.25,0,9.8]) cube([0.5,20,1]);
    rotate([0,0,180+25]) translate([-0.25,0,9.8]) cube([0.5,20,1]);

     for (r = [-150:50:180]) rotate(r)
        translate([0,-14.6,9])
         cube([4,5,4], center=true);

  }
}


module lid() {
  render()
  difference() {
  union() {

  rotate_extrude(convexity=10, $fn=200) intersection() {
    translate([0,-5]) square([10,10]);
    offset(0.3) intersection() {
//      translate([0,-19.1]) circle(r=10, $fn=300);
      translate([0,-16.2]) circle(r=17, $fn=300);
      square([2.54*4, 20], center=true);
    }
  }

  scale(1.3) hull() {
   translate([0,2,1]) sphere(r=0.2, $fn=Q);
   translate([0,4,1]) sphere(r=0.2, $fn=Q);
   translate([0,2,0]) sphere(r=0.3, $fn=Q);
   translate([0,4,0]) sphere(r=0.3, $fn=Q);
  }
  scale(1.3) hull() {
    cylinder(r=0.5, h=1.0, $fn=Q);
    translate([0,5.5,0]) cylinder(r=0.5, h=1.0, $fn=Q);
  }
  }
  scale(1.3) {
    translate([0,  0,0.9]) cylinder(r=0.35, h=0.6, $fn=Q);
    translate([0,5.5,0.9]) cylinder(r=0.35, h=0.6, $fn=Q);
    translate([0, 0,1]) rotate([0,0,45]) cube([0.2,0.57,0.5], center=true);
    translate([0, 5.5,1]) rotate([0,0,45]) cube([0.2,0.57,0.5], center=true);
    translate([-10,-10,-21]) cube([20,20,20]);
  }
  }

  scale(1.3) x_mirror() translate([0.7,0,0]) hull() {
   translate([0,0,  0.9]) sphere(r=0.2, $fn=Q);
   translate([0,3.8, 0.9]) sphere(r=0.2, $fn=Q);

   translate([-0.4,  0,0]) sphere(r=0.2, $fn=Q);
   translate([-0.4,3.8,0]) sphere(r=0.2, $fn=Q);
   translate([+0.4,  0,0]) sphere(r=0.2, $fn=Q);
   translate([+0.4,3.8,0]) sphere(r=0.2, $fn=Q);
  }

  scale(1.3) translate([0,0,0.9]) intersection()  {
    translate([-10,-20,-10]) cube([20,20,20]);
   rotate_extrude(convexity=10, $fn=Q*2) translate([0.7,0]) circle(r=0.2, $fn=Q);
  }
}

module nipple() {
   render() {
   difference() {
     translate([0,0,radius-1]) cylinder(r=1.7, h=2, $fn=Q*2);
     translate([0,0,radius-2]) cylinder(r=0.8, h=8, $fn=Q*2);
   }
   difference() {
    intersection() {
       sphere(r=radius+0.2, $fn=Q*30);
       translate([0,0,radius-1]) cylinder(r=7, h=10, $fn=Q*4);
     }
     for (a=[0:90:359]) {
       rotate([0,0,a])
         translate([0,5,radius-1]) cylinder(r=0.7, h=10, $fn=Q*4);
     }
   }
   }
}

module CLIP() {
  if (show_top && show_bottom)
    children();
  else {
    intersection() {
      union() { children(); }
      translate([0,0,(show_top ? 100 : 0) + (show_bottom? -100 : 0)]) {
        cube([200,200,200], center=true);
      }
    }
  }
}

module remote() {
  CLIP() {
  // Base sphere
  color([0.8,0.8,0.7])
  difference() {
    sphere(r=radius, $fn=Q*30);
    if (hollow) {
      sphere(r=radius-1.5, $fn=Q*10);
      mr()
      for (z=[0:90:A])
        rotate([0,main_angle, z+45])
         pyra()
           circle(r=22/2, $fn=Q*5);
    }
  }


  // Patches
  color([0.5,0.5,0.5]) {
    // Equator patches
    for (z=[45/2:45:350])
      rotate([0,90,z])
        pyra2()
          rounded_rectangle(15, 30, 0.5);
  
    mr() {
      for (z=[0:90:A]) {
        // Triple patches
        for (x=[-7.7,0,7.7]) {
          H=14.5;
          W=8;
          R=0.5;
          T=0.4;
          rotate([0,0,x])
            rotate([0,main_angle,z])
             pyra2()
//                rounded_rectangle(14.3, 6.4, 0.5);
                rounded_poly([[-H/2+R, -W/2+R+T],
                              [-H/2+R, W/2-R-T],
                              [H/2-R, W/2-R],
                              [H/2-R, -W/2+R]], 0.5);
        }

        // Rounded patches
        rotate([0,main_angle,z+45]) pyra2() difference() {
          circle(r=54/2, $fn=Q*5);
          circle(r=32/2, $fn=Q*5);
          square([100,7], center=true);
        }
      }
    }

    // Bottom patch
    if (show_bottom) pyra2() difference() {
      circle(r=12, $fn=Q*5);
      circle(r=4, $fn=Q*4);
    }
  }

  PEGA = 17.6;
  if (show_top) rotate([180,0,45+90])
  for (z=[0:90:A]) {
   rotate([0,main_angle,z]) {
     rotate([PEGA,0,0]) translate([0,0,radius+application_thickness]) peg1();
     rotate([-PEGA,0,0]) translate([0,0,radius+application_thickness]) peg1();
     rotate([0,18.5,0]) translate([0,0,radius]) peg3();
     rotate([0,-18.5,0]) translate([0,0,radius]) peg3();
     rotate([-12.5,0,0]) translate([0,0,radius+0.5]) u();
     rotate([-12.5,0,180]) translate([0,0,radius+0.5]) u();
    }
  }
  if (show_bottom) rotate([180,0,45+90])
  for (z=[0:90:A]) {
    rotate([19,0,z]) translate([0,0,radius]) tt();
    rotate([28,0,z]) translate([0,0,radius]) peg2();
    rotate([37.5,0,z]) translate([0,0,radius]) peg4();
    rotate([-48,0,z]) translate([0,0,radius]) gunport();
  }

  if (show_bottom) for (z=[0:90:A]) {
   rotate([0,main_angle,z]) {
     rotate([PEGA,0,0]) translate([0,0,radius+application_thickness]) peg2();
     rotate([-PEGA,0,0]) translate([0,0,radius+application_thickness]) peg2();
     rotate([0,18.5,0]) translate([0,0,radius]) peg3();
     rotate([0,-18.5,0]) translate([0,0,radius]) peg3();
     rotate([-12.5,0,0]) translate([0,0,radius+0.5]) u();
     rotate([-12.5,0,180]) translate([0,0,radius+0.5]) u();
    }
  }
  if (show_top) for (z=[0:90:A]) {
    rotate([19,0,z]) translate([0,0,radius]) tt();
    rotate([28,0,z]) translate([0,0,radius]) peg1();
    rotate([37.5,0,z]) translate([0,0,radius]) peg4();
    rotate([-48,0,z]) translate([0,0,radius]) gunport();
  }

  if (show_rims) mr() for (z=[0:90:A]) {
   rotate([0,main_angle+180,z+45]) {
     translate([0,0,radius-11]) rim();
   }
  }
  mr() for (z=[0:90:A]) {
    rotate([76,0,z]) translate([0,0,radius-0.5]) turret();
  }
  if (show_top) for (z=[0:90:A]) {
    rotate([ 9.2,23.1,z]) translate([0,0,radius]) rotate([0,0,0]) lid();
    rotate([-9.2,23.1,z]) translate([0,0,radius]) rotate([0,0,180]) lid();
  }
  if (show_top) nipple();
  } // End CLIP

  mr() for (z=[0:90:A]) {
    difference() {
      union() {
        rotate([90,0,z+45]) {
         translate([0,0,radius-0.3]) rotate([-2,0,0]) clamp();
        }
      }
      if (!show_bottom || !show_top) {
        translate([0,0,show_top ? -20 : 0])
          cylinder(r=radius, h=20, $fn=Q*30);
      }
    }
  }
}

remote();


