#include<Uduino.h>
Uduino uduino("RoverController");

int buttonInputPins[] = {53, 51, 49, 47};
int ledOutputPins[] = {52, 50, 48, 46};

void setup() {
  Serial.begin(9600);

  for(int i = 0;i<4;i++)
  {
    pinMode(buttonInputPins[i], INPUT_PULLUP);
    pinMode(ledOutputPins[i], OUTPUT);
  }
}

void loop() {
  uduino.update();

  String serialLine = "_";

  for(int i = 0;i<4;i++)
  {
    serialLine += String(digitalRead(buttonInputPins[i]));
  }


//Serial data structure
/**
0 - testpin1
1 - testPin2
2 - testPin3
3 - testPin4

**/
  uduino.println(serialLine);
  uduino.delay(15);
}
