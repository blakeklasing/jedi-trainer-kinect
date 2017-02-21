# Blake Klasing and Javier Lores
# CAP6121 
# 2/20/2017
# 
#
# Work distrubtion:  work was distributed equally between both team members. 

This is where we train members of the Jedi...


## Modes

- Training Mode
    - Jedi Temple Scene
    - Known bugs:
       - Droid is not affected by Force powers
- Attack Mode: 
    - Republic Era Forest/Empire Era Lava

## Lightsaber
- Features
  - lightsaber moves with angles calculated with trigonemtry of hand positions. 
  - kill enemies
  - reflect lasers
  - select lightsaber color in menu
- MOTION: 
  - State 1: hands together, one on top of the other, take lightsaber out. 
  - State 2: able to bring hands apart about a foot, wielding the lightsaber. 
  - State 3: hands far apart from eachother, putting lightsaber back.
 
## Enemies
  - Features
     - Models
       - Republic Era
         - Clonetrooper
         - Sniper clonetrooper
       - Empire Era
         - Dual-wield stormtrooper
         - Unarmed stormtrooper
       - Training
         - Marksman-H training droid
     - Attack Behavior
       - Grunt
       - Sniper
       - Melee
       - Random
     - Health/Damage System
     - Animations
     - Movement Behavior

## Force Gestures  

- Shoot lightning from hands
  - MOTION: Extend left arm out in front of you, must have hand above 
            half the distance between neck and base of spine.
  - Features
    - Damages enemies
            
- Grab objects to throw at enemies
  - MOTION: 
    - State 1: extend right arm out in front of you
    - State 2: move arm around to select object
    - State 3: object is grabbed and can be moved around
  - Features
    - Highlight selection
    - raycast using the direction of the vector leading from your right elbow
      to your right hand with kinect data.
  - Known Bugs:
     - Enemies don't get highlighted when selected 

- Send wave of energy  
  - knock down enemies in front of you in cone
  - MOTION:
    - State 1: bring hands close to shoulders
    - State 2: Push hands away from shoulders

- Heal
  - give x health
  - MOTION: raise both hands

- See in the future  
  - Travels between a planet during the Republic era and the Empire era.
  - MOTION:
    - State 1: semi-crouched position
    - State 2: jump!

