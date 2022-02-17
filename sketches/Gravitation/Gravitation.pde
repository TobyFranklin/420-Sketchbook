
ArrayList<Agent> agents = new ArrayList<Agent>();

float G = 10;
float maxForce = 1;

void setup(){
 size(800,600); 
 
 //spawn agents
 for(int i = 0; i < 10; i++){
  agents.add(new Agent(10, 100) ); 
 }
 
 Agent sun = new Agent(1000, 2000);
 sun.position = new PVector(width/2, height/2);
 agents.add(sun);
 
 
}

void draw(){
  //update
  for (Agent a : agents){
  a.update(); 
 }
 
 //draw
 background(0);
 
 for (Agent a : agents){
   a.doneCalculatingGravity=false;
  a.draw(); 
 }
 
 
}
