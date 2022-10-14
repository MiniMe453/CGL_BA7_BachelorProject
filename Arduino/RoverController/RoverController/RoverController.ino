#include<Uduino.h>
Uduino uduino("RoverController");

int buttonInputPins[] = {53, 51, 49, 47, 45, 11};
int numOfDigitalInputs = 5;
int ledOutputPins[] = {22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50, 52};


void setup() {
  Serial.begin(9600);

  for(int i = 0;i<numOfDigitalInputs;i++)
  {
    pinMode(buttonInputPins[i], INPUT_PULLUP);
  }

  for(int i = 0;i<16;i++)
  {
    pinMode(ledOutputPins[i], OUTPUT);
  }

  uduino.addCommand("led", SetLEDPins);
}

void loop() {
  uduino.update();

  String serialLine = "_";

  for(int i = 0;i<numOfDigitalInputs;i++)
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

void SetLEDPins()
{
  int numOfParams = uduino.getNumberOfParameters();
  char *arg;
  arg = uduino.next();

  if(numOfParams > 0)
  {
    for(int i = 0;i<numOfParams;i++)
    {
      if(arg != NULL)
        digitalWrite(ledOutputPins[i], uduino.charToInt(arg));

      arg = uduino.next();
    }
  }
}