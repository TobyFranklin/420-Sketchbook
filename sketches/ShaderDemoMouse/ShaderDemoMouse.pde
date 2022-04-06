
PImage img;
PShader shader;

void setup(){
 size(800, 600, P2D); 
 
 img = loadImage("panda.jpg");
 imageMode(CENTER);
 
 shader = loadShader("frag.glsl", "vert.glsl");
 //shader(shader);
}

void draw(){
  
  background(100);
  shader.set("mouse", mouseX/(float)width,1 - mouseY/(float)height);
 
  filter(shader);
}
