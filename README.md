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
  - Lightsaber moves with angles calculated with trigonemtry of hand positions. 
  - Kill enemies
  - Reflect lasers
  - Select lightsaber color in menu
- MOTION: 
  - State 1: Hands together, one on top of the other, take lightsaber out. 
  - State 2: Able to bring hands apart about a foot, wielding the lightsaber. 
  - State 3: Hands far apart from eachother, putting lightsaber back.
 
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
    - State 1: Extend right arm out in front of you
    - State 2: Move arm around to select object
    - State 3: Nod head to the side to grab object and move around
  - Features
    - Highlight selection
    - Raycast using the direction of the vector leading from your right elbow
      to your right hand with kinect data.
  - Known Bugs:
     - Enemies don't get highlighted when selected 

- Send wave of energy  
  - knock down enemies in front of you in cone
  - MOTION:
    - State 1: Bring hands close to shoulders
    - State 2: Push hands away from shoulders

- Heal
  - give x health
  - MOTION: Raise both hands

- See in the future  
  - Travels between a planet during the Republic era and the Empire era.
  - MOTION:
    - State 1: Semi-crouched position
    - State 2: jump!

