
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
// background(100);s
 pushMatrix();
 translate(mouseX, mouseY);
 scale(.25);
 image(img, 0, 0);
 popMatrix();
 
  filter(shader);
}
