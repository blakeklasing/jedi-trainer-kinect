# Blake Klasing and Javier Lores
# CAP6121 
# 2/20/2017
# 
#
# Work distrubtion:  work was distributed equally between both team members. 

this is where we train jedis...


## modes

- Training Mode: one droid, moving randomly, shooting lasers. 
- Attack Mode: many droids, constantly move towards you, not shooting. deal damage when they get close to you. 
- select items in menu by point at it and holding for x seconds

## world  

- some terrain  
- some sweet assets of starwars  
- trees  
- lighting  
 

# Lightsaber
- lightsaber moves with angles calculated with trigonemtry of hand positions. 
- kill enemies
- reflect lasers
- select lightsaber color in menu
  - MOTION: State 1: hands together, one on top of the other, take lightsaber out. 
            State 2: able to bring hands apart about a foot, wielding the lightsaber. 
            state 3: hands far apart from eachother, putting lightsaber back.
 
## Enemys 
- enemy models
- animations
- movement behavior
- damage/health system
- attack behavior

## connect Kinect  

- figure out whats up


## Force Gestures  

- Shoot lightning from hands
  - damage enemies
  - MOTION: Extend left arm out infront of you, must have hand above 
            hald the distance between neck and base of spine. 
            
- Grab objects to throw at enemies
  - HL2 explosive barrels               #TODO
  - MOTION: State 1: extend right arm out infront of you
            State 2: move arm around to select object
            State 3: object is grabbed and can be moved around 
  - highlight selection
  - raycast using the direction of the vector leading from your right elbow
    to your right hand with kinect data. 

- Send wave of energy  
  - knock down enemies infront of you in cone
  - MOTION: State 1: bring hands close to shoulders
            State 2: Push hands away from shoulders

- Heal
  - give x health
  - MOTION: raise both hands

- See in the future  
  - change terrain to a terrain from a future time
  - MOTION: state 1: semi-crouched position
            state 2: jump!

