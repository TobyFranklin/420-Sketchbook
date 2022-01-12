void setup(){
  size(600,300);
}

void draw(){
  
  float time = millis() /1000.0;
  
  float d1 = map(sin(time), -1 , 1 , 50, 200);
  float d2 = random(50,200);
  float d3 = map(noise(time), 0 , 1, 50 ,200);
  
  ellipse(mouseX,mouseY, d3, d3);
  
}
