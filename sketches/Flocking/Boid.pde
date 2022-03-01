class Boid {

  int TYPE = 1;

  PVector position = new PVector();
  PVector velocity = new PVector();
  PVector force = new PVector();

  PVector _dir = new PVector();
  
  float mass =1;
  float speed = 10;

  float raduisCohesion = 200;
  float raduisAlignment = 100;
  float raduisSeperation = 50;

  float forceCohesion = 1;
  float forceAlignment = .25;
  float forceSeperation = 10;

  Boid(float x, float y) {
    position.x = x;
    position.y = y;

    velocity.x = random(-3, 3);
    velocity.y = random(-3, 3);
  }

  void calcForces(Flock f) {

    // calculate forces

    //1. cohesion // pull towards group center
    //2. serperation // pushes boids apart
    // 3. allignment // turn boid to nearby avg direction

    //force that psuhes boids towards center
    // force that pushes voids away from sides
    //

    PVector centerOfGroup = new PVector();
    PVector avgAlignment = new PVector();
    int numCohesion = 0;
    int numAlignment = 0;

    for (Boid b : f.boids) {
      
      if(b == this) continue;
      
      float dx = b.position.x - position.x;
      float dy = b.position.y - position.y;
      float dis = sqrt(dx*dx + dy*dy); //pythagorean theorem

      if (dis < raduisCohesion) {
        centerOfGroup.add(b.position);
        numCohesion++;
      }
      
      if (dis < raduisSeperation) {
         PVector awayFromB = new PVector(-dx/dis, -dy/dis);
         awayFromB.mult(forceSeperation / dis);
         
         force.add(awayFromB);
       }
      if (dis < raduisAlignment) {
        avgAlignment.add(b._dir);
        numAlignment++;
      }
       
    }//end of for loop

    if (numCohesion > 0) {
      centerOfGroup.div(numCohesion);
      //steer towards group center

      PVector dirToCenter = PVector.sub(centerOfGroup, position);
      dirToCenter.setMag(speed);

      PVector cohesionForce = PVector.sub(dirToCenter, velocity);
      
      cohesionForce.limit(forceCohesion);
      force.add(cohesionForce);
    }
    
    if(numAlignment > 0){
      avgAlignment.div(numAlignment); //get average direction
      avgAlignment.mult(speed); //get desired vel = dir * max speed
      
      PVector alignmentForce = PVector.sub(avgAlignment, velocity);
      alignmentForce.limit(forceAlignment);
      force.add(alignmentForce);
    }
  }

  void updateAndDraw() {
    //euler step:
    PVector acceleration = PVector.div(force, mass);
    position.add(velocity);
    velocity.add(acceleration);
    force = new PVector(0, 0, 0);
    
    //loop on sides of screen:
    if(position.x < 0) position.x += width;
    else if (position.x > width) position.x -= width;
    
    if(position.y < 0) position.y += height;
    else if (position.y > height) position.y -= height;
    
    
    // cache the direction vector
    _dir = PVector.div(velocity, velocity.mag());

    ellipse(position.x, position.y, 10, 10);
  }
}
