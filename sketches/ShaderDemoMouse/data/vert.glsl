
#define PROCESSING_TEXTURE_SHADER 

// values from processing 
uniform mat4 transform;
uniform mat4 texMatrix;

attribute vec4 vertex; // position in local-space
attribute vec4 color; // vertex color
attribute vec2 texCoord; // uv

varying vec4 vertcolor;
varying vec4 vertTexCoord;

//runs once per vertex:
void main(){

    // gl_Position to the final vertex screen-space position:
    gl_Position = transform * vertex;

    vertcolor = color;

    vertTexCoord = texMatrix * vec4(texCoord.x, texCoord.y, 1, 1);


}

